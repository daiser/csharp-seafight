using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFight.Players
{
    class Monkey : Player, IHaveSkill
    {
        private readonly Random rnd;

        public Skill Skill { get { return Skill.Monkey; } }

        public Monkey(Random generator = null) : base()
        {
            rnd = generator ?? new Random();
        }

        public override string ToString()
        {
            return string.Format("Monkey #{0:d}", Id);
        }

        public override Fleet PlaceFleet(FleetLayout layout, Board board)
        {
            return board.PlaceFleet(layout, rnd);
        }

        public override Shot Shoot(IEnumerable<HitBoard> boards)
        {
            var bs = boards.ToArray();
            var board = bs[rnd.Next() % bs.Length];
            return new Shot
            {
                rival = board.Rival,
                coords = new Cell
                {
                    Col = rnd.Next() % board.Dim,
                    Row = rnd.Next() % board.Dim,
                }
            };
        }

        public override void UpdateHits(IEnumerable<HitBoard> board, Hit hit)
        {
        }
    }
}
