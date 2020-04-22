namespace SeaFight
{
    struct Hit
    {
        public IIdentifiableCompetitor attacker;
        public IIdentifiableCompetitor target;
        public Pos coords;
        public ShotEffect effect;

        public override string ToString()
        {
            return string.Format("{0} -> {1} @ {2} = {3}", attacker, target, coords, effect);
        }
    }
}
