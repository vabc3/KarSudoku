using System.IO;
using System.Linq;

namespace Sudoku
{
    public class SudokuManager
    {
        private SudokuManager() { }
        public static SudokuManager Instance = new SudokuManager();

        public Sudokee LoadFromFile(string path)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);
                using (var sw = new StreamReader(fs))
                {
                    fs = null;
                    var ba = int.Parse(sw.ReadLine());
                    var si = ba * ba;
                    var s = new Sudokee(ba);

                    for (var i = 0; i < si; i++)
                    {
                        var line = sw.ReadLine();
                        var data = line.Split(',');
                        var dat = data.Select(item => int.Parse(item));
                        int j = 0;
                        foreach (var item in dat)
                        {
                            if (item > 0) s[j, i] = item;
                            j++;
                        }
                    }
                    return s;
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        public void SaveToFile(Sudokee skd, string path)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Create);
                using (var sw = new StreamWriter(fs))
                {
                    fs = null;
                    sw.WriteLine(skd.BaseSize);
                    int l = skd.Size;
                    for (var i = 0; i < l; i++)
                    {
                        sw.Write(skd[0, i]);
                        for (var j = 1; j < l; j++)
                            sw.Write(",{0}", skd[j, i]);
                        sw.WriteLine();
                    }
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }
    }
}
