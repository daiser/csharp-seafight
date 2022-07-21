using System;
using System.Collections.Generic;

namespace SeaFight.Board
{
    public class Point
    {
        public readonly int Col;
        public readonly int Row;


        public Point() { }


        public Point(int col, int row) {
            Col = col;
            Row = row;
        }


        public Point[] Neighbours() {
            var cells = new List<Point>();
            for (var col = -1; col < 2; col++) {
                for (var row = -1; row < 2; row++) {
                    if (row == 0 && col == 0) continue;
                    cells.Add(new Point(Col + col, Row + row));
                }
            }
            return cells.ToArray();
        }


        public Point Add(Point other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return new Point(Col + other.Col, Row + other.Row);
        }


        public Point[] Make(in Orientation orientation, in int size) {
            var cells = new List<Point> { this };
            for (var i = 1; i < size; i++) {
                cells.Add(new Point(Col + (orientation == Orientation.Column ? i : 0), Row + (orientation == Orientation.Row ? i : 0)));
            }

            return cells.ToArray();
        }


        public Point[] MakeRow(in int size) { return Make(Orientation.Row, size); }


        public Point[] MakeColumn(in int size) { return Make(Orientation.Column, size); }


        public override bool Equals(object obj) {
            if (!(obj is Point o)) return false;
            return o.Col == Col && o.Row == Row;
        }


        public override int GetHashCode() { return (Col ^ Row).GetHashCode(); }


        public override string ToString() { return $"({Col:d},{Row:d})"; }
    }
}