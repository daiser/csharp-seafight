using System.Collections.Generic;

namespace SeaFight
{
    class Cell
    {
        public int Col;
        public int Row;

        public Cell() { }
        public Cell(int col, int row)
        {
            Col = col;
            Row = row;
        }
        public Cell[] Neighbours()
        {
            List<Cell> cells = new List<Cell>();
            for (int col = -1; col < 2; col++)
            {
                for (int row = -1; row < 2; row++)
                {
                    if (row == 0 && col == 0) continue;
                    cells.Add(new Cell { Col = Col + col, Row = Row + row });
                }
            }
            return cells.ToArray();
        }

        public Cell[] Make(in Orientation orientation, in int size)
        {
            List<Cell> cells = new List<Cell> { this };
            for (int i = 1; i < size; i++)
            {
                cells.Add(new Cell { Col = Col + (orientation == Orientation.Column ? i : 0), Row = Row + (orientation == Orientation.Row ? i : 0) });
            }

            return cells.ToArray();
        }

        public Cell[] MakeRow(in int size)
        {
            return Make(Orientation.Row, size);
        }
        public Cell[] MakeColumn(in int size)
        {
            return Make(Orientation.Column, size);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Cell o)) return false;
            return o.Col==Col&&o.Row==Row;
        }
        public override int GetHashCode()
        {
            return (Col ^ Row).GetHashCode();
        }

        public int Plain(int row_size)
        {
            return Row * row_size + Col;
        }

        public override string ToString()
        {
            return string.Format(string.Format("({0:d},{1:d})", Col, Row));
        }
    }

    class Target : Cell
    {
        public bool Live = false;
        public Target(int col, int row) : base()
        {
            Col = col;
            Row = row;
        }
    }
}
