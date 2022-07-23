using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaFight.Boards;

namespace SeaFightTests
{
    [TestClass]
    public class TestBoardOf
    {
        [TestMethod]
        public void ResetBoard() {
            var board = new BoardOf<int>(2, 4);
            Assert.IsTrue(board.Cells.All(c => c == 0));
            board.ResetBoard(10);
            Assert.IsTrue(board.Cells.All(c => c == 10));
        }


        [TestMethod]
        public void TestContains() {
            var board = new BoardOf<int>(3, 2);

            Assert.IsTrue(board.Contains(0, 0));
            Assert.IsTrue(board.Contains(2, 1));
            Assert.IsFalse(board.Contains(-1, 0));
            Assert.IsFalse(board.Contains(2, 2));
            Assert.IsFalse(board.Contains(3, 1));

            Assert.IsTrue(board.Contains((1, 1)));
        }


        [TestMethod]
        public void TestNeighbors() {
            var board = new BoardOf<int>(3, 3);

            var n = board.NeighborsOf((0, 0)).ToArray();
            Assert.AreEqual(3, n.Length);
            CollectionAssert.AreEqual(new List<(int, int, int)> { (1, 0, 0), (0, 1, 0), (1, 1, 0) }, n);

            n = board.NeighborsOf((1, 0)).ToArray();
            Assert.AreEqual(5, n.Length);
            CollectionAssert.AreEqual(new List<(int, int, int)> {
                    (0, 0, 0),
                    (2, 0, 0),
                    (0, 1, 0),
                    (1, 1, 0),
                    (2, 1, 0)
                },
                n);
        }


        [TestMethod]
        public void TestNewBoard() {
            var board = new BoardOf<int>(3, 5);
            Assert.AreEqual(3, board.Columns);
            Assert.AreEqual(5, board.Rows);
            Assert.AreEqual(15, board.Area);
            Assert.AreEqual(15, board.Cells.Length);
            Assert.IsTrue(board.Cells.All(c => c == 0));
        }


        [TestMethod]
        public void TestToPlain() {
            var board = new BoardOf<int>(3, 3);
            Assert.AreEqual(0, board.ToPlain(0, 0));
            Assert.AreEqual(1, board.ToPlain(1, 0));
            Assert.AreEqual(3, board.ToPlain(0, 1));
        }


        [TestMethod]
        public void Test2D() {
            var board = new BoardOf<int>(3, 3);

            Assert.IsInstanceOfType(board[1], typeof(BoardOf<int>.Row));
            Assert.AreEqual(0, board[0][0]);
            board[0][0] = 10;
            Assert.AreEqual(10, board[0][0]);
            Assert.AreEqual(10, board.Cells[0]);
        }
    }
}