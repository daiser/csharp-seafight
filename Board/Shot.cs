using SeaFight.Players;

namespace SeaFight.Board
{
    struct Shot
    {
        public ICompetitor Attacker;
        public ICompetitor Victim;
        public Point Target;
    }
}