using System;

namespace Ethereum.Core
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}