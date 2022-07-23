using System;
using System.Collections.Generic;
using SeaFight.Boards;

namespace SeaFight.Ai
{
    internal class Monkey: Player
    {
        public Monkey(Random generator): base(generator) { }


        public override Shot Shoot(IEnumerable<Player> liveCompetitors) {
            // Monkey can shoot anybody. Even itself.
            var victim = liveCompetitors.PickRandom(Rng);
            var hitBoard = GetHitBoard(victim);
            return new Shot { Victim = victim, Target = (Rng.Next() % hitBoard.Columns, Rng.Next() % hitBoard.Rows) };
        }


        public override string ToString() { return $"Monkey #{Id:d}"; }


        public override void UpdateHits(Hit hit) {
            // Monkey does not care about results.
        }
    }
}