using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaFight.Boards;
using SeaFight.Ships;

namespace SeaFightTests
{
    [TestClass]
    public class ShipTest
    {
        [TestMethod]
        public void TakeShot() {
            var ship = new Ship(new[] { (0, 0, CellState.None), (0, 1, CellState.None) });

            Assert.AreEqual(CellState.Miss, ship.TakeShot(new Shot { Target = (1, 1), Victim = null }));
            Assert.AreEqual(CellState.Hit, ship.TakeShot(new Shot { Target = (0, 0), Victim = null }));
            Assert.AreEqual(CellState.Kill, ship.TakeShot(new Shot { Target = (0, 1), Victim = null }));
        }


        [TestMethod]
        public void IsDead() {
            var ship = new Ship(new[] { (0, 0, CellState.None), (0, 1, CellState.None) });
            Assert.IsFalse(ship.IsDead);

            ship = new Ship(new[] { (0, 0, CellState.Hit), (0, 1, CellState.None) });
            Assert.IsFalse(ship.IsDead);

            ship = new Ship(new[] { (0, 0, CellState.Hit), (0, 1, CellState.Hit) });
            Assert.IsTrue(ship.IsDead);

            ship = new Ship(new[] { (0, 0, CellState.None), (0, 1, CellState.None) });
            ship.TakeShot(new Shot { Target = (0, 0) });
            ship.TakeShot(new Shot { Target = (0, 1) });
            Assert.IsTrue(ship.IsDead);
        }
    }
}