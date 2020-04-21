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
        private static readonly object latch = new object();
        private static int next_id = 0;
        private static int GetNextId()
        {
            lock (latch)
            {
                return ++next_id;
            }
        }

        public int Id { get; private set; }
        public Player()
        {
            Id = GetNextId();
        }

        public override string ToString()
        {
            return string.Format("Player #{0:d}", Id);
        }

        public abstract Fleet PlaceFleet(FleetLayout layout, Board board);

        public abstract Shot Shoot(IEnumerable<HitBoard> boards);

        public abstract void UpdateHits(IEnumerable<HitBoard> board, Hit hit);

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
