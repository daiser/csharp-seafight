namespace SeaFight
{
    class BoardOf<TElem>
    {
        protected readonly TElem[] cells;
        public TElem[] Cells { get { return cells; } }
        public int Dim { get; private set; }
        public int Size { get; private set; }

        public BoardOf(int dim)
        {
            Dim = dim;
            Size = dim * dim;
            cells = new TElem[Size];
        }

        protected int Plain(int col, int row)
        {
            return row * Dim + col;
        }
        protected int Plain(Cell cell)
        {
            return cell.Plain(Dim);
        }
        public Cell Unplain(int idx)
        {
            return new Cell(idx % Dim, idx / Dim);
        }
        public TElem At(int col, int row)
        {
            return cells[Plain(col, row)];
        }
        public TElem At(Cell cell)
        {
            return cells[Plain(cell)];
        }

        public bool Contains(Cell cell)
        {
            return cell.Col < Dim && cell.Row < Dim && cell.Col >= 0 && cell.Row >= 0;
        }

        public void Set(int col, int row, TElem value)
        {
            cells[Plain(col, row)] = value;
        }
        public void Set(Cell cell, TElem value)
        {
            cells[Plain(cell)] = value;
        }
    }
}
