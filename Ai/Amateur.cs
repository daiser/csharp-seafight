using System;
using System.Collections.Generic;
using System.Linq;
using SeaFight.Boards;

namespace SeaFight.Ai
{
    public class Amateur: Player
    {
        private readonly Dictionary<int, List<(int col, int row)>> m_todo = new Dictionary<int, List<(int col, int row)>>();


        public Amateur(Random generator): base(generator) { }


        public override Shot Shoot(IEnumerable<Player> liveCompetitors) {
            // Tries to eliminate most damaged competitor first
            var (victim, hitBoard) = liveCompetitors.Where(c => c.Id != Id)
                .Select(c => (c, GetHitBoard(c)))
                .OrderByDescending(t => t.Item2.HitRate)
                .First();

            var todo = GetTodo(victim);
            if (todo.Count == 0)
                // Shoot @ random free cell
                return new Shot { Victim = victim, Target = hitBoard.SelectCells(c => c == CellState.None).PickRandom(Rng) };

            var target = todo.PickRandom(Rng);
            todo.Remove(target);
            return new Shot { Target = target, Victim = victim };
        }


        public override string ToString() { return $"Amateur #{Id:d}"; }


        public override void UpdateHits(Hit hit) {
            // Remembers his hits only.
            if (hit.Attacker.Id != Id) return;

            SaveHit(hit);

            var hitBoard = GetHitBoard(hit.Victim);
            var todo = GetTodo(hit.Victim);


            IEnumerable<(int col, int row)> FindEnds(IEnumerable<(int col, int row)> solidShape) {
                var s = solidShape as (int col, int row)[] ?? solidShape.ToArray();
                var cols = s.Select(c => c.col).Distinct().OrderBy(v => v).ToArray();
                var rows = s.Select(c => c.row).Distinct().OrderBy(v => v).ToArray();

                switch (s.DetectShape()) {
                    case Shape.Point:
                        var point = s[0];
                        yield return point.Shift((0, -1)); // up
                        yield return point.Shift((0, 1)); // down
                        yield return point.Shift((-1, 0)); // left
                        yield return point.Shift((1, 0)); // right
                        break;
                    case Shape.Vertical:
                        yield return (cols[0], rows.First() - 1); // up
                        yield return (cols[0], rows.Last() + 1); // down
                        break;
                    case Shape.Horizontal:
                        yield return (cols.First() - 1, rows[0]); // left
                        yield return (cols.Last() + 1, rows[0]); // right
                        break;
                    case Shape.None:
                    default:
                        throw new InvalidOperationException("failed to determine shape");
                }
            }


            if (hit.Result == CellState.Hit) {
                // Find points to shoot next 
                todo.Clear();
                todo.AddRange(FindEnds(hitBoard.FindSolidShape(hit.Target, c => c == CellState.Hit)).Where(end => hitBoard.Contains(end)));
            }

            if (hit.Result == CellState.Kill) {
                todo.Clear();
                // Mark all cells around ship as hits.
                var ship = hitBoard.FindSolidShape(hit.Target, c => c == CellState.Hit || c == CellState.Kill).ToArray();
                foreach (var shipCell in ship) {
                    hitBoard[shipCell] = CellState.Kill;
                    foreach (var (nCol, nRow, _) in hitBoard.NeighborsOf(shipCell).Where(n=>n.value==CellState.None)) hitBoard[nCol][nRow] = CellState.Miss;
                }
            }
        }


        private List<(int col, int row)> GetTodo(Player rival) {
            if (!m_todo.ContainsKey(rival.Id)) m_todo[rival.Id] = new List<(int col, int row)>();
            return m_todo[rival.Id];
        }
    }
}