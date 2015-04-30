using System.Collections.Generic;

namespace Ethereum.Network.Messaging
{
    public enum P2PMessageCode
    {
        Hello = 0x00,
        Disconnect = 0x01,
        Ping = 0x02,
        Pong = 0x03,
        GetPeers = 0x04,
        Peers = 0x05,
    }
}
