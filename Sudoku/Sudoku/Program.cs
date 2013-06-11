using System;

namespace Sudoku
{
    class Program
    {
        private static string fp = @"g:\1.txt";
        static void Main(string[] args)
        {
            var a = new Sudokee(3);
            Console.WriteLine(a);
            //a[2, 1] = 1;
            //var sm = SudokuManager.Instance;
            //sm.SaveToFile(a,fp);
            //var b = sm.LoadFromFile(fp);
            //Console.WriteLine(b);

        }
    }
}
