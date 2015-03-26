using System.Collections.Generic;

namespace Ethereum.Network
{
    public static class P2PMessageCode
    {
        public static int Hello = 0x00;
        public static int Disconnect = 0x01;
        public static int Ping = 0x02;
        public static int Pong = 0x03;
        public static int GetPeers = 0x04;
        public static int Peers = 0x05;

        public static IEnumerable<int> Values
        {
            get
            {
                yield return Hello;
                yield return Disconnect;
                yield return Ping;
                yield return Pong;
                yield return GetPeers;
                yield return Peers;
            }
        }
    }
}
