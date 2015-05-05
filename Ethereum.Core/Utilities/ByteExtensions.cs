using Ethereum.Utilities;

namespace System
{
    /// <summary>
    /// Extensions for <see cref="System.Byte"/>
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Removes leading 0's from byte arrays
        /// </summary>
        public static byte[] TrimStart(this byte[] source)
        {
            var i = Array.FindIndex(source, x => x != 0);
            var count = Math.Abs(i - source.Length);

            var target = new byte[count];
            Array.Copy(source, i, target, 0, count);

            return target;
        }
        
        /// <summary>
        /// Converts int to big endian bytes
        /// </summary>
        public static byte[] ToBytes(this int source, bool trim = true)
        {
            var bytes = EndianBitConverter.Big.GetBytes(source);

            return trim ? bytes.TrimStart() : bytes;
        }
        
        /// <summary>
        /// Converts string to ASCII bytes
        /// </summary>
        public static byte[] ToBytes(this string source)
        {
            return Text.Encoding.ASCII.GetBytes(source);
        }
    }
}