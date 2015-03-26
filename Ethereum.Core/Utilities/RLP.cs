using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ethereum.Utilities
{

    /// <summary>
    /// TODO: Refactor
    /// 
    /// Implementation of the 'Recursive Length Prefix' serialisation protocol
    /// https://github.com/ethereum/wiki/wiki/RLP
    /// </summary>
    public static class RLP
    {
        private static readonly BigEndianBitConverter converter = new BigEndianBitConverter();
        private static readonly byte[] syncToken = new byte[] { 34, 64, 8, 145 };

        private const int SyncTokenLength = 4;
        private const int MessageStartIndex = 8;
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
            var bytes = Encoding.ASCII.GetBytes(input);
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

        public static byte[] Encode(IEnumerable<string> input)
        {
            var items = new List<byte[]>();
            var totalLength = 0;

            foreach (var bytes in input.Select(Encode))
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

        public static IEnumerable<string> Decode(byte[] input, int position = 0, int length = 0, IList<string> decoded = null)
        {
            if (decoded == null)
                decoded = new List<string>();

            if (length == 0)
                length = input.Length;

            while (position < length)
            {
                var firstByte = Convert.ToInt16(input[position]);

                // single byte
                if (firstByte < ShortItemOffset)
                {
                    decoded.Add(firstByte.ToString(CultureInfo.InvariantCulture));
                    position++;
                    continue;
                }

                // string under 55 bytes
                if (firstByte <= LargeItemOffset)
                {
                    var itemLength = CalculateItemLength(firstByte, 0x7f);
                    decoded.Add(Encoding.ASCII.GetString(input, position + 1, itemLength - 1));
                    position += itemLength;
                    continue; 
                }

                // string over 55 bytes
                if (firstByte < ShortCollectionOffset)
                {
                    position++;
                    var itemLength = Convert.ToInt16(input[position]);
                    position++;
                    decoded.Add(Encoding.ASCII.GetString(input, position, itemLength));
                    position += itemLength;
                    continue;
                }

                // collection under 55 bytes
                if (firstByte <= LargeCollectionOffset)
                {
                    var totalItemsLength = CalculateItemLength(firstByte, ShortCollectionOffset);
                    var offset = totalItemsLength > SizeThreshold ? LargeItemOffset : ShortItemOffset;
                    position++;

                    while (position < totalItemsLength)
                    {
                        var itemLength = CalculateItemLength(Convert.ToInt16(input[position]), offset);
                        position++;
                        decoded.Add(Encoding.ASCII.GetString(input, position, itemLength));
                        position += itemLength;
                    }

                    continue;
                }

                // collection over 55 bytes
                if (firstByte <= MaxItemLength)
                {
                    position++;
                    var itemCount = 0;
                    var totalItems = CalculateItemLength(firstByte, LargeCollectionOffset);
                    var totalItemsLength = Convert.ToInt16(input[position]);

                    while (itemCount < totalItems)
                    {
                        position++;

                        Decode(input, position, totalItemsLength, decoded);

                        itemCount++;
                        position += totalItemsLength;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("input", "input is too long");
                }
            }

            return decoded;
        }

        public static byte[] EncodePacket(byte[] payload)
        {
            return new List<byte[]>
                {
                    syncToken, 
                    converter.GetBytes(payload.Length), 
                    payload
                }
                .SelectMany(x => x)
                .ToArray();
        }

        public static IEnumerable<string> DecodePacket(byte[] data)
        {
            if (!StartsWithSyncToken(data))
            {
                return Decode(data);
            }

            var payloadLength = Convert.ToInt16(data.Skip(SyncTokenLength)
                                       .SkipWhile(x => x == 0)
                                       .FirstOrDefault());

            return Decode(data, MessageStartIndex, payloadLength);
        }
        
        private static bool StartsWithSyncToken(byte[] data)
        {
            return data.Length >= 4 && 
                   data[0] == syncToken[0] && 
                   data[1] == syncToken[1] &&
                   data[2] == syncToken[2] &&
                   data[3] == syncToken[3];
        }

        private static int CalculateItemLength(int length, int offset)
        {
            var max = Math.Max(length, offset);
            var min = Math.Min(length, offset);

            return max - min;
        }
    }
}
