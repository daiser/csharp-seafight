using SeaFight.Ai;

namespace SeaFight.Board
{
    class HitBoard: SquareBoardOf<byte>
    {
        public ICompetitor Owner { get; }

        public ICompetitor Rival { get; }


        public HitBoard(ICompetitor owner, ICompetitor rival, in int dim): base(dim) {
            Owner = owner;
            Rival = rival;
        }
    }
}