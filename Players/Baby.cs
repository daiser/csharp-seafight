using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFight.Players
{
    class Baby : Player, IHaveSkill
    {
        private readonly Random rnd;

        public Skill Skill
        {
            get { return Skill.Baby; }
        }

        public Baby(Random generator = null) : base(AiFeatures.DontShootYourself | AiFeatures.RememberOwnShots)
        {
            rnd = generator ?? new Random();
        }

        public override string ToString()
        {
            return string.Format("Baby #{0:d}", Id);
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
