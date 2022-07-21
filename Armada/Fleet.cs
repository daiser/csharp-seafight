using System.Collections.Generic;
using System.Linq;
using SeaFight.Board;

namespace SeaFight.Armada
{
    class Fleet: List<Ship>, ITarget
    {
        public static readonly Fleet Fixed0 = new Fleet {
            new Ship(new[] { new Pos(2, 2), new Pos(2, 3), new Pos(2, 4), new Pos(2, 5) }), // 4
            new Ship(new[] { new Pos(7, 1), new Pos(8, 1), new Pos(9, 1), }), // 3
            new Ship(new[] { new Pos(7, 4), new Pos(8, 4), new Pos(9, 4), }), // 3
            new Ship(new[] { new Pos(4, 0), new Pos(4, 1), }), // 2
            new Ship(new[] { new Pos(5, 6), new Pos(5, 7), }), // 2
            new Ship(new[] { new Pos(0, 9), new Pos(1, 9), }), // 2
            new Ship(new[] { new Pos(4, 3) }), // 1
            new Ship(new[] { new Pos(2, 7) }), // 1
            new Ship(new[] { new Pos(7, 6) }), // 1
            new Ship(new[] { new Pos(9, 6) }), // 1
        };

        public int Size { get; private set; }


        public Fleet(IEnumerable<Ship> ships): base(ships) { Size = this.Select(s => s.Size).Sum(); }


        public Fleet(): base() { }


        public ShotEffect TakeShot(Pos at) {
            return this.Select(ship => ship.TakeShot(at)).FirstOrDefault(effect => effect != ShotEffect.Miss);
        }


        public bool IsAlive() { return this.Any(ship => ship.Any(cell => cell.Value)); }
    }
}