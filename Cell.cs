using System;

namespace SeaFight
{
    class Cell<TValue> : Pos
    {
        private const string NULL_VALUE = "<null>";

        public TValue Value;
        public Cell() { }
        public Cell(int col, int row) : base(col, row) { }
        public Cell(int col, int row, TValue value) : base(col, row)
        {
            Value = value;
        }
        public Cell(Cell<TValue> other)
        {
            if (other == null) throw new ArgumentNullException("other");
            Col = other.Col;
            Row = other.Row;
            Value = other.Value;
        }
        public Cell(Pos pos, TValue value)
        {
            if (pos == null) throw new ArgumentNullException("pos");
            Col = pos.Col;
            Row = pos.Row;
            Value = value;
        }
        public override bool Equals(object obj)
        {
            bool result = false;
            Pos pos = obj as Pos;
            Cell<TValue> cell = obj as Cell<TValue>;
            if (pos != null) result = base.Equals(pos);
            if (cell != null) result = result && Value != null && Value.Equals(cell.Value);
            return result;
        }
        public override int GetHashCode()
        {
            int hash = base.GetHashCode();
            if (Value != null) hash ^= Value.GetHashCode();
            return hash;
        }
        public override string ToString()
        {
            return string.Format("({0:d},{1:d})={2}", Col, Row, (object)Value ?? NULL_VALUE);
        }
    }
}
