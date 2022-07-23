using System;
using System.Collections.Generic;
using System.Linq;
using SeaFight.Boards;

namespace SeaFight.Ai
{
    internal class Noob: Monkey
    {
        public Noob(Random generator = null): base(generator) { }


        public override Shot Shoot(IEnumerable<Player> liveCompetitors) {
            // Noob is smart enough to not shoot himself. That's all hi got.
            return base.Shoot(liveCompetitors.Where(c => c.Id != Id));
        }


        public override string ToString() { return $"Noob #{Id:d}"; }


        public override void UpdateHits(Hit hit) {
            // Noob is lazy. That's why he is noob. He does not track his hits.
        }
    }
}