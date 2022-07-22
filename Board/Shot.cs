using SeaFight.Ai;

namespace SeaFight.Board
{
    struct Shot
    {
        public ICompetitor Attacker;
        public ICompetitor Victim;
        public Point Target;
    }
}