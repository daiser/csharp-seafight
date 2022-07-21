using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaFight.Board;

namespace SeaFightTests
{
    [TestClass]
    public class TestBoardOf
    {
        [TestMethod]
        public void TestNewBoard() {
            var board = new BoardOf<int>(3, 5);
            Assert.AreEqual(3, board.XDim);
            Assert.AreEqual(5, board.YDim);
            Assert.AreEqual(15, board.Size);
            Assert.AreEqual(15, board.Cells.Length);
            Assert.IsTrue(board.Cells.All(c => c == 0));
        }
    }
}