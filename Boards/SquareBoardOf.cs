namespace SeaFight.Boards
{
    public class SquareBoardOf<T>: BoardOf<T>
    {
        public SquareBoardOf(int dim): base(dim, dim) { }
    }
}