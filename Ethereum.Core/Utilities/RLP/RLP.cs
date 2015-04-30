using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ethereum.Utilities;

namespace Ethereum.Encoding
{
    public static class RLP
    {
        private static readonly BigEndianBitConverter converter = new BigEndianBitConverter();

        private const int SizeThreshold = 55;
        private const int ShortItemOffset = 128;
        private const int LargeItemOffset = 183;
        private const int ShortCollectionOffset = 192;
        private const int LargeCollectionOffset = 247;
        private const int MaxItemLength = 255;

        public static byte[] Encode(int data)
        {
            var bytes = converter.GetBytes(data);

            if (data < ShortItemOffset)
            {
                return new[] { bytes[bytes.Length - 1] };
            }

            return Encode(data.ToString(CultureInfo.InvariantCulture));
        }

        public static byte[] Encode(string input)
        {
            var bytes = System.Text.Encoding.ASCII.GetBytes(input);
            var length = bytes.Length;

            if (length <= SizeThreshold)
            {
                var newBytes = new byte[length + 1];
                newBytes[0] = Convert.ToByte(ShortItemOffset + length);
                Array.Copy(bytes, 0, newBytes, 1, length);

                return newBytes;
            }

            if (length > SizeThreshold && length <= MaxItemLength)
            {
                var newBytes = new byte[length + 2];
                newBytes[0] = Convert.ToByte(LargeItemOffset + 1);  // TODO: 183 + length of length of bytes (how many bytes it needed to fit into)
                newBytes[1] = Convert.ToByte(length);
                Array.Copy(bytes, 0, newBytes, 2, length);

                return newBytes;
            }

            throw new ArgumentOutOfRangeException("input", "input is too long");
        }

        public static byte[] Encode(IEnumerable<dynamic> input)
        {
            var items = new List<byte[]>();
            var totalLength = 0;

            foreach (var bytes in input.Select(x => Encode(x)))
            {
                totalLength += bytes.Length;
                items.Add(bytes);
            }

            if (totalLength <= SizeThreshold)
            {
                items.Insert(0, new[] { Convert.ToByte(ShortCollectionOffset + totalLength) });
            }

            if (totalLength > SizeThreshold && totalLength <= MaxItemLength)
            {
                items.Insert(0, new[] { Convert.ToByte(LargeCollectionOffset + 1) }); // TODO: 247 + length of length of bytes (how many bytes it needed to fit into)
                items.Insert(1, new[] { Convert.ToByte(totalLength) });
            }

            if (totalLength > MaxItemLength)
            {
                throw new ArgumentOutOfRangeException("input", "input is too long");
            }

            return items.SelectMany(x => x).ToArray();
        }

        public static IList<string> Decode(byte[] input)
        {
            var message = new RLPMessage(input);

            while (message.Remainder.Offset < input.Length)
            {
                Decode(message);
            }

            return message.Decoded;
        }

        private static void Decode(RLPMessage msg)
        {
            var firstByte = Convert.ToInt16(msg.Remainder.Array[msg.Remainder.Offset]);

            // single byte
            if (firstByte <= 0x7f)
            {
                msg.Decoded.Add(firstByte.ToString(CultureInfo.InvariantCulture));
                msg.Remainder = msg.Remainder.Slice(1);
                return;
            }

            // string <55 bytes
            if (firstByte <= 0xb7)
            {
                var itemLength = Math.Abs(128 - firstByte);
                var data = firstByte == 0x80 ? new ArraySegment<byte>(new byte[0]) : msg.Remainder.Slice(1, itemLength);

                msg.Decoded.Add(System.Text.Encoding.ASCII.GetString(data.Array, data.Offset, data.Count));
                msg.Remainder = msg.Remainder.Slice(data.Count + 1);
                return;
            }

            // string >55 bytes
            if (firstByte <= 0xbf)
            {
                var listLength = Math.Abs(183 - firstByte);
                var itemLength = Convert.ToInt16(msg.Remainder.Array[msg.Remainder.Offset + 1]);
                var data = msg.Remainder.Slice(listLength + 1, itemLength);

                msg.Decoded.Add(System.Text.Encoding.ASCII.GetString(msg.Remainder.Array, data.Offset, data.Count));
                msg.Remainder = msg.Remainder.Slice(data.Offset + data.Count);
                return;
            }

            // collection <55 bytes
            if (firstByte <= 0xf7)
            {
                var itemLength = Math.Abs(192 - firstByte);
                var data = msg.Remainder.Slice(1, itemLength).ToArray();

                while (msg.Remainder.Offset < msg.Remainder.Array.Length)
                {
                    var decoded = Decode(data);
                    msg.Decoded.AddRange(decoded);
                    msg.Remainder = msg.Remainder.Slice(msg.Remainder.Count);
                }

                return;
            }

            // collection >55 bytes
            if (firstByte <= 0xff)
            {
                var listLength = Math.Abs(247 - firstByte);
                var itemLength = Convert.ToInt16(msg.Remainder.Array[msg.Remainder.Offset + 1]);
                var data = msg.Remainder.Slice(listLength + 1, itemLength).ToArray();

                while (msg.Remainder.Offset < msg.Remainder.Array.Length)
                {
                    var decoded = Decode(data);
                    msg.Decoded.AddRange(decoded);
                    msg.Remainder = msg.Remainder.Slice(msg.Remainder.Count);
                }

                return;
            }

            throw new ArgumentOutOfRangeException("msg", "msg is too long");
        }
    }
}