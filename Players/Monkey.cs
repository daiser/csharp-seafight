using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFight.Players
{
    class Monkey: Player, IHaveSkill
    {
        private readonly Random m_rnd;

        public Skill Skill => Skill.Monkey;


        public Monkey(Random generator = null): base(AiFeatures.None) { m_rnd = generator ?? new Random(); }


        public override string ToString() { return $"Monkey #{Id:d}"; }


        public override Fleet PlaceFleet(FleetLayout layout, Board board) { return board.PlaceFleet(layout, m_rnd); }


        public override Shot Shoot(IEnumerable<HitBoard> boards) {
            var bs = boards.ToArray();
            var board = bs[m_rnd.Next() % bs.Length];
            return new Shot { Rival = board.Rival, Coords = new Pos(m_rnd.Next() % board.Dim, m_rnd.Next() % board.Dim) };
        }


        public override void UpdateHits(IEnumerable<HitBoard> board, Hit hit) { }
    }
}