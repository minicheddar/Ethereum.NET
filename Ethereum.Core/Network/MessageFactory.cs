using System.Collections.Generic;

namespace Ethereum.Network
{
    public sealed class MessageFactory
    {
        private static readonly IEnumerable<Capability> capabilities = new[]
            {
                new Capability(Capability.ETH),
                new Capability(Capability.SHH)
            };

        /// <summary>
        /// TODO: Implement peerId (unique 512bit hash)
        /// </summary>
        public P2PHelloMessage CreateHelloMessage()
        {
            return new P2PHelloMessage(1, "Ethereum", capabilities, Config.DefaultPort, "12345");
        }
    }
}
