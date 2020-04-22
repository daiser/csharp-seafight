using System.Collections.Generic;
using System.Linq;

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
        protected int Plain(Pos cell)
        {
            return cell.Plain(Dim);
        }
        public Pos Unplain(int idx)
        {
            return new Pos(idx % Dim, idx / Dim);
        }
        public TElem At(int col, int row)
        {
            return cells[Plain(col, row)];
        }
        public TElem At(Pos cell)
        {
            return cells[Plain(cell)];
        }

        public bool Contains(Pos cell)
        {
            return cell.Col < Dim && cell.Row < Dim && cell.Col >= 0 && cell.Row >= 0;
        }

        public void Set(int col, int row, TElem value)
        {
            cells[Plain(col, row)] = value;
        }
        public void Set(Pos cell, TElem value)
        {
            cells[Plain(cell)] = value;
        }

        public Cell<TElem>[] FindSolid(Pos start, params TElem[] values)
        {
            List<Cell<TElem>> cells = new List<Cell<TElem>>();

            var cellVal = At(start);
            if (values.Contains(cellVal))
            {
                cells.Add(new Cell<TElem>(start, cellVal));

                for (int col = start.Col - 1; col >= 0; col--)
                {
                    cellVal = At(col, start.Row);
                    if (values.Contains(cellVal)) cells.Add(new Cell<TElem>(col, start.Row, cellVal));
                    else break;
                }
                for (int col = start.Col + 1; col < Dim; col++)
                {
                    cellVal = At(col, start.Row);
                    if (values.Contains(cellVal)) cells.Add(new Cell<TElem>(col, start.Row, cellVal));
                    else break;
                }
                for (int row = start.Row - 1; row >= 0; row--)
                {
                    cellVal = At(start.Col, row);
                    if (values.Contains(cellVal)) cells.Add(new Cell<TElem>(start.Col, row, cellVal));
                    else break;
                }
                for (int row = start.Row + 1; row < Dim; row++)
                {
                    cellVal = At(start.Col, row);
                    if (values.Contains(cellVal)) cells.Add(new Cell<TElem>(start.Col, row, cellVal));
                    else break;
                }
            }

            return cells.ToArray();
        }
    }
}
