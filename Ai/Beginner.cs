using System;
using System.Collections.Generic;
using System.Linq;
using SeaFight.Boards;

namespace SeaFight.Ai
{
    internal class Beginner: Player
    {
        public Beginner(Random generator): base(generator) { }


        public override Shot Shoot(IEnumerable<Player> liveCompetitors) {
            var victim = liveCompetitors.Where(c => c.Id != Id).PickRandom(Rng);
            var hitBoard = GetHitBoard(victim);
            return new Shot { Victim = victim, Target = hitBoard.SelectCells(c => c != CellState.Hit).PickRandom() };
        }


        public override string ToString() { return $"Beginner #{Id:d}"; }


        public override void UpdateHits(Hit hit) {
            // Remembers his hits only.
            if (hit.Attacker.Id == Id && (hit.Result == CellState.Hit || hit.Result == CellState.Kill)) SaveHit(hit);
        }
    }
}