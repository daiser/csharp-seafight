namespace SeaFight.Board
{
    class Target: Cell<bool>
    {
        public Target(int col, int row): base(col, row, true) { }
    }
}