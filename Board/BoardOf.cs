using System.Collections.Generic;
using System.Linq;

namespace SeaFight.Board
{
    public class BoardOf<TElem>
    {
        public TElem[] Cells { get; }

        public int XDim { get; }

        public int YDim { get; }

        public int Size => XDim * YDim;


        public BoardOf(int xDim, int yDim) {
            XDim = xDim;
            YDim = yDim;
            Cells = new TElem[Size];
        }


        protected int ToPlain(int col, int row) { return row * YDim + col; }


        protected int ToPlain(Point cell) { return ToPlain(cell.Col, cell.Row); }


        public Point ToPosition(int idx) { return new Point(idx % YDim, idx / YDim); }


        public TElem At(int col, int row) { return Cells[ToPlain(col, row)]; }


        public TElem At(Point cell) { return Cells[ToPlain(cell)]; }


        public bool Contains(Point cell) { return cell.Col < YDim && cell.Row < XDim && cell.Col >= 0 && cell.Row >= 0; }


        public void Set(int col, int row, TElem value) { Cells[ToPlain(col, row)] = value; }


        public void Set(Point cell, TElem value) { Cells[ToPlain(cell)] = value; }


        public void ResetBoard(TElem newValue) {
            for (var idx = 0; idx < Size; idx++) {
                Cells[idx] = newValue;
            }
        }


        public Cell<TElem>[] FindSolid(Point start, params TElem[] values) {
            var cells = new List<Cell<TElem>>();

            var cellVal = At(start);
            if (values.Contains(cellVal)) {
                cells.Add(new Cell<TElem>(start, cellVal));

                for (var col = start.Col - 1; col >= 0; col--) {
                    cellVal = At(col, start.Row);
                    if (values.Contains(cellVal)) cells.Add(new Cell<TElem>(col, start.Row, cellVal));
                    else break;
                }
                for (var col = start.Col + 1; col < YDim; col++) {
                    cellVal = At(col, start.Row);
                    if (values.Contains(cellVal)) cells.Add(new Cell<TElem>(col, start.Row, cellVal));
                    else break;
                }
                for (var row = start.Row - 1; row >= 0; row--) {
                    cellVal = At(start.Col, row);
                    if (values.Contains(cellVal)) cells.Add(new Cell<TElem>(start.Col, row, cellVal));
                    else break;
                }
                for (var row = start.Row + 1; row < XDim; row++) {
                    cellVal = At(start.Col, row);
                    if (values.Contains(cellVal)) cells.Add(new Cell<TElem>(start.Col, row, cellVal));
                    else break;
                }
            }

            return cells.ToArray();
        }
    }
}