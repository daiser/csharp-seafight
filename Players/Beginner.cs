using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaFight.Armada;
using SeaFight.Board;

namespace SeaFight.Players
{
    class Beginner: Player, IHaveSkill
    {
        public Skill Skill
        {
            get { return Skill.Beginner; }
        }

        private readonly Random rnd;


        public Beginner(Random generator):
            base(AiFeatures.DontShootYourself | AiFeatures.RememberOwnShots | AiFeatures.RememberRivalShots) {
            rnd = generator ?? new Random();
        }


        public override string ToString() { return string.Format("Beginner #{0:d}", Id); }


        public override Armada.Fleet PlaceFleet(FleetLayout layout, Board.Board board) { return board.PlaceFleet(layout, rnd); }


        public override Shot Shoot(IEnumerable<HitBoard> boards) {
            var board = PreSelectBoard(boards).PickRandom(rnd);
            var unknownIdxs = GetUnknownCellsIndexes(board);
            var idx = unknownIdxs.PickRandom(rnd);

            return new Shot { Victim = board.Rival, Target = board.ToPosition(idx), };
        }


        public override void UpdateHits(IEnumerable<HitBoard> boards, Hit hit) { SaveHit(boards, hit); }
    }
}