namespace SeaFight.Board
{
    struct Hit
    {
        public IIdentifiableCompetitor Attacker;
        public IIdentifiableCompetitor Target;
        public Point Coords;
        public ShotEffect Effect;

        public override string ToString()
        {
            return $"{Attacker} -> {Target} @ {Coords} = {Effect}";
        }
    }
}
