using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFight.Players
{
    class Adult : Player, IHaveSkill
    {
        private static readonly Pos stepUp = new Pos(0, -1);
        private static readonly Pos stepDown = new Pos(0, 1);
        private static readonly Pos stepLeft = new Pos(-1, 0);
        private static readonly Pos stepRight = new Pos(1, 0);

        public Skill Skill { get { return Skill.Adult; } }
        private readonly Random rnd;
        public Adult(Random generator) : base(AiFeatures.DontShootYourself | AiFeatures.RememberOwnShots | AiFeatures.RememberRivalShots)
        {
            rnd = generator;
        }

        public override Fleet PlaceFleet(FleetLayout layout, Board board)
        {
            return board.PlaceFleet(layout, rnd);
        }

        public override Shot Shoot(IEnumerable<HitBoard> boards)
        {
            var board = PreSelectBoard(boards).PickRandom(rnd);

            var hits = GetCellIndexes(board, CELL_HIT);
            if (hits.Count() > 0)
            {
                var target = board.Unplain(hits.PickRandom(rnd));
                var fig = new Figure(board.FindSolid(target, CELL_HIT));
                var shots = new List<Pos>();
                if (fig.IsRowOriented())
                {
                    var left = new Pos(fig.Left() - 1, target.Row);
                    var right = new Pos(fig.Right() + 1, target.Row);
                    shots.AddIf(left, board.Contains(left) && board.At(left) == CELL_UNKNOWN);
                    shots.AddIf(right, board.Contains(right) && board.At(right) == CELL_UNKNOWN);
                }
                if (fig.IsColumnOriented())
                {
                    var top = new Pos(target.Col, fig.Top() - 1);
                    var bottom = new Pos(target.Col, fig.Bottom() + 1);
                    shots.AddIf(top, board.Contains(top) && board.At(top) == CELL_UNKNOWN);
                    shots.AddIf(bottom, board.Contains(bottom) && board.At(bottom) == CELL_UNKNOWN);
                }

                if (shots.Count == 0) throw new InvalidOperationException("No target");
                var shot = shots.PickRandom(rnd);
                return new Shot
                {
                    rival = board.Rival,
                    coords = shot,
                };
            }

            var unknownIdxs = GetUnknownCellsIdxs(board);
            var idx = unknownIdxs.PickRandom(rnd);

            return new Shot
            {
                rival = board.Rival,
                coords = board.Unplain(idx),
            };

        }

        private static void SetMisses(HitBoard board, Figure figure, params Pos[] offsets)
        {
            foreach (var pos in figure.Blocks)
            {
                foreach (var offset in offsets)
                {
                    var offPos = pos.Add(offset);
                    if (board.Contains(offPos)) board.Set(offPos, CELL_MISS);
                }
            }
        }

        public override void UpdateHits(IEnumerable<HitBoard> boards, Hit hit)
        {
            var board = SaveHit(boards, hit);
            if (board == null) return;

            if (hit.effect == ShotEffect.Kill)
            {
                var fig = new Figure(board.FindSolid(hit.coords, CELL_KILL, CELL_HIT));
                foreach (var pos in fig.Blocks)
                {
                    board.Set(pos, CELL_KILL);
                }
                var neighbours = fig.Neighbours();
                foreach (var neighbour in neighbours)
                {
                    if (board.Contains(neighbour)) board.Set(neighbour, CELL_MISS);
                }
            }

            if (hit.effect == ShotEffect.Hit)
            {
                var fig = new Figure(board.FindSolid(hit.coords, CELL_HIT));

                var rowPlaced = fig.IsRowOriented();
                var colPlaced = fig.IsColumnOriented();
                if (rowPlaced && colPlaced) return;

                if (colPlaced) SetMisses(board, fig, stepLeft, stepRight);
                if (rowPlaced) SetMisses(board, fig, stepUp, stepDown);
            }
        }
    }
}
