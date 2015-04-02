using System;
using System.Collections.Generic;
using System.Linq;
using Ethereum.Encoding;
using Ethereum.Utilities;

namespace Ethereum.Network.Messaging
{
    public sealed class P2PHelloMessage : Message
    {
        private readonly int p2pVersion;
        private readonly string clientId;
        private readonly IEnumerable<Capability> capabilities;
        private readonly int defaultPort;
        private readonly string peerId;

        private byte[] encodedMessage = new byte[0];
        private string decodedMessage = string.Empty;

        public P2PHelloMessage(
            int p2pVersion, 
            string clientId,
            IEnumerable<Capability> capabilities,
            int defaultPort,
            string peerId)
        {
            Ensure.Argument.IsNotNull(p2pVersion, "p2pVersion");
            Ensure.Argument.IsNotNullOrEmpty(clientId, "clientId");
            Ensure.Argument.IsNotNullOrEmpty(capabilities, "capabilities");
            Ensure.Argument.IsNotNull(defaultPort, "port");
            Ensure.Argument.IsNotNullOrEmpty(peerId, "peerId");

            this.p2pVersion = p2pVersion;
            this.clientId = clientId;
            this.capabilities = capabilities;
            this.defaultPort = defaultPort;
            this.peerId = peerId;
        }

        public P2PHelloMessage(byte[] message)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");

            this.encodedMessage = message;
        }

        public int MessageCode { get { return P2PMessageCode.Hello; }}

        public byte[] Encoded
        {
            get 
            {
                return encodedMessage.Length == 0 
                        ? this.EncodeMessage() 
                        : this.encodedMessage;
            }
        }

        public string Decoded
        {
            get
            {
                return decodedMessage.Length == 0
                        ? this.DecodeMessage()
                        : this.decodedMessage;
            }
        }

        private byte[] EncodeMessage()
        {
            var caps = new List<byte[]>();

            foreach (var capability in capabilities)
            {
                caps.Add(RLP.Encode(capability.Name));
                caps.Add(RLP.Encode(capability.Version));
            }

            var code = RLP.Encode(P2PMessageCode.Hello);
            var ver = RLP.Encode(this.p2pVersion);
            var client = RLP.Encode(this.clientId);
            var port = RLP.Encode(this.defaultPort);
            var peer = RLP.Encode(this.peerId);

            var message = new List<byte[]>
                {
                    code,
                    ver,
                    client,
                    caps.SelectMany(x => x).ToArray(),
                    port,
                    peer
                }.SelectMany(x => x).ToArray();

            //this.encodedMessage = RLP.EncodePacket(message);

            return this.encodedMessage;
        }

        private string DecodeMessage()
        {
            //var decoded = RLP.DecodePacket(this.encodedMessage);

            //return decoded.ToString();

            return string.Empty;
        }

        public override string ToString()
        {
            return this.Decoded;
        }
    }
}
