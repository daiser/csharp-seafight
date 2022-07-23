﻿using System.Collections.Generic;
using System.Linq;
using SeaFight.Ships;

namespace SeaFight.Boards
{
    public class ShipBoard: SquareBoardOf<byte>
    {
        private const byte FREE = 0;
        private const byte OCCUPIED = 1;
        private readonly List<Ship> m_ships = new List<Ship>();


        public ShipBoard(int dim): base(dim) { }


        public Fleet Commit() { return new Fleet(m_ships); }


        public IEnumerable<(int col, int row, bool horizontal)> GetFreeCells(int shipSize) {
            foreach (var freeCell in SelectCells(c => c == FREE)) {
                // Cell is free and all neighbors are free.
                if (Horizontal(freeCell, shipSize).All(c => Contains(c) && this[c] == FREE && NeighborsOf(c).All(n => n.value == FREE)))
                    yield return (freeCell.col, freeCell.row, true);
                if (Vertical(freeCell, shipSize).All(c => Contains(c) && this[c] == FREE && NeighborsOf(c).All(n => n.value == FREE)))
                    yield return (freeCell.col, freeCell.row, false);
            }
        }


        public bool PlaceShip((int col, int row) atPosition, int size, bool horizontal = false) {
            var cells = (horizontal ? Horizontal(atPosition, size) : Vertical(atPosition, size)).ToArray();

            // Checking if cells and there neighbors are available.
            if (cells.Any(p => this[p] == OCCUPIED || NeighborsOf(p).Any(n => n.value == OCCUPIED))) return false;

            // Tagging as occupied.
            foreach (var cell in cells) {
                this[cell] = OCCUPIED;
                foreach (var (col, row, _) in NeighborsOf(cell)) this[col][row] = OCCUPIED;
            }

            m_ships.Add(new Ship(cells.Select(p => (p.col, p.row, CellState.None))));
            return true;
        }


        private static IEnumerable<(int col, int row)> Horizontal((int col, int row) start, int size) {
            for (var col = 0; col < size; col++) yield return (start.col + col, start.row);
        }


        private static IEnumerable<(int col, int row)> Vertical((int col, int row) start, int size) {
            for (var row = 0; row < size; row++) yield return (start.col, start.row + row);
        }
    }
}