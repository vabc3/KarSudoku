using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SudokuWPF
{
    class SudokuCell
    {
        private bool isLeaf;
        private int index;
        private int _content;
        public int Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                if (ui != null)
                    ui.Content = _content > 0 ? _content.ToString() : "";
            }
        }

        private IList<SudokuCell> children;
        private Label ui;
        private Path border;
        private UIElementCollection panel;
        private int x, y;
        private int bsize;
        SudokuBoard sb;

        public SudokuCell(UIElementCollection panel, int bsize, SudokuBoard sb)
        {
            this.sb = sb;
            this.bsize = bsize;
            this.children = new List<SudokuCell>();
            this.panel = panel;
            isLeaf = false;
            int size = bsize * bsize;
            int bss = size * size;
            int[] data = new int[bss];
            for (var i = 0; i < bss; i++) data[i] = i;

            for (var i = 0; i < bsize; i++)
            {
                for (var j = 0; j < bsize; j++)
                {
                    var t = new List<int>();
                    for (var k = i * bsize; k < (i + 1) * bsize; k++)
                    {
                        for (var l = j * bsize; l < (j + 1) * bsize; l++)
                        {
                            t.Add(data[k * size + l]);
                        }
                    }
                    var sc = new SudokuCell(this.panel, t.ToArray(), j, i, bsize, sb);
                    children.Add(sc);
                }
            }
        }

        private SudokuCell(UIElementCollection panel, int[] data, int x, int y, int bsize, SudokuBoard sb)
        {
            this.sb = sb;
            this.bsize = bsize;
            this.panel = panel;
            this.x = x;
            this.y = y;
            if (data.Length > 1)
            {
                isLeaf = false;
                this.children = new List<SudokuCell>();
                for (var i = 0; i < bsize; i++)
                    for (var j = 0; j < bsize; j++)
                    {
                        var sc = new SudokuCell(panel, new int[] { data[i * bsize + j] }, j, i, bsize, sb);
                        children.Add(sc);
                    }
            }
            else
            {
                isLeaf = true;
                this.index = data[0];
            }
        }

        private void AddUI()
        {
            ui = new Label();
            ui.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.panel.Add(ui);
        }

        private void AddMouse()
        {
            ui.MouseUp += ui_MouseUp;
            ui.MouseEnter += ui_MouseEnter;
            ui.MouseLeave += ui_MouseLeave;
        }

        private void DelMouse()
        {
            ui.MouseUp -= ui_MouseUp;
            ui.MouseEnter -= ui_MouseEnter;
            ui.MouseLeave -= ui_MouseLeave;
        }
        void ui_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = null;
        }

        void ui_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = Brushes.Green;
        }

        void ui_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            sb.Change(index, Content);
        }

        private static void trace(int[] data, int x, int y)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0},{1}:", x, y);
            for (var i = 0; i < data.Length; i++)
                sb.AppendFormat("{0},", data[i]);
            sb.AppendLine();
            Trace.WriteLine(sb.ToString());
        }

        public void Load(int[] data)
        {
            if (isLeaf)
            {
                int d = data[index];
                if (d > 0)
                {

                    if (children != null)
                    {
                        foreach (var item in children)
                        {
                            panel.Remove(item.ui);
                            panel.Remove(item.border);
                        }
                        children.Clear();
                        children = null;
                    }
                    if (ui == null)
                    {
                        AddUI();
                        updateUI();

                        //DoubleAnimation oLabelAngleAnimation = new DoubleAnimation();
                        //oLabelAngleAnimation.From = 0;
                        //oLabelAngleAnimation.To = 360;
                        //oLabelAngleAnimation.Duration
                        //  = new Duration(new TimeSpan(0, 0, 0, 0, 500));
                        //oLabelAngleAnimation.RepeatBehavior = new RepeatBehavior(4);
                        //ui.LayoutTransform=new System.Windows.Media.ScaleTransform(40 * 1, 1);
                        //ScaleTransform oTransform = ui.LayoutTransform   as ScaleTransform;
                        //oTransform.BeginAnimation(RotateTransform.AngleProperty, oLabelAngleAnimation);


                    }
                    this.Content = d;
                }
                else
                {
                    if (ui != null)
                        panel.Remove(ui);
                    if (children == null)
                    {
                        children = new List<SudokuCell>();
                        var al = sb.Allow(index);
                        for (var i = 0; i < bsize; i++)
                        {
                            for (var j = 0; j < bsize; j++)
                            {
                                var dig = i * bsize + j + 1;
                                var sc = new SudokuCell(panel, new int[] { index }, j, i, bsize, sb);
                                sc.AddUI();
                                sc.Content = al.Contains(dig) ? dig : 0;
                                if (sc.Content > 0) sc.AddMouse();
                                children.Add(sc);


                            }
                        }
                    }
                    else
                    {
                        var al = sb.Allow(index);
                        foreach (var item in children)
                        {
                            var dig = item.y * bsize + item.x + 1;
                            item.Content = al.Contains(dig) ? dig : 0;
                            if (item.Content == 0) item.DelMouse();
                        }
                    }
                }
            }
            else
                foreach (var item in children)
                    item.Load(data);
        }
        private double left, top, width, height;

        public void Resize(double left, double top, double width, double height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;

            if (children == null)
                updateUI();
            else
            {
                var dx = width / bsize;
                var dy = height / bsize;
                foreach (var item in children)
                    item.Resize(left + item.x * dx, top + item.y * dy, dx, dy);
            }

            panel.Remove(border);
            border = create(.01 * width, Brushes.Black);
            panel.Add(border);
        }

        private void updateUI()
        {
            if (ui != null && height > 0 && width > 0)
            {
                ui.Height = height;
                ui.Width = width;
                ui.FontSize = .6 * height;
                Canvas.SetTop(ui, top);
                Canvas.SetLeft(ui, left);
            }
        }

        private Path create(double thick, Brush bs)
        {
            return new Path
            {
                Stroke = bs,
                StrokeThickness = thick,
                Data = new RectangleGeometry
                {
                    Rect = new Rect(left, top, width, height)
                }
            };
        }
    }
}
