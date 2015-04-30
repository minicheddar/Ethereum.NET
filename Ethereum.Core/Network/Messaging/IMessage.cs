using System;

namespace Ethereum.Network.Messaging
{
    public interface IMessage
    {
        Guid Id { get; }

        P2PMessageCode MessageCode { get; }

        byte[] Encoded { get; }
    }
}