using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ethereum.Encoding
{
    [TestClass]
    public class RLPTests
    {
        /// <summary>
        /// Given data to be encoded in RLP
        /// When encoded data is single byte and less than or equal to 127
        /// Then the original data is returned
        /// </summary>
        [TestMethod]
        public void Encode_WhenSingleByteIsLessThanOrEqualTo127ReturnByte()
        {
            var result = RLP.Encode(127);

            Assert.IsTrue(result.GetType() == typeof(byte[]));
            Assert.IsTrue(result.Length == 1);
            Assert.IsTrue(result[0] == 127);
        }

        /// <summary>
        /// Given data to be encoded in RLP
        /// When encoded data is a 0-55 bytes
        /// Then the first byte is 128 + length of the encoded string
        /// Then followed by the string in bytes
        /// </summary>
        [TestMethod]
        public void Encode_WhenEncodedStringIs0To55BytesReturnCorrectBytes()
        {
            var result = RLP.Encode("dog");

            Assert.IsTrue(result.GetType() == typeof(byte[]));
            Assert.IsTrue(result.Length == 4);
            Assert.IsTrue(result[0] == 131);
            Assert.IsTrue(result[1] == 100);
            Assert.IsTrue(result[2] == 111);
            Assert.IsTrue(result[3] == 103);
        }

        /// <summary>
        /// Given data to be encoded in RLP
        /// When encoded data is > 55 bytes
        /// Then the first byte is 183 + length of the number of bytes
        /// Then followed by the length of the encoded string
        /// Then followed by the string in bytes
        /// </summary>
        [TestMethod]
        public void Encode_WhenEncodedStringIsGreaterThan55BytesReturnCorrectBytes()
        {
            var result = RLP.Encode("This is a sentence. A sentence that is longer than 55 bytes.");

            Assert.IsTrue(result.GetType() == typeof(byte[]));
            Assert.IsTrue(result.Length == 62);
            Assert.IsTrue(result[0] == 184);
            Assert.IsTrue(result[1] == 60);
            Assert.IsTrue(result[2] == 84);
            Assert.IsTrue(result[61] == 46);
        }

        /// <summary>
        /// Given data to be encoded in RLP
        /// When total length of encoded collection is 0-55 bytes
        /// Then the first byte is 192 + length of the encoded items
        /// Then followed by the encoded items
        /// </summary>
        [TestMethod]
        public void Encode_WhenEncodedCollectionIs0To55BytesReturnCorrectBytes()
        {
            var result = RLP.Encode(new List<string> { "cat", "dog" });

            Assert.IsTrue(result.GetType() == typeof(byte[]));
            Assert.IsTrue(result.Length == 9);
            Assert.IsTrue(result[0] == 200);
            Assert.IsTrue(result[1] == 131);
            Assert.IsTrue(result[5] == 131);
        }

        /// <summary>
        /// Given data to be encoded in RLP
        /// When total length of encoded collection is > 55 bytes
        /// Then the first byte is 247 + length of the number of bytes
        /// Then followed by the length of the encoded items
        /// Then followed by the encoded items
        /// </summary>
        [TestMethod]
        public void Encode_WhenEncodedCollectionIsGreaterThan55BytesReturnCorrectBytes()
        {
            var result = RLP.Encode(new List<string>
                {
                    "this is a collection",
                    "that when encoded",
                    "will be greater than 55 bytes",
                });

            Assert.IsTrue(result.GetType() == typeof(byte[]));
            Assert.IsTrue(result.Length == 71);
            Assert.IsTrue(result[0] == 248);
            Assert.IsTrue(result[1] == 69);
            Assert.IsTrue(result[2] == 148);
            Assert.IsTrue(result[70] == 115);
        }

        [TestMethod]
        public void Decode_WhenDataIsSingleByteAndLessThanOrEqualTo127ReturnValue()
        {
            var bytes = RLP.Encode(10);

            var result = RLP.Decode(bytes);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0] == "10");
        }

        [TestMethod]
        public void Decode_WhenEncodedDataIs0To55BytesReturnCorrectValue()
        {
            var bytes = RLP.Encode("cheese on toast");

            var result = RLP.Decode(bytes);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0] == "cheese on toast");
        }

        [TestMethod]
        public void Decode_WhenEncodedDataIsGreaterThan55BytesReturnCorrectValue()
        {
            var bytes = RLP.Encode("This is a sentence. A sentence that is longer than 55 bytes.");

            var result = RLP.Decode(bytes);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0] == "This is a sentence. A sentence that is longer than 55 bytes.");
        }

        [TestMethod]
        public void Decode_WhenEncodedCollectionIs0To55BytesReturnCorrectValues()
        {
            var bytes = RLP.Encode(new List<string> { "cat", "dog" });

            var result = RLP.Decode(bytes);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0] == "cat");
            Assert.IsTrue(result[1] == "dog");
        }

        [TestMethod]
        public void Decode_WhenDecodedCollectionIsGreaterThan55BytesReturnCorrectValues()
        {
            var bytes = RLP.Encode(new List<string>
                {
                    "cat",
                    "dog",
                    "this is a collection",
                    "that when encoded",
                    "will be greater than 55 bytes",
                    "127"
                });

            var result = RLP.Decode(bytes);

            Assert.IsTrue(result.Count == 6);
            Assert.IsTrue(result[0] == "cat");
            Assert.IsTrue(result[1] == "dog");
            Assert.IsTrue(result[2] == "this is a collection");
            Assert.IsTrue(result[3] == "that when encoded");
            Assert.IsTrue(result[4] == "will be greater than 55 bytes");
            Assert.IsTrue(result[5] == "127");
        }
    }
}