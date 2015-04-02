using System.Collections.Generic;

namespace Ethereum.Network
{
    public static class P2PDisconnectCode
    {
        public static int Requested = 0x00;
        public static int TCPError = 0x01;
        public static int BadProtocol = 0x02;
        public static int UselessPeer = 0x03;
        public static int TooManyPeers = 0x04;
        public static int AlreadyConnected = 0x05;
        public static int IncompatibleVersion = 0x06;
        public static int NullPeerIdentity = 0x07;
        public static int PeerQuit = 0x08;
        public static int UnexpectedIdentity = 0x09;
        public static int ConnectedToSelf = 0x0a;
        public static int Timeout = 0x0b;
        public static int Other = 0x10;

        public static IEnumerable<int> Values
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
