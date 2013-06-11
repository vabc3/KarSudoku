using Sudoku;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SudokuWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private fields
        private const double Factor = 0.95;
        private double canvasHeight, canvasWidth;
        private double boardTop, boardLeft;
        private double boardHeight, boardWidth;
        private SudokuBoard sb;
        #endregion

        #region cstr
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            sb.Update();
        }
        #endregion

        #region Action
        private void Canvas1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas_update();
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            Canvas_update();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            sb.Update();
            Canvas_update();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Private Methods
        private void LoadData()
        {
            sb = new SudokuBoard(Canvas1.Children);
            Canvas1.Children.Clear();
        }

        private void Canvas_update()
        {
            canvasHeight = Canvas1.ActualHeight;
            canvasWidth = Canvas1.ActualWidth;
            if (1.0 > canvasWidth / canvasHeight)
                boardWidth = boardHeight = canvasWidth * Factor;//Depends on width
            else
                boardWidth = boardHeight = canvasHeight * Factor;

            boardTop = (canvasHeight - boardHeight) / 2;
            boardLeft = (canvasWidth - boardWidth) / 2;

            if (boardHeight > 0 && boardWidth > 0)
                sb.Resize(boardLeft, boardTop, boardWidth, boardHeight);
        }
        #endregion
    }
}
