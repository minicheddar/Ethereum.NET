using System.Collections.Generic;

namespace NEthereum.p2p
{
    public static class P2PMessageCodes
    {
        public static string Hello = "0x00";
        public static string Disconnect = "0x01";
        public static string Ping = "0x02";
        public static string Pong = "0x03";
        public static string GetPeers = "0x04";
        public static string Peers = "0x05";

        public static IEnumerable<string> Values
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
