using SeaFight.Ai;

namespace SeaFight.Board
{
    struct Hit
    {
        public IIdentifiableCompetitor Attacker;
        public IIdentifiableCompetitor Victim;
        public Point Target;
        public ShotEffect Effect;


        public override string ToString() { return $"{Attacker} -> {Victim} @ {Target} = {Effect}"; }
    }
}