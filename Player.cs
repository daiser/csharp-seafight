using SeaFight.Players;
using System.Collections.Generic;
using System.Linq;
using SeaFight.Armada;
using SeaFight.Board;


namespace SeaFight
{
    abstract class Player: ICompetitor
    {
        protected const byte CELL_UNKNOWN = 0;
        protected const byte CELL_MISS = 1;
        protected const byte CELL_HIT = 2;
        protected const byte CELL_KILL = 3;

        protected static readonly byte[] HitValues = { CELL_MISS, CELL_HIT, CELL_KILL };

        private static readonly object s_latch = new object();
        private static int s_nextId;


        private static int GetNextId() {
            lock (s_latch) {
                return ++s_nextId;
            }
        }


        private readonly AiFeatures m_features;

        public int Id { get; }


        public Player(AiFeatures features) {
            Id = GetNextId();
            m_features = features;
        }


        protected IEnumerable<HitBoard> PreSelectBoard(IEnumerable<HitBoard> boards) {
            if (m_features.HasFlag(AiFeatures.DontShootYourself)) {
                return boards.Where(b => !b.Rival.Equals(this));
            }
            return boards;
        }


        protected HitBoard SaveHit(IEnumerable<HitBoard> boards, Hit hit) {
            if (hit.Target.Equals(this)) return null;

            HitBoard selectedBoard = null;

            if ((m_features.HasFlag(AiFeatures.RememberOwnShots) && hit.Attacker.Equals(this)) ||
                (m_features.HasFlag(AiFeatures.RememberRivalShots) && !hit.Attacker.Equals(this))) {
                selectedBoard = boards.First(b => b.Owner.Equals(this) && b.Rival.Equals(hit.Target));
                selectedBoard.Set(hit.Coords, HitValues[(int)hit.Effect]);
            }

            return selectedBoard;
        }


        protected static IEnumerable<int> GetUnknownCellsIndexes(HitBoard board) {
            return board.Cells.Select((c, i) => new { cell = c, idx = i }).Where(e => e.cell == CELL_UNKNOWN).Select(e => e.idx);
        }


        protected static IEnumerable<int> GetCellIndexes(HitBoard board, params byte[] values) {
            return board.Cells.Select((c, i) => new { val = c, idx = i }).Where(e => values.Contains(e.val)).Select(e => e.idx);
        }


        public override string ToString() { return $"Player #{Id:d}"; }


        public abstract Armada.Fleet PlaceFleet(FleetLayout layout, Board.Board board);


        public abstract Shot Shoot(IEnumerable<HitBoard> boards);


        public abstract void UpdateHits(IEnumerable<HitBoard> boards, Hit hit);


        public override int GetHashCode() { return Id; }


        public override bool Equals(object other) {
            if (!(other is IIdentifiableCompetitor o)) return false;
            return o.Id == Id;
        }
    }
}