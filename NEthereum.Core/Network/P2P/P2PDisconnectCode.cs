using System.Collections.Generic;

namespace NEthereum.Network
{
    public static class P2PDisconnectCode
    {
        public static string Requested = "0x00";
        public static string TCPError = "0x01";
        public static string BadProtocol = "0x02";
        public static string UselessPeer = "0x03";
        public static string TooManyPeers = "0x04";
        public static string AlreadyConnected = "0x05";
        public static string IncompatibleVersion = "0x06";
        public static string NullPeerIdentity = "0x07";
        public static string PeerQuit = "0x08";
        public static string UnexpectedIdentity = "0x09";
        public static string ConnectedToSelf = "0x0a";
        public static string Timeout = "0x0b";
        public static string Other = "0x10";

        public static IEnumerable<string> Values
        {
            get
            {
                yield return Requested;
                yield return TCPError;
                yield return BadProtocol;
                yield return UselessPeer;
                yield return TooManyPeers;
                yield return AlreadyConnected;
                yield return IncompatibleVersion;
                yield return NullPeerIdentity;
                yield return PeerQuit;
                yield return UnexpectedIdentity;
                yield return ConnectedToSelf;
                yield return Timeout;
                yield return Other;
            }
        }
    }
}
