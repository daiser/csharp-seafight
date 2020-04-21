namespace SeaFight
{
    class HitBoard : BoardOf<byte>
    {
        public ICompetitor Owner { get; private set; }
        public ICompetitor Rival { get; private set; }
        public HitBoard(ICompetitor owner, ICompetitor rival, in int dim) : base(dim)
        {
            Owner = owner;
            Rival = rival;
        }
    }
}
