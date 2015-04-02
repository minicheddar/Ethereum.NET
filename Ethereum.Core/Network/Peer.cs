using System;
using System.Net;

namespace Ethereum.Network
{
    public sealed class Peer
    {
        private readonly string peerId;
        private readonly IPEndPoint endpoint;

        public Peer(string peerId, IPEndPoint endpoint)
        {
            Ensure.Argument.IsNotNullOrEmpty(peerId, "peerId");
            Ensure.Argument.IsNotNull(endpoint, "endpoint");

            this.peerId = peerId;
            this.endpoint = endpoint;
        }

        public string PeerId
        {
            get { return this.peerId; }
        }

        public IPEndPoint Endpoint
        {
            get { return this.endpoint; }
        }
    }
}
