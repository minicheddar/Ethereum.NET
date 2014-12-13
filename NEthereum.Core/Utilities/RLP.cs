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

        public static IEnumerable<string> Decode(byte[] data, int position = 0, int length = 0, IList<string> decoded = null)
        {
            if (decoded == null)
                decoded = new List<string>();

            if (length == 0)
                length = data.Length;

            while (position < length)
            {
                var lengthByte = Convert.ToInt16(data[position]);
                position++;

                // single byte
                if (lengthByte < ShortItemOffset)
                {
                    decoded.Add(lengthByte.ToString(CultureInfo.InvariantCulture));
                    position += lengthByte;
                }

                // string under 55 bytes
                if (lengthByte > ShortItemOffset && lengthByte <= LargeItemOffset)
                {
                    var itemLength = CalculateItemLength(lengthByte, ShortItemOffset);
                    decoded.Add(Encoding.ASCII.GetString(data, position, itemLength));
                    position += itemLength;
                }

                // string over 55 bytes
                if (lengthByte > LargeItemOffset && lengthByte <= ShortCollectionOffset)
                {
                    var itemLength = Convert.ToInt16(data[position]);
                    position++;
                    decoded.Add(Encoding.ASCII.GetString(data, position, itemLength));
                    position += itemLength;
                }

                // collection under 55 bytes
                if (lengthByte > ShortCollectionOffset && lengthByte <= LargeCollectionOffset)
                {
                    var totalItemsLength = CalculateItemLength(lengthByte, ShortCollectionOffset);

                    while (position < totalItemsLength)
                    {
                        var itemLength = CalculateItemLength(Convert.ToInt16(data[position]), ShortItemOffset); // TODO: check item length +/- 55 bytes
                        position++;
                        decoded.Add(Encoding.ASCII.GetString(data, position, itemLength));
                        position += itemLength;
                    }
                }

                // collection over 55 bytes
                if (lengthByte > LargeCollectionOffset)
                {
                    var itemCount = 0;
                    var totalItems = CalculateItemLength(lengthByte, LargeCollectionOffset);
                    var totalItemsLength = Convert.ToInt16(data[position]);
                    position++;

                    while (itemCount < totalItems)
                    {
                        while (position < totalItemsLength)
                        {
                            var itemLength = CalculateItemLength(Convert.ToInt16(data[position]), ShortItemOffset);  // TODO: check item length +/- 55 bytes
                            position++;
                            decoded.Add(Encoding.ASCII.GetString(data, position, itemLength));
                            position += itemLength;
                        }

                        itemCount++;
                    }
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

            var payloadLength = Convert.ToInt16(data.Skip(SyncTokenLength).SkipWhile(x => x == 0).FirstOrDefault());
            //var decodedMessage = new List<string> {data[9].ToString()};

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

        //private static IList<byte> EncodeLength(int itemLength)
        //{
        //    var lengths = new List<byte>();

        //    // single byte
        //    if (itemLength < ShortItemOffset)
        //    {
        //        lengths.Add(Convert.ToByte(itemLength));
        //        return lengths;
        //    }

        //    // string under 55 bytes
        //    if (itemLength > ShortItemOffset && itemLength <= LargeItemOffset)
        //    {
        //        lengths.Add(Convert.ToByte(ShortItemOffset + itemLength));
        //        return lengths;
        //    }

        //    // string over 55 bytes
        //    if (itemLength > LargeItemOffset && itemLength <= ShortCollectionOffset)
        //    {
        //        lengths.Add(Convert.ToByte(LargeItemOffset + 1)); // TODO: 183 + length of length of bytes
        //        lengths.Add(Convert.ToByte(itemLength));
        //        return lengths;
        //    }

        //    // collection under 55 bytes
        //    if (itemLength > ShortCollectionOffset && itemLength <= LargeCollectionOffset)
        //    {
        //        lengths.Add(Convert.ToByte(ShortCollectionOffset + itemLength));
        //    }

        //    // collection over 55 bytes
        //    if (itemLength > LargeCollectionOffset && itemLength <= MaxItemLength)
        //    {
        //        lengths.Add(Convert.ToByte(ShortCollectionOffset + itemLength)); // TODO: 247 + length of length of bytes
        //        lengths.Add(Convert.ToByte(itemLength));
        //        return lengths;
        //    }

        //    throw new ArgumentOutOfRangeException("itemLength", "length is too long");
        //}
    }
}
