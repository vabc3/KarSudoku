using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SudokuWPF
{
    class SudokuBoard
    {
        private Sudokee sudokee;
        private SudokuCell cell;
        private SudokuManager sm = SudokuManager.Instance;

        public SudokuBoard(UIElementCollection coll)
        {
            string file = @"g:\1.txt";
            try
            {
                sudokee = sm.LoadFromFile(file);
            }
            catch (Exception)
            {
                MessageBox.Show("Error during loading.", "Error");
            }

            cell = new SudokuCell(coll,sudokee.BaseSize,this);
        }

        public void Resize(double left, double top, double width, double height)
        {
            cell.Resize(left, top, width, height);
        }

        public void Update()
        {
            cell.Load(sudokee.GetRaw());
        }

        public void Change(int index, int tv)
        {
            sudokee[index] = tv;
            Update();
        }

        public ISet<int> Allow(int index)
        {
            return sudokee.AllowedNums(index);
        }
    }
}
