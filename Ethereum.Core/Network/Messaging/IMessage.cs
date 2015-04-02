using System;

namespace Ethereum.Network.Messaging
{
    public interface IMessage
    {
        Guid Id { get; }
    }
}