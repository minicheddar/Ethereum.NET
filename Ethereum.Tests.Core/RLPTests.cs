using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ethereum.Encoding
{
    [TestClass]
    public class RLPTests
    {
        /// <summary>
        /// Given data to be RLP encoded
        /// When input is single byte less than or equal to 127
        /// Then that byte is its own RLP encoding
        /// </summary>
        [TestMethod]
        public void Encode_WhenInputIsSingleByteAndLessThanOrEqualTo127()
        {
            var result = RLP.Encode(new byte[] {127});

            Assert.IsTrue(result.Length == 1);
            Assert.IsTrue(result[0] == 127);
        }

        /// <summary>
        /// Given data to be RLP encoded
        /// When input length is 0-55 bytes
        /// Then the first byte is 128 + length of the item
        /// Then followed by the item
        /// </summary>
        [TestMethod]
        public void Encode_WhenInputLengthIs0To55Bytes()
        {
            var input = "dog".ToBytes();

            var result = RLP.Encode(input);

            Assert.IsTrue(result.Length == 4);
            Assert.IsTrue(result[0] == 131);
            Assert.IsTrue(result[1] == 100);
            Assert.IsTrue(result[2] == 111);
            Assert.IsTrue(result[3] == 103);
        }

        /// <summary>
        /// Given data to be RLP encoded
        /// When input length 56-255 bytes
        /// Then the first byte is 183 + length of the length of the item
        /// Then followed by the length of the item
        /// Then followed by the item
        /// </summary>
        [TestMethod]
        public void Encode_WhenInputLengthIs56To255Bytes()
        {
            var input = "This is a sentence. A sentence that is longer than 55 bytes.".ToBytes();

            var result = RLP.Encode(input);

            Assert.IsTrue(result.Length == 62);
            Assert.IsTrue(result[0] == 184);
            Assert.IsTrue(result[1] == 60);
            Assert.IsTrue(result[2] == 84);
            Assert.IsTrue(result[61] == 46);
        }

        /// <summary>
        /// Given data to be RLP encoded
        /// When total length of all collection items is 0-55 bytes
        /// Then the first byte is 192 + length of the items
        /// Then followed by the items
        /// </summary>
        [TestMethod]
        public void Encode_WhenInputIsCollectionAndTotalLengthOfItemsIs0To55Bytes()
        {
            var input = new List<byte[]> {"cat".ToBytes(), "dog".ToBytes()};

            var result = RLP.Encode(input);

            Assert.IsTrue(result.Length == 9);
            Assert.IsTrue(result[0] == 200);
            Assert.IsTrue(result[1] == 131);
            Assert.IsTrue(result[5] == 131);
        }

        /// <summary>
        /// Given data to be RLP encoded
        /// When total length of all collection items is 56-255 bytes
        /// Then the first byte is 247 + length of the length of the items
        /// Then followed by the length of the items
        /// Then followed by the items
        /// </summary>
        [TestMethod]
        public void Encode_WhenInputIsCollectionAndTotalLengthOfItemsIs56To255Bytes()
        {
            var input = new List<byte[]>
                {
                    "this is a collection".ToBytes(),
                    "that when encoded".ToBytes(),
                    "will be greater than 55 bytes".ToBytes(),
                };

            var result = RLP.Encode(input);

            Assert.IsTrue(result.Length == 71);
            Assert.IsTrue(result[0] == 248);
            Assert.IsTrue(result[1] == 69);
            Assert.IsTrue(result[2] == 148);
            Assert.IsTrue(result[70] == 115);
        }

        [TestMethod]
        public void Decode_WhenEncodedIsSingleByteAndLessThanOrEqualTo127()
        {
            var item = RLP.Encode(127.ToBytes());

            var result = RLP.Decode(item);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0].IsCollection == false);
            Assert.IsTrue(result[0].String[0] == 127);
        }

        [TestMethod]
        public void Decode_WhenEncodedLengthIs0To55Bytes()
        {
            var input = RLP.Encode("cheese on toast".ToBytes());

            var result = RLP.Decode(input);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0].IsCollection == false);
            Assert.AreEqual("cheese on toast", System.Text.Encoding.ASCII.GetString(result[0].String));
        }

        [TestMethod]
        public void Decode_WhenEncodedLengthIs56To255Bytes()
        {
            var input = RLP.Encode("This is a sentence. A sentence that is longer than 55 bytes.".ToBytes());

            var result = RLP.Decode(input);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0].IsCollection == false);
            Assert.AreEqual("This is a sentence. A sentence that is longer than 55 bytes.", System.Text.Encoding.ASCII.GetString(result[0].String));
        }

        [TestMethod]
        public void Decode_WhenEncodedCollectionIs0To55Bytes()
        {
            var input = RLP.Encode(new List<byte[]> {"cat".ToBytes(), "dog".ToBytes()});

            var result = RLP.Decode(input);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.All(x => x.IsCollection == false));
            Assert.AreEqual("cat", System.Text.Encoding.ASCII.GetString(result[0].String));
            Assert.AreEqual("dog", System.Text.Encoding.ASCII.GetString(result[1].String));
        }

        [TestMethod]
        public void Decode_WhenEncodedCollectionIs56To255Bytes()
        {
            var input = RLP.Encode(new List<byte[]>
                {
                    "cat".ToBytes(),
                    "dog".ToBytes(),
                    "this is a collection".ToBytes(),
                    "that when encoded".ToBytes(),
                    "will be greater than 55 bytes".ToBytes(),
                    127.ToBytes(),
                });

            var result = RLP.Decode(input);

            Assert.IsTrue(result.Count == 6);
            Assert.IsTrue(result.All(x => x.IsCollection == false));
            Assert.AreEqual("cat", System.Text.Encoding.ASCII.GetString(result[0].String));
            Assert.AreEqual("dog", System.Text.Encoding.ASCII.GetString(result[1].String));
            Assert.AreEqual("this is a collection", System.Text.Encoding.ASCII.GetString(result[2].String));
            Assert.AreEqual("that when encoded", System.Text.Encoding.ASCII.GetString(result[3].String));
            Assert.AreEqual("will be greater than 55 bytes", System.Text.Encoding.ASCII.GetString(result[4].String));
            Assert.AreEqual(127, result[5].String[0]);
        }
    }
}