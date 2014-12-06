namespace NEthereum
{
    public static class Config
    {
        public static string[] PeerDiscoverySeeds = new[]
            {
                "poc-7.ethdev.com:30303",
                "185.43.109.23:30303"
            };

        public static int Port = 30303;
        public static bool PeerDiscoveryEnabled = true;
        public static bool TeardownDbOnLoad = true;
    }
}
