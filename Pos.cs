using System;
using System.Collections.Generic;

namespace SeaFight
{
    class Pos
    {
        public readonly int Col;
        public readonly int Row;


        public Pos() { }


        public Pos(int col, int row) {
            Col = col;
            Row = row;
        }


        public Pos[] Neighbours() {
            var cells = new List<Pos>();
            for (var col = -1; col < 2; col++) {
                for (var row = -1; row < 2; row++) {
                    if (row == 0 && col == 0) continue;
                    cells.Add(new Pos(Col + col, Row + row));
                }
            }
            return cells.ToArray();
        }


        public Pos Add(Pos other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return new Pos(Col + other.Col, Row + other.Row);
        }


        public Pos[] Make(in Orientation orientation, in int size) {
            var cells = new List<Pos> { this };
            for (var i = 1; i < size; i++) {
                cells.Add(new Pos(Col + (orientation == Orientation.Column ? i : 0), Row + (orientation == Orientation.Row ? i : 0)));
            }

            return cells.ToArray();
        }


        public Pos[] MakeRow(in int size) { return Make(Orientation.Row, size); }


        public Pos[] MakeColumn(in int size) { return Make(Orientation.Column, size); }


        public override bool Equals(object obj) {
            if (!(obj is Pos o)) return false;
            return o.Col == Col && o.Row == Row;
        }


        public override int GetHashCode() { return (Col ^ Row).GetHashCode(); }


        public int Plain(int rowSize) { return Row * rowSize + Col; }


        public override string ToString() { return $"({Col:d},{Row:d})"; }
    }
}