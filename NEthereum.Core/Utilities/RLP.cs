using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NEthereum.Utilities
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

        private const int SizeThreshold = 55;
        private const int ShortItemOffset = 128;
        private const int LargeItemOffset = 183;
        private const int ShortCollectionOffset = 192;
        private const int LargeCollectionOffset = 247;

        public static byte[] Encode(int data)
        {
            var bytes = converter.GetBytes(data);

            if (data < ShortItemOffset)
            {
                return new[] { bytes[bytes.Length - 1] };
            }

            return Encode(data.ToString(CultureInfo.InvariantCulture));
        }

        public static byte[] Encode(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            var length = bytes.Length;

            if (length <= SizeThreshold)
            {
                var newBytes = new byte[length + 1];
                newBytes[0] = Convert.ToByte(ShortItemOffset + length);
                Array.Copy(bytes, 0, newBytes, 1, length);

                return newBytes;
            }

            if (length > SizeThreshold)
            {
                var newBytes = new byte[length + 2];
                newBytes[0] = Convert.ToByte(LargeItemOffset + 1);  // TODO: 183 + length of length of bytes
                newBytes[1] = Convert.ToByte(length);
                Array.Copy(bytes, 0, newBytes, 2, length);

                return newBytes;
            }

            throw new ArgumentOutOfRangeException("data", "data is too long");
        }

        public static byte[] Encode(IEnumerable<string> data)
        {
            var items = new List<byte[]>();
            var count = 0;

            foreach (var bytes in data.Select(Encode))
            {
                count += bytes.Length;
                items.Add(bytes);
            }

            if (count <= SizeThreshold)
            {
                items.Insert(0, new[] { Convert.ToByte(ShortCollectionOffset + count) });
            }

            if (count > SizeThreshold)
            {
                items.Insert(0, new[] { Convert.ToByte(LargeCollectionOffset + 1) }); // TODO: 247 + length of length of bytes
                items.Insert(1, new[] { Convert.ToByte(count) });
            }

            return items.SelectMany(x => x).ToArray();
        }

        public static byte[] Encode(IList<byte[]> data)
        {
            var count = data.Sum(item => item.Length);

            if (count <= SizeThreshold)
            {
                data.Insert(0, new[] { Convert.ToByte(ShortCollectionOffset + count) });
            }

            if (count > SizeThreshold)
            {
                data.Insert(0, new[] { Convert.ToByte(LargeCollectionOffset + 1) }); // TODO: 247 + length of length of bytes
                data.Insert(1, new[] { Convert.ToByte(count) });
            }

            return data.SelectMany(x => x).ToArray();
        }

        public static IEnumerable<string> Decode(byte[] data)
        {
            var message = new List<string>();
            var index = 1;
            var lengthByte = Convert.ToInt16(data[0]);

            // single byte
            if (lengthByte < ShortItemOffset)
            {
                message.Add(lengthByte.ToString(CultureInfo.InvariantCulture));
            }

            // item under 55 bytes
            if (lengthByte > ShortItemOffset && lengthByte <= LargeItemOffset)
            {
                var itemLength = lengthByte - ShortItemOffset;
                message.Add(Encoding.ASCII.GetString(data, index, itemLength));
            }

            // item over 55 bytes
            if (lengthByte > LargeItemOffset && lengthByte <= ShortCollectionOffset)
            {
                var itemLength = Convert.ToInt16(data[index]);
                index++;
                message.Add(Encoding.ASCII.GetString(data, index, itemLength));
            }

            // collection under 55 bytes
            if (lengthByte > ShortCollectionOffset && lengthByte <= LargeCollectionOffset)
            {
                var totalItemsLength = lengthByte - ShortCollectionOffset;

                while (index < totalItemsLength)
                {
                    lengthByte = Convert.ToInt16(data[index]);
                    var itemLength = lengthByte - ShortItemOffset; // TODO: check item length +/- 55 bytes
                    index++;
                    message.Add(Encoding.ASCII.GetString(data, index, itemLength));
                    index += itemLength;
                }
            }

            // collection over 55 bytes
            if (lengthByte > LargeCollectionOffset)
            {
                var totalItemsLength = Convert.ToInt16(data[index]);
                index++;

                while (index < totalItemsLength)
                {
                    lengthByte = Convert.ToInt16(data[index]);
                    var itemLength = lengthByte - ShortItemOffset;  // TODO: check item length +/- 55 bytes
                    index++;
                    message.Add(Encoding.ASCII.GetString(data, index, itemLength));
                    index += itemLength;
                }
            }

            return message;
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
            return Decode(data.Skip(4).SkipWhile(x => x == 0).ToArray());
        }
    }
}
