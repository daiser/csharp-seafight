using System;
using System.Collections.Generic;
using System.Linq;
using SeaFight.Armada;

namespace SeaFight.Board
{
    class Board: SquareBoardOf<byte>
    {
        public const byte CELL_FREE = 0;
        public const byte CELL_SHIP = 1;


        public Board(in int dim): base(dim) { }


        private bool CheckPlacement(IEnumerable<Pos> places) {
            foreach (var shipCell in places) {
                if (!Contains(shipCell)) return false;
                if (At(shipCell) != CELL_FREE) return false;
                var neighbours = shipCell.Neighbours();
                if (neighbours.Where(Contains).Any(neightbour => At(neightbour) != CELL_FREE)) {
                    return false;
                }
            }
            return true;
        }


        public Pos[] PlaceShip(in Pos at, in int shipSize, in Orientation orientation) {
            var placement = at.Make(orientation, shipSize);

            if (!CheckPlacement(placement)) return null;

            foreach (var shipCell in placement) {
                Set(shipCell, CELL_SHIP);
            }
            return placement;
        }


        public void Reset() { ResetBoard(CELL_FREE); }


        public Pos[] PlaceShipRandom(in int shipSize, in Random generator = null) {
            var gen = generator ?? new Random();
            var placements = new List<Placement>();
            for (var row = 0; row < XDim; row++) {
                for (var col = 0; col < YDim; col++) {
                    var cell = new Pos(col, row);
                    var colPlacement = cell.MakeColumn(shipSize);
                    var rowPlacement = cell.MakeRow(shipSize);

                    if (CheckPlacement(colPlacement)) placements.Add(new Placement { Cell = cell, Orientation = Orientation.Column });
                    if (CheckPlacement(rowPlacement)) placements.Add(new Placement { Cell = cell, Orientation = Orientation.Row });
                }
            }

            if (placements.Count == 0) return null;
            var placement = placements[gen.Next() % placements.Count];
            return PlaceShip(placement.Cell, shipSize, placement.Orientation);
        }


        public Fleet PlaceFleet(in FleetLayout layout, in Random generator = null) {
            var fleet = new Fleet();
            foreach (var division in layout.Content) {
                int remain = division.NumberOfShips;
                while (remain > 0) {
                    var placement = PlaceShipRandom(division.ShipSize, generator);
                    if (placement == null) return null;
                    fleet.Add(new Ship(placement));
                    remain--;
                }
            }
            return fleet;
        }


        private void PrintCell(in int col, in int row, ref byte val) { Console.Write(val == CELL_FREE ? '-' : 'X'); }


        public void Print() { Iterate(PrintCell, (in int r) => { Console.WriteLine(); }); }


        delegate void ForEachCellCallback(in int col, in int row, ref byte value);

        delegate void ForEachRowCallback(in int row);


        private void Iterate(in ForEachCellCallback forEachCell, in ForEachRowCallback forEachRow = null) {
            for (var row = 0; row < XDim; row++) {
                forEachRow?.Invoke(row);
                for (var col = 0; col < YDim; col++) {
                    forEachCell(col, row, ref Cells[row * YDim + col]);
                }
            }
        }


        private struct Placement
        {
            public Pos Cell;
            public Orientation Orientation;
        }
    }
}