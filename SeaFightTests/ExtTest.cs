using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaFight;

namespace SeaFightTests
{
    [TestClass]
    public class ExtTest
    {
        [TestMethod]
        public void TestDetectShape() {
            Assert.AreEqual(Shape.Point, new[] { (1, 1) }.DetectShape());
            Assert.AreEqual(Shape.Horizontal, new[] { (1, 1), (2, 1), (3, 1) }.DetectShape());
            Assert.AreEqual(Shape.Vertical, new[] { (1, 1), (1, 2), (1, 3) }.DetectShape());
            Assert.AreEqual(Shape.None, new[] { (1, 1), (2, 2) }.DetectShape());
            Assert.AreEqual(Shape.None, new (int, int)[] { }.DetectShape());
        }
    }
}