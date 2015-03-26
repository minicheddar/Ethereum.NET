using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ethereum.Utilities;

namespace Ethereum.Tests.Core
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

            result.Should().BeOfType<byte[]>().And.HaveCount(1);
            result[0].Should().Be(127);
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

            result.Should().BeOfType<byte[]>().And.HaveCount(4);
            result[0].Should().Be(131);
            result[1].Should().Be(100);
            result[2].Should().Be(111);
            result[3].Should().Be(103);
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

            result.Should().BeOfType<byte[]>().And.HaveCount(62);
            result[0].Should().Be(184);
            result[1].Should().Be(60);
            result[2].Should().Be(84);
            result[61].Should().Be(46);
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
            var result = RLP.Encode(new List<string> {"cat", "dog"});

            result.Should().BeOfType<byte[]>().And.HaveCount(9);
            result[0].Should().Be(200);
            result[1].Should().Be(131);
            result[5].Should().Be(131);
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

            result.Should().BeOfType<byte[]>().And.HaveCount(71);
            result[0].Should().Be(248);
            result[1].Should().Be(69);
            result[2].Should().Be(148);
            result[70].Should().Be(115);
        }

        [TestMethod]
        public void Decode_WhenDataIsSingleByteAndLessThanOrEqualTo127ReturnValue()
        {
            var bytes = RLP.Encode(10);

            var result = RLP.Decode(bytes).ToList();

            result.Should().HaveCount(1);
            result[0].Should().Be("10");
        }

        [TestMethod]
        public void Decode_WhenEncodedDataIs0To55BytesReturnCorrectValue()
        {
            var bytes = RLP.Encode("cheese on toast");

            var result = RLP.Decode(bytes).ToList();

            result.Should().HaveCount(1);
            result[0].Should().Be("cheese on toast");
        }

        [TestMethod]
        public void Decode_WhenEncodedDataIsGreaterThan55BytesReturnCorrectValue()
        {
            var bytes = RLP.Encode("This is a sentence. A sentence that is longer than 55 bytes.");

            var result = RLP.Decode(bytes).ToList();

            result.Should().HaveCount(1);
            result[0].Should().Be("This is a sentence. A sentence that is longer than 55 bytes.");
        }

        [TestMethod]
        public void Decode_WhenEncodedCollectionIs0To55BytesReturnCorrectValues()
        {
            var bytes = RLP.Encode(new List<string> { "cat", "dog" });

            var result = RLP.Decode(bytes).ToList();

            result.Should().HaveCount(2);
            result[0].Should().Be("cat");
            result[1].Should().Be("dog");
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
                });

            var result = RLP.Decode(bytes).ToList();

            result.Should().HaveCount(5);
            result[0].Should().Be("cat");
            result[1].Should().Be("dog");
            result[2].Should().Be("this is a collection");
            result[3].Should().Be("that when encoded");
            result[4].Should().Be("will be greater than 55 bytes");
        }
    }
}
