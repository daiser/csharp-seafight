using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaFight.Boards;

namespace SeaFightTests
{
    [TestClass]
    public class FleetTest
    {
        [TestMethod]
        public void Hits() {
            var board = new ShipBoard(3);
            Assert.IsTrue(board.PlaceShip((0, 0), 1));
            Assert.IsTrue(board.PlaceShip((2, 0), 2, false));
            Assert.IsTrue(board.PlaceShip((0, 2), 1, true));

            var fleet = board.Commit();
            Assert.IsTrue(fleet != null);
            // 1.2
            // ..2
            // 1..

            Assert.AreEqual(CellState.Kill, fleet.TakeShot(new Shot { Target = (0, 0), Victim = null }));
            Assert.AreEqual(CellState.Kill, fleet.TakeShot(new Shot { Target = (0, 0), Victim = null }));
            Assert.IsTrue(fleet.IsAlive);

            Assert.AreEqual(CellState.Hit, fleet.TakeShot(new Shot { Target = (2, 0), Victim = null }));
            Assert.AreEqual(CellState.Kill, fleet.TakeShot(new Shot { Target = (2, 1), Victim = null }));
            Assert.IsTrue(fleet.IsAlive);

            Assert.AreEqual(CellState.Miss, fleet.TakeShot(new Shot { Target = (1, 0), Victim = null }));

            Assert.AreEqual(CellState.Kill, fleet.TakeShot(new Shot { Target = (0, 2), Victim = null }));
            Assert.IsFalse(fleet.IsAlive);
        }
    }
}