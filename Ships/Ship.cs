using System;
using System.Collections.Generic;
using System.Linq;
using SeaFight.Boards;

namespace SeaFight.Ships
{
    public class Ship
    {
        private readonly (int col, int row, CellState state)[] m_cells;


        public Ship(IEnumerable<(int col, int row, CellState state)> cells) {
            m_cells = cells.ToArray();
            var columns = m_cells.Select(c => c.col).Distinct().ToArray();
            var rows = m_cells.Select(c => c.row).Distinct().ToArray();

            if (columns.Length > 1 && rows.Length > 1) throw new ArgumentException("multi col/row ship");

            var shouldBeOrdered = columns.Length == 1 ? columns : rows;
            for (var i = shouldBeOrdered.Min() + 1; i < shouldBeOrdered.Max(); i++)
                if (!shouldBeOrdered.Contains(i))
                    throw new ArgumentException("ship has gaps");
        }


        public bool IsDead => m_cells.All(c => c.state == CellState.Hit);


        public CellState TakeShot(Shot shot) {
            foreach (var cell in m_cells) {
                var valueTuple = cell;
                if (valueTuple.col != shot.Target.col || valueTuple.row != shot.Target.row) continue;
                valueTuple.state = CellState.Hit;
                return IsDead ? CellState.Kill : CellState.Hit;
            }
            return CellState.Miss;
        }
    }
}