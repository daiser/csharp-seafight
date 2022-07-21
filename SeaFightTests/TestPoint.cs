using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaFight.Board;

namespace SeaFightTests
{
    [TestClass]
    public class TestPoint
    {
        [TestMethod]
        public void New() {
            var pos = new Point();
            Assert.AreEqual(0, pos.Col);
            Assert.AreEqual(0, pos.Row);
        }


        [TestMethod]
        public void NewWithParams() {
            var pos = new Point(1, 2);
            Assert.AreEqual(1, pos.Col);
            Assert.AreEqual(2, pos.Row);
        }


        [TestMethod]
        public void TestEquals() {
            var p1 = new Point(1, 2);
            var p2 = new Point(1, 2);
            var p3 = new Point(2, 1);

            Assert.IsTrue(p1.Equals(p2));
            Assert.IsFalse(p1.Equals(p3));

            Assert.IsTrue(p1.GetHashCode() == p2.GetHashCode());
        }


        [TestMethod]
        public void TestToString() {
            var p = new Point(1, 2);
            Assert.AreEqual("(1,2)", p.ToString());
        }


        [TestMethod]
        public void Add() {
            var p1 = new Point(1, 2);
            var p2 = new Point(2, 5);
            var p = p1.Add(p2);

            Assert.AreEqual(3, p.Col);
            Assert.AreEqual(7, p.Row);
        }
    }
}