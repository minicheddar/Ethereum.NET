using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ethereum.Encoding
{
    [TestClass]
    public class ArraySegmentExtensionsTests
    {
        [TestMethod]
        public void SliceReturnsLast2ItemsWhenStart2AndEnd2()
        {
            var input = new ArraySegment<string>(new[] { "a", "b", "c", "d" });

            var result = input.Slice(2, 2).ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("c", result[0]);
            Assert.AreEqual("d", result[1]);
        }

        [TestMethod]
        public void SliceReturnsLast3ItemsWhenStart1AndEndNull()
        {
            var input = new ArraySegment<string>(new[] { "a", "b", "c", "d" });

            var result = input.Slice(1).ToArray();

            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("b", result[0]);
            Assert.AreEqual("c", result[1]);
            Assert.AreEqual("d", result[2]);
        }

        [TestMethod]
        public void SliceReturnsLast0ItemsWhenStart1AndEndNull()
        {
            var input = new ArraySegment<string>(new[] { "a" });

            var result = input.Slice(1).ToArray();

            Assert.AreEqual(0, result.Length);
        }
    }
}
