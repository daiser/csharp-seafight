using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SeaFight.Armada;
using SeaFight.Board;

namespace SeaFight.Players
{
    class Amateur: Player, IHaveSkill
    {
        private static readonly Point s_stepUp = new Point(0, -1);
        private static readonly Point s_stepDown = new Point(0, 1);
        private static readonly Point s_stepLeft = new Point(-1, 0);
        private static readonly Point s_stepRight = new Point(1, 0);

        public Skill Skill => Skill.Amature;

        private readonly Random rnd;


        public Amateur(Random generator): base(AiFeatures.DontShootYourself | AiFeatures.RememberOwnShots | AiFeatures.RememberRivalShots) {
            rnd = generator;
        }


        public override Armada.Fleet PlaceFleet(FleetLayout layout, Board.Board board) { return board.PlaceFleet(layout, rnd); }


        public override string ToString() { return $"Amateur #{Id:d}"; }


        public override Shot Shoot(IEnumerable<HitBoard> boards) {
            var preSelectedBoards = PreSelectBoard(boards);

            try {
                var board = preSelectedBoards.Where(b => b.Cells.Contains(CELL_HIT)).PickRandom(rnd);
                var hits = GetCellIndexes(board, CELL_HIT);
                var target = board.ToPosition(hits.PickRandom(rnd));
                var fig = new Figure(board.FindSolid(target, CELL_HIT));

                var shots = new List<Point>();
                if (fig.IsRowOriented()) {
                    var left = new Point(fig.Left() - 1, target.Row);
                    var right = new Point(fig.Right() + 1, target.Row);
                    shots.AddIf(left, board.Contains(left) && board.At(left) == CELL_UNKNOWN);
                    shots.AddIf(right, board.Contains(right) && board.At(right) == CELL_UNKNOWN);
                }
                if (fig.IsColumnOriented()) {
                    var top = new Point(target.Col, fig.Top() - 1);
                    var bottom = new Point(target.Col, fig.Bottom() + 1);
                    shots.AddIf(top, board.Contains(top) && board.At(top) == CELL_UNKNOWN);
                    shots.AddIf(bottom, board.Contains(bottom) && board.At(bottom) == CELL_UNKNOWN);
                }

                if (shots.Count == 0) {
                    throw new InvalidOperationException("No target");
                }
                var shot = shots.PickRandom(rnd);
                return new Shot { Rival = board.Rival, Coords = shot, };
            }
            catch (InvalidOperationException x) {
                Debug.WriteLine("{0}: {1}", this, x.Message);
                var board = preSelectedBoards.PickRandom(rnd);
                var unknownIdxs = GetUnknownCellsIndexes(board);
                var idx = unknownIdxs.PickRandom(rnd);

                return new Shot { Rival = board.Rival, Coords = board.ToPosition(idx), };
            }
        }


        private static void SetMisses(HitBoard board, Figure figure, params Point[] offsets) {
            foreach (var pos in figure.Blocks) {
                foreach (var offset in offsets) {
                    var offPos = pos.Add(offset);
                    if (board.Contains(offPos)) board.Set(offPos, CELL_MISS);
                }
            }
        }


        public override void UpdateHits(IEnumerable<HitBoard> boards, Hit hit) {
            var board = SaveHit(boards, hit);
            if (board == null) return;

            if (hit.Effect == ShotEffect.Kill) {
                var fig = new Figure(board.FindSolid(hit.Coords, CELL_KILL, CELL_HIT));
                foreach (var pos in fig.Blocks) {
                    board.Set(pos, CELL_KILL);
                }
                var neighbours = fig.Neighbours();
                foreach (var neighbour in neighbours) {
                    if (board.Contains(neighbour)) board.Set(neighbour, CELL_MISS);
                }
            }

            if (hit.Effect == ShotEffect.Hit) {
                var fig = new Figure(board.FindSolid(hit.Coords, CELL_HIT));

                var rowPlaced = fig.IsRowOriented();
                var colPlaced = fig.IsColumnOriented();
                if (rowPlaced && colPlaced) return;

                if (colPlaced) SetMisses(board, fig, s_stepLeft, s_stepRight);
                if (rowPlaced) SetMisses(board, fig, s_stepUp, s_stepDown);
            }
        }
    }
}