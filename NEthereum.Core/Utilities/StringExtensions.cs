using System.Linq;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// Extensions for <see cref="System.String"/>
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// A nicer way of calling <see cref="System.String.IsNullOrEmpty(string)"/>
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// A nicer way of calling the inverse of <see cref="System.String.IsNullOrEmpty(string)"/>
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the value parameter is not null or an empty string (""); otherwise, false.</returns>
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !value.IsNullOrEmpty();
        }

        public static byte[] HexToBytes(this string value)
        {
            return Enumerable.Range(0, value.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(value.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string RemoveWhitespace(this string value)
        {
            return Regex.Replace(value, @"\s+", "");
        }

        public static string BytesToHex(this byte[] value)
        {
            return BitConverter.ToString(value, 0).Replace("-", string.Empty);
        }
    }
}
