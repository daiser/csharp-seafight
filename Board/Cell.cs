using System;

namespace SeaFight.Board
{
    public class Cell<TValue>: Point
    {
        private const string NULL_VALUE = "<null>";

        public readonly TValue Value;


        public Cell() { }


        public Cell(int col, int row): base(col, row) { }


        public Cell(int col, int row, TValue value): base(col, row) { Value = value; }


        public Cell(Cell<TValue> other): base(other.Col, other.Row) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            Value = other.Value;
        }


        public Cell(Point point, TValue value): base(point.Col, point.Row) {
            if (point == null) throw new ArgumentNullException(nameof(point));
            Value = value;
        }


        public override bool Equals(object obj) {
            var result = false;
            var cell = obj as Cell<TValue>;
            if (obj is Point pos) result = base.Equals(pos);
            if (cell != null) result = result && Value != null && Value.Equals(cell.Value);
            return result;
        }


        public override int GetHashCode() {
            var hash = base.GetHashCode();
            if (Value != null) hash ^= Value.GetHashCode();
            return hash;
        }


        public override string ToString() { return $"({Col:d},{Row:d})={(object)Value ?? NULL_VALUE}"; }
    }
}