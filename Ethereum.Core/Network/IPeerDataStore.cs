using System.Collections.Concurrent;

namespace Ethereum.Network
{
    public interface IPeerDataStore
    {
        ConcurrentDictionary<string, Peer> Peers { get; }

        void TryAdd(Peer peer);

        void TryRemove(string peerId);
    }
}