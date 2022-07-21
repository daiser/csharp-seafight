using System.Collections.Generic;
using SeaFight.Armada;
using SeaFight.Board;

namespace SeaFight.Players
{
    interface ICompetitor : IIdentifiableCompetitor
    {
        Fleet PlaceFleet(FleetLayout layout, Board.Board board);
        Shot Shoot(IEnumerable<HitBoard> boards);
        void UpdateHits(IEnumerable<HitBoard> boards, Hit hit);
    }
}
