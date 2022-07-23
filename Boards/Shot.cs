using SeaFight.Ai;

namespace SeaFight.Boards
{
    public struct Shot
    {
        public Player Victim;
        public (int col, int row) Target;
    }
}