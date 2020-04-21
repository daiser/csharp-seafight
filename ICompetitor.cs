using System.Collections.Generic;


namespace SeaFight
{
    interface ICompetitor : IIdentifiableCompetitor
    {
        Fleet PlaceFleet(FleetLayout layout, Board board);
        Shot Shoot(IEnumerable<HitBoard> boards);
        void UpdateHits(IEnumerable<HitBoard> boards, Hit hit);
    }
}
