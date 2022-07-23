using System;
using System.Collections.Generic;

namespace SeaFight.Boards
{
    public class BoardOf<TElem>
    {
        public delegate bool CellSelector(TElem value);


        public BoardOf(int columns, int rows) {
            Columns = columns;
            Rows = rows;
            Cells = new TElem[Area];
        }


        public int Area => Columns * Rows;

        public TElem[] Cells { get; }

        public int Columns { get; }

        public Row this[int row] => new Row(this, row);

        public TElem this[(int, int) point]
        {
            get => Cells[ToPlain(point)];
            set => Cells[ToPlain(point)] = value;
        }

        public int Rows { get; }


        public bool Contains(int col, int row) { return col < Columns && row < Rows && col >= 0 && row >= 0; }


        public bool Contains((int col, int row) point) { return Contains(point.col, point.row); }


        public IEnumerable<(int col, int row)> FindSolidShape((int col, int row) start, CellSelector selector) {
            if (!selector(this[start])) yield break;

            yield return start;


            var units = new (int col, int row)[] {
                (0, -1), // Up
                (0, 1), // Down
                (-1, 0), // Left
                (1, 0) // Right
            };
            foreach (var directionUnit in units)
            foreach (var (col, row, value) in Move(start, directionUnit)) {
                if (!selector(value)) break;
                yield return (col, row);
            }
        }


        public IEnumerable<(int col, int row, TElem value)> NeighborsOf((int col, int row) point) {
            for (var row = Math.Max(0, point.row - 1); row <= Math.Min(point.row + 1, Rows - 1); row++)
            for (var col = Math.Max(0, point.col - 1); col <= Math.Min(point.col + 1, Columns - 1); col++) {
                if (point.col == col && point.row == row) continue;
                yield return (col, row, this[col][row]);
            }
        }


        public void ResetBoard(TElem withValue) {
            for (var idx = 0; idx < Area; idx++) Cells[idx] = withValue;
        }


        public IEnumerable<(int col, int row)> SelectCells(CellSelector selector) {
            for (var row = 0; row < Rows; row++)
            for (var col = 0; col < Columns; col++) {
                var plain = ToPlain(col, row);
                if (selector(Cells[plain])) yield return (col, row);
            }
        }


        public int ToPlain(int col, int row) { return row * Columns + col; }


        public int ToPlain((int col, int row) point) { return ToPlain(point.col, point.row); }


        private IEnumerable<(int col, int row, TElem value)> Move((int col, int row) start, (int col, int row) unit) {
            var cursor = start;
            while (Contains(cursor)) {
                if (cursor != start)
                    yield return (cursor.col, cursor.row, this[cursor]);
                cursor = (start.col + unit.col, start.row + unit.row);
            }
        }


        public class Row
        {
            private readonly BoardOf<TElem> m_board;
            private readonly int m_column;


            internal Row(BoardOf<TElem> board, int column) {
                m_board = board;
                m_column = column;
            }


            public TElem this[int row]
            {
                get => m_board.Cells[m_board.ToPlain(m_column, row)];
                set => m_board.Cells[m_board.ToPlain(m_column, row)] = value;
            }
        }
    }
}