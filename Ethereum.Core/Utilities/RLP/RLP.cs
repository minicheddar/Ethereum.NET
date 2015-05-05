using System;
using System.Collections.Generic;
using System.Linq;

namespace Ethereum.Encoding
{
    public static class RLP
    {
        private const int SizeThreshold = 55;
        private const int ShortItemOffset = 128;
        private const int LargeItemOffset = 183;
        private const int ShortCollectionOffset = 192;
        private const int LargeCollectionOffset = 247;
        private const int MaxItemLength = 255;

        public static byte[] Encode(byte[] item)
        {
            var length = item.Length;

            if (length == 1 && item[0] < 128)
            {
                return item;
            }

            if (length <= 55)
            {
                var encoded = new byte[length + 1];
                encoded[0] = Convert.ToByte(128 + length);
                Array.Copy(item, 0, encoded, 1, length);

                return encoded;
            }

            if (length > 55 && length <= 255)
            {
                var encoded = new byte[length + 2];
                encoded[0] = Convert.ToByte(LargeItemOffset + 1);  // TODO: 183 + length of length of bytes (how many bytes it needed to fit into)
                encoded[1] = Convert.ToByte(length);
                Array.Copy(item, 0, encoded, 2, length);

                return encoded;
            }

            throw new ArgumentOutOfRangeException("item", "item is too long");
        }

        public static byte[] Encode(IList<byte[]> items)
        {
            var encoded = new List<byte[]>();
            var totalLength = 0;

            foreach (var bytes in items.Select(Encode))
            {
                totalLength += bytes.Length;
                encoded.Add(bytes);
            }

            if (totalLength <= SizeThreshold)
            {
                encoded.Insert(0, new[] { Convert.ToByte(ShortCollectionOffset + totalLength) });
            }

            if (totalLength > SizeThreshold && totalLength <= MaxItemLength)
            {
                encoded.Insert(0, new[] { Convert.ToByte(LargeCollectionOffset + 1) }); // TODO: 247 + length of length of bytes (how many bytes it needed to fit into)
                encoded.Insert(1, new[] { Convert.ToByte(totalLength) });
            }

            if (totalLength > MaxItemLength)
            {
                throw new ArgumentOutOfRangeException("items", "items is too long");
            }

            return encoded.SelectMany(x => x).ToArray();
        }

        public static IList<RLPItem> Decode(byte[] input)
        {
            var message = new RLPMessage(input);

            while (message.Remainder.Offset < input.Length)
            {
                Decode(message);
            }

            return message.Data;
        }

        private static void Decode(RLPMessage msg)
        {
            var firstByte = Convert.ToInt16(msg.Remainder.Array[msg.Remainder.Offset]);
            
            // single byte
            if (firstByte <= 0x7f)
            {
                msg.Data.Add(new RLPItem(new[] { msg.Remainder.Array[msg.Remainder.Offset] }));
                msg.Remainder = msg.Remainder.Slice(1);
                return;
            }

            // string 0-55 bytes
            if (firstByte <= 0xb7)
            {
                var itemLength = Math.Abs(128 - firstByte);
                var data = firstByte == 0x80 ? new ArraySegment<byte>(new byte[0]) : msg.Remainder.Slice(1, itemLength);

                msg.Data.Add(new RLPItem(data.ToArray()));
                msg.Remainder = msg.Remainder.Slice(data.Count + 1);
                return;
            }

            // string 56-255 bytes
            if (firstByte <= 0xbf)
            {
                var listLength = Math.Abs(183 - firstByte);
                var itemLength = Convert.ToInt16(msg.Remainder.Array[msg.Remainder.Offset + 1]);
                var data = msg.Remainder.Slice(listLength + 1, itemLength);

                msg.Data.Add(new RLPItem(data.ToArray()));
                msg.Remainder = msg.Remainder.Slice(data.Offset + data.Count);
                return;
            }

            // collection 0-55 bytes
            if (firstByte <= 0xf7)
            {
                var itemLength = Math.Abs(192 - firstByte);
                var data = msg.Remainder.Slice(1, itemLength).ToArray();

                while (msg.Remainder.Offset < msg.Remainder.Array.Length)
                {
                    var decoded = Decode(data);
                    msg.Data.AddRange(decoded);
                    msg.Remainder = msg.Remainder.Slice(msg.Remainder.Count);
                }

                return;
            }

            // collection 56-255 bytes
            if (firstByte <= 0xff)
            {
                var listLength = Math.Abs(247 - firstByte);
                var itemLength = Convert.ToInt16(msg.Remainder.Array[msg.Remainder.Offset + 1]);
                var data = msg.Remainder.Slice(listLength + 1, itemLength).ToArray();

                while (msg.Remainder.Offset < msg.Remainder.Array.Length)
                {
                    var decoded = Decode(data);
                    msg.Data.AddRange(decoded);
                    msg.Remainder = msg.Remainder.Slice(msg.Remainder.Count);
                }

                return;
            }

            throw new ArgumentOutOfRangeException("msg", "msg is too long");
        }
    }
}