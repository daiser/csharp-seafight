using System.Collections.Generic;

namespace SeaFight
{
    class Fleet : List<Ship>, IShootable
    {
        public Fleet(IEnumerable<Ship> ships) : base(ships) { }
        public Fleet() : base() { }

        public ShotEffect TakeShot(Cell at)
        {
            foreach (var ship in this)
            {
                var effect = ship.TakeShot(at);
                if (effect != ShotEffect.Miss) return effect;
            }
            return ShotEffect.Miss;
        }

        public bool IsAlive()
        {
            foreach (var ship in this)
            {
                foreach (var cell in ship)
                {
                    if (cell.Live) return true;
                }
            }
            return false;
        }
    }
}
