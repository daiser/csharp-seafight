namespace SeaFight
{
    struct Hit
    {
        public IIdentifiableCompetitor Attacker;
        public IIdentifiableCompetitor Target;
        public Pos Coords;
        public ShotEffect Effect;

        public override string ToString()
        {
            return $"{Attacker} -> {Target} @ {Coords} = {Effect}";
        }
    }
}
