using System.Collections.Generic;
using System.Linq;
using SeaFight.Boards;

namespace SeaFight.Ships
{
    public class Fleet
    {
        private readonly Ship[] m_ships;


        public Fleet(IEnumerable<Ship> ships) { m_ships = ships.ToArray(); }


        public bool IsAlive => m_ships.Any(s => !s.IsDead);


        public CellState TakeShot(Shot shot) { return m_ships.Select(s => s.TakeShot(shot)).Max(); }
    }
}