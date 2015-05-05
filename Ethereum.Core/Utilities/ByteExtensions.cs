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
    }
}