using System.Security.Cryptography;

namespace NEthereum.Utilities
{
    public static class RandomGenerator
    {
        private static readonly RNGCryptoServiceProvider _random;

        static RandomGenerator()
        {
            _random = new RNGCryptoServiceProvider();
        }

        public static byte[] GetBytes(int length)
        {
            var bytes = new byte[length];
            _random.GetBytes(bytes);

            return bytes;
        }
    }
}
