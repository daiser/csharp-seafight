using System;
using System.Collections.Generic;
using System.Linq;
using SeaFight.Boards;
using SeaFight.Ships;

namespace SeaFight.Ai
{
    public abstract class Player
    {
        private static readonly object s_latch = new object();
        private static int s_nextId;

        private readonly Dictionary<int, HitBoard> m_hitBoards = new Dictionary<int, HitBoard>();
        protected readonly Random Rng;
        private int m_boardSize;


        protected Player(Random rng) {
            Rng = rng ?? throw new ArgumentNullException(nameof(rng));
            Id = GetNextId();
        }


        public int Id { get; }


        public override bool Equals(object other) {
            if (!(other is Player o)) return false;
            return o.Id == Id;
        }


        public override int GetHashCode() { return Id; }


        public abstract Shot Shoot(IEnumerable<Player> liveCompetitors);


        public Fleet StartGame(int boardSize, FleetLayout layout) {
            m_boardSize = boardSize;
            var shipBoard = new ShipBoard(boardSize);

            foreach (var shipSize in layout) {
                var freeCells = shipBoard.GetFreePlaces(shipSize).ToArray();
                if (freeCells.Length == 0) throw new InvalidOperationException($"failed to place ship of size {shipSize}: no space");

                var (col, row, horizontal) = freeCells.PickRandom(Rng);
                if (!shipBoard.PlaceShip((col, row), shipSize, horizontal))
                    throw new InvalidOperationException($"failed to place ship of size {shipSize}: free space detection failed");
            }

            return shipBoard.Commit();
        }


        public abstract void UpdateHits(Hit hit);


        protected static IEnumerable<int> GetCellIndexes(HitBoard board, params CellState[] values) {
            return board.Cells.Select((c, i) => new { val = c, idx = i }).Where(e => values.Contains(e.val)).Select(e => e.idx);
        }


        protected static IEnumerable<int> GetUnknownCellsIndexes(HitBoard board) {
            return board.Cells.Select((c, i) => new { cell = c, idx = i }).Where(e => e.cell == CellState.None).Select(e => e.idx);
        }


        protected HitBoard GetHitBoard(Player competitor) {
            if (!m_hitBoards.ContainsKey(competitor.Id)) m_hitBoards[competitor.Id] = new HitBoard(m_boardSize);
            return m_hitBoards[competitor.Id];
        }


        protected void SaveHit(Hit hit) { GetHitBoard(hit.Victim)[hit.Target] = hit.Result; }


        private static int GetNextId() {
            lock (s_latch) {
                return ++s_nextId;
            }
        }
    }
}