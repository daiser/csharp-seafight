using System;
using System.Collections.Generic;

namespace SeaFight
{
    class Pos
    {
        public int Col;
        public int Row;

        public Pos() { }
        public Pos(int col, int row)
        {
            Col = col;
            Row = row;
        }
        public Pos[] Neighbours()
        {
            List<Pos> cells = new List<Pos>();
            for (int col = -1; col < 2; col++)
            {
                for (int row = -1; row < 2; row++)
                {
                    if (row == 0 && col == 0) continue;
                    cells.Add(new Pos { Col = Col + col, Row = Row + row });
                }
            }
            return cells.ToArray();
        }

        public Pos Add(Pos other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return new Pos { Col = Col + other.Col, Row = Row + other.Row };
        }

        public Pos[] Make(in Orientation orientation, in int size)
        {
            List<Pos> cells = new List<Pos> { this };
            for (int i = 1; i < size; i++)
            {
                cells.Add(new Pos { Col = Col + (orientation == Orientation.Column ? i : 0), Row = Row + (orientation == Orientation.Row ? i : 0) });
            }

            return cells.ToArray();
        }

        public Pos[] MakeRow(in int size)
        {
            return Make(Orientation.Row, size);
        }
        public Pos[] MakeColumn(in int size)
        {
            return Make(Orientation.Column, size);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Pos o)) return false;
            return o.Col == Col && o.Row == Row;
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
}
