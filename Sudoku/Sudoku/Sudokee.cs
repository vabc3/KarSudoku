using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sudoku
{
    public class Sudokee : ICloneable
    {
        #region Fields
        public int BaseSize;
        public int Size;
        public int BoardSize;
        private int[] board;
        private List<IList<int>> indexGroups;
        private HashSet<int>[] allowed, affected;

        #endregion

        #region Constructor
        public Sudokee(int BaseSize = 3)
        {
            this.BaseSize = BaseSize;
            this.Size = BaseSize * BaseSize;
            this.BoardSize = Size * Size;
            this.board = new int[BoardSize];
            InitIndexGroups();

            allowed = new HashSet<int>[BoardSize];
            for (var i = 0; i < BoardSize; i++)
            {
                allowed[i] = new HashSet<int>();
                for (var j = 1; j <= Size; j++) allowed[i].Add(j);
            }
            affected = new HashSet<int>[BoardSize];
            for (var i = 0; i < BoardSize; i++)
            {
                affected[i] = new HashSet<int>();
                foreach (var item in indexGroups)
                {
                    if (item.Contains(i))
                        foreach (var it in item)
                            affected[i].Add(it);
                }
            }
        }
        #endregion



        #region Public methods


        public int[] GetRaw()
        {
            return board;
        }

        public ISet<int> AllowedNums(int index)
        {
            return allowed[index];
        }

        public object Clone()
        {
            var a = MemberwiseClone() as Sudokee;

            a.board = new int[BoardSize];
            Array.Copy(board, a.board, BoardSize);

            return a;
        }

        public override string ToString()
        {
            return IdxToString() + AffToString();
        }

        public int this[int x, int y]
        {
            get
            {
                CheckRange(x, Size);
                CheckRange(y, Size);
                return board[y * Size + x];
            }
            set
            {
                CheckRange(x, Size);
                CheckRange(y, Size);
                CheckRange(value, Size + 1);
                board[y * Size + x] = value;
                ch(y * Size + x);
            }
        }

        public int this[int index]
        {
            get
            {
                CheckRange(index, BoardSize);
                return board[index];
            }
            set
            {
                CheckRange(index, BoardSize);
                CheckRange(value, Size + 1);
                board[index] = value;
                ch(index);
            }
        }

        private void ch(int index)
        {
            var grp = affected[index].Where(item => (board[item] == 0));
            foreach (var item in grp)
                allowed[item].Remove(board[index]);
        }
        #endregion

        #region private methods

        private string IdxToString()
        {
            StringBuilder sb = new StringBuilder();
            int k = 0;
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                    sb.AppendFormat("{0}\t", k++);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string AffToString()
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < Size; i++)
            {
                sb.AppendFormat("{0}:", i);
                foreach (var item in affected[i])
                    sb.AppendFormat("{0}\t", item);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private string IGToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in indexGroups)
            {
                foreach (var it in item)
                    sb.AppendFormat("{0}\t", it);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string DataToString()
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                    sb.AppendFormat("{0}\t", this[j, i]);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private void CheckRange(int item, int upper, int lower = 0)
        {//[,]
            if (item < lower || item >= upper)
                throw new ApplicationException("out of range");
        }

        private void CheckRange(params int[] list)
        {
            foreach (var item in list)
                if (item < 0 || item > Size)
                    throw new ApplicationException("out of range");
        }

        private void InitIndexGroups()
        {
            indexGroups = new List<IList<int>>();
            for (var i = 0; i < Size; i++)
            {
                var list = new List<int>();
                for (var j = 0; j < Size; j++)
                {
                    list.Add(i * Size + j);
                }
                indexGroups.Add(list);
            }

            for (var i = 0; i < Size; i++)
            {
                var list = new List<int>();
                for (var j = 0; j < Size; j++)
                {
                    list.Add(j * Size + i);
                }
                indexGroups.Add(list);
            }

            for (var i = 0; i < Size; i += BaseSize)
            {
                for (var j = 0; j < Size; j += BaseSize)
                {
                    var li = new List<int>();
                    for (var k = 0; k < BaseSize; k++)
                    {
                        for (var l = 0; l < BaseSize; l++)
                        {
                            li.Add((i + k) * Size + j + l);
                        }
                    }
                    indexGroups.Add(li);
                }
            }
        }

        private string getIndexStr(int index)
        {
            return string.Format("({0},{1})", index % Size, index / Size);
        }
        #endregion
    }
}