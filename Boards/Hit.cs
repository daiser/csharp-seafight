using SeaFight.Ai;

namespace SeaFight.Boards
{
    public struct Hit
    {
        public Player Attacker;
        public Player Victim;
        public (int col, int row) Target;
        public CellState Result;


        public override string ToString() { return $"{Attacker} -> {Victim} @ {Target} = {Result}"; }
    }
}