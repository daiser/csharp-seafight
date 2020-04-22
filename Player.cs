using SeaFight.Players;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SeaFight
{
    abstract class Player : ICompetitor
    {
        protected const byte CELL_UNKNOWN = 0;
        protected const byte CELL_MISS = 1;
        protected const byte CELL_HIT = 2;
        protected const byte CELL_KILL = 3;

        protected static readonly byte[] HitValues = { CELL_MISS, CELL_HIT, CELL_KILL };

        private static readonly object latch = new object();
        private static int next_id = 0;
        private static int GetNextId()
        {
            lock (latch)
            {
                return ++next_id;
            }
        }

        private readonly AiFeatures features;
        public int Id { get; private set; }
        public Player(AiFeatures features)
        {
            Id = GetNextId();
            this.features = features;
        }

        protected IEnumerable<HitBoard> PreSelectBoard(IEnumerable<HitBoard> boards)
        {
            if (features.HasFlag(AiFeatures.DontShootYourself))
            {
                return boards.Where(b => !b.Rival.Equals(this));
            }
            return boards;
        }

        protected HitBoard SaveHit(IEnumerable<HitBoard> boards, Hit hit)
        {
            if (hit.target.Equals(this)) return null;

            HitBoard selectedBoard = null;

            if (
                (features.HasFlag(AiFeatures.RememberOwnShots) && hit.attacker.Equals(this)) ||
                (features.HasFlag(AiFeatures.RememberRivalShots) && !hit.attacker.Equals(this))
                )
            {
                selectedBoard = boards.Where(b => b.Owner.Equals(this) && b.Rival.Equals(hit.target)).First();
                selectedBoard.Set(hit.coords, HitValues[(int)hit.effect]);
            }

            return selectedBoard;
        }

        protected static IEnumerable<int> GetUnknownCellsIdxs(HitBoard board)
        {
            return board.Cells.Select((c, i) => new { cell = c, idx = i }).Where(e => e.cell == CELL_UNKNOWN).Select(e => e.idx);
        }

        protected static IEnumerable<int> GetCellIndexes(HitBoard board, params byte[] values)
        {
            return board.Cells.Select((c, i) => new { val = c, idx = i }).Where(e => values.Contains(e.val)).Select(e => e.idx);
        }

        public override string ToString()
        {
            return string.Format("Player #{0:d}", Id);
        }

        public abstract Fleet PlaceFleet(FleetLayout layout, Board board);

        public abstract Shot Shoot(IEnumerable<HitBoard> boards);

        public abstract void UpdateHits(IEnumerable<HitBoard> boards, Hit hit);

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object other)
        {
            if (!(other is IIdentifiableCompetitor o)) return false;
            return o.Id == Id;
        }
    }
}
