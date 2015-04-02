using System.Collections.Concurrent;

namespace Ethereum.Network
{
    public sealed class PeerDataStore : IPeerDataStore
    {
        private readonly ConcurrentDictionary<string, Peer> peers;

        public PeerDataStore()
        {
            this.peers = new ConcurrentDictionary<string, Peer>();
        }

        public ConcurrentDictionary<string, Peer> Peers
        {
            get { return this.peers; }
        }

        public void TryAdd(Peer peer)
        {
            var added = this.peers.TryAdd(peer.PeerId, peer);

            if (added)
            {
                // peer added event
            }
        }

        public void TryRemove(string peerId)
        {
            Peer removedPeer;
            var removed = this.peers.TryRemove(peerId, out removedPeer);

            if (removed)
            {
                // peer removed event
            }
        }
    }
}
