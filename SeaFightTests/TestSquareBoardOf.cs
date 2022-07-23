using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaFight.Boards;

namespace SeaFightTests
{
    [TestClass]
    public class TestSquareBoardOf
    {
        [TestMethod]
        public void TestNew() {
            var board = new SquareBoardOf<int>(3);

            Assert.AreEqual(3, board.Columns);
            Assert.AreEqual(3, board.Rows);
            Assert.AreEqual(9, board.Area);
            Assert.AreEqual(9, board.Cells.Length);
        }
    }
}