using System.Collections.Generic;

namespace Ethereum
{
    public static class Config
    {
        public static IEnumerable<string> PeerDiscoverySeeds = new[]
            {
                "poc-7.ethdev.com:30303",
                "185.43.109.23:30303"
            };

        public static int DefaultPort = 30303;
        public static bool PeerDiscoveryEnabled = true;
        public static bool TeardownDbOnLoad = true;
    }
}
