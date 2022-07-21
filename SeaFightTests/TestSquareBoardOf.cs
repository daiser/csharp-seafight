using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaFight.Board;

namespace SeaFightTests
{
    [TestClass]
    public class TestSquareBoardOf
    {
        [TestMethod]
        public void TestNew() {
            var board = new SquareBoardOf<int>(3);

            Assert.AreEqual(3, board.XDim);
            Assert.AreEqual(3, board.YDim);
            Assert.AreEqual(9, board.Size);
            Assert.AreEqual(9, board.Cells.Length);
        }
    }
}