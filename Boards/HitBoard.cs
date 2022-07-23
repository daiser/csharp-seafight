using System.Linq;

namespace SeaFight.Boards
{
    public class HitBoard: SquareBoardOf<CellState>
    {
        public HitBoard(int dim): base(dim) { }


        public double HitRate => (double)Cells.Count(c => c == CellState.Hit | c == CellState.Kill) / Area;
    }
}