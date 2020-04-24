using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFight.Players
{
    class Beginner : Player, IHaveSkill
    {
        public Skill Skill
        {
            get { return Skill.Beginner; }
        }

        private readonly Random rnd;

        public Beginner(Random generator) : base(AiFeatures.DontShootYourself | AiFeatures.RememberOwnShots | AiFeatures.RememberRivalShots)
        {
            rnd = generator ?? new Random();
        }

        public override string ToString()
        {
            return string.Format("Beginner #{0:d}", Id);
        }

        public override Fleet PlaceFleet(FleetLayout layout, Board board)
        {
            return board.PlaceFleet(layout, rnd);
        }

        public override Shot Shoot(IEnumerable<HitBoard> boards)
        {
            var board = PreSelectBoard(boards).PickRandom(rnd);
            var unknownIdxs = GetUnknownCellsIdxs(board);
            var idx = unknownIdxs.PickRandom(rnd);

            return new Shot
            {
                rival = board.Rival,
                coords = board.Unplain(idx),
            };
        }

        public override void UpdateHits(IEnumerable<HitBoard> boards, Hit hit)
        {
            SaveHit(boards, hit);
        }
    }
}
