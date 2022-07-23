using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaFight.Boards;

namespace SeaFightTests
{
    [TestClass]
    public class TestShipBoard
    {
        [TestMethod]
        public void AvailableCells() {
            var board = new ShipBoard(3); // 3x3

            var freeCells = board.GetAvailableCells(1).ToArray();
            Assert.AreEqual(18, freeCells.Length); // 9 verticals + 9 horizontals
            CollectionAssert.Contains(freeCells, (0, 0, true));
            CollectionAssert.Contains(freeCells, (1, 2, false));

            freeCells = board.GetAvailableCells(2).ToArray();
            Assert.AreEqual(12, freeCells.Length);
            CollectionAssert.Contains(freeCells, (2, 0, false));
            CollectionAssert.DoesNotContain(freeCells, (2, 0, true));

            board[0][0] = 1; // (0,0) = OCCUPIED
            // OBF
            // BBF
            // FFF

            freeCells = board.GetAvailableCells(2).ToArray();
            Assert.AreEqual(10, freeCells.Length);
            CollectionAssert.DoesNotContain(freeCells, (0, 0, true));
            CollectionAssert.DoesNotContain(freeCells, (0, 0, false));
        }


        [TestMethod]
        public void PlaceShip() {
            var board = new ShipBoard(3);
            Assert.IsTrue(board.PlaceShip((0, 0), 1));
            // SBF
            // BBF
            // FFF

            var freeCells = board.GetAvailableCells(2).ToArray();
            Assert.AreEqual(4, freeCells.Length);
            CollectionAssert.Contains(freeCells, (2, 0, false));
            CollectionAssert.Contains(freeCells, (2, 1, false));
            CollectionAssert.Contains(freeCells, (0, 2, true));
            CollectionAssert.Contains(freeCells, (1, 2, true));

            // another ship in the way
            Assert.IsFalse(board.PlaceShip((0, 0), 2, true));
            Assert.IsFalse(board.PlaceShip((0, 0), 2, false));
            // cell nearby another ship should be banned
            Assert.IsFalse(board.PlaceShip((1, 0), 2, true));
            Assert.IsFalse(board.PlaceShip((1, 0), 2, false));
            Assert.IsFalse(board.PlaceShip((0, 1), 2, true));
            Assert.IsFalse(board.PlaceShip((0, 1), 2, false));
            Assert.IsFalse(board.PlaceShip((1, 1), 2, true));
            Assert.IsFalse(board.PlaceShip((1, 1), 2, false));

            Assert.IsTrue(board.PlaceShip((2, 0), 2, false));
            Assert.IsTrue(board.PlaceShip((0, 2), 1, true));

            // SBS
            // BBS
            // SBB
        }


        [TestMethod]
        public void Commit() {
        }
    }
}