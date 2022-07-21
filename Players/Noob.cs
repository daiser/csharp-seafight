using System;
using System.Collections.Generic;
using SeaFight.Armada;
using SeaFight.Board;

namespace SeaFight.Players
{
    class Noob: Player, IHaveSkill
    {
        private readonly Random m_rnd;

        public Skill Skill => Skill.Noob;


        public Noob(Random generator = null): base(AiFeatures.DontShootYourself | AiFeatures.RememberOwnShots) {
            m_rnd = generator ?? new Random();
        }


        public override string ToString() { return $"Noob #{Id:d}"; }


        public override Armada.Fleet PlaceFleet(FleetLayout layout, Board.Board board) { return board.PlaceFleet(layout, m_rnd); }


        public override Shot Shoot(IEnumerable<HitBoard> boards) {
            var board = PreSelectBoard(boards).PickRandom(m_rnd);
            var unknownCellsIndexes = GetUnknownCellsIndexes(board);
            var idx = unknownCellsIndexes.PickRandom(m_rnd);

            return new Shot { Rival = board.Rival, Coords = board.ToPosition(idx), };
        }


        public override void UpdateHits(IEnumerable<HitBoard> boards, Hit hit) { SaveHit(boards, hit); }
    }
}