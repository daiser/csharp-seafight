﻿namespace SeaFight
{
    class HitBoard : BoardOf<byte>
    {
        public ICompetitor Owner { get; }
        public ICompetitor Rival { get; }
        public HitBoard(ICompetitor owner, ICompetitor rival, in int dim) : base(dim)
        {
            Owner = owner;
            Rival = rival;
        }
    }
}
