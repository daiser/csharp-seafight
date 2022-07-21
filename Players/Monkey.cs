﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaFight.Armada;
using SeaFight.Board;

namespace SeaFight.Players
{
    class Monkey: Player, IHaveSkill
    {
        private readonly Random m_rnd;

        public Skill Skill => Skill.Monkey;


        public Monkey(Random generator = null): base(AiFeatures.None) { m_rnd = generator ?? new Random(); }


        public override string ToString() { return $"Monkey #{Id:d}"; }


        public override Armada.Fleet PlaceFleet(FleetLayout layout, Board.Board board) { return board.PlaceFleet(layout, m_rnd); }


        public override Shot Shoot(IEnumerable<HitBoard> boards) {
            var bs = boards.ToArray();
            var board = bs[m_rnd.Next() % bs.Length];
            return new Shot { Rival = board.Rival, Coords = new Point(m_rnd.Next() % board.XDim, m_rnd.Next() % board.YDim) };
        }


        public override void UpdateHits(IEnumerable<HitBoard> board, Hit hit) { }
    }
}