using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ethereum.Encoding;

namespace Ethereum.Network.Messaging
{
    public sealed class P2PHelloMessage : Message
    {
        private int p2pVersion;
        private string clientId;
        private IList<Capability> supportedCapabilities;
        private int defaultPort;
        private string peerId;
        private byte[] encodedMessage = new byte[0];

        public P2PHelloMessage(byte[] message)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");

            this.encodedMessage = message;

            this.DecodeMessage();
        }

        public P2PHelloMessage(
            int p2pVersion, 
            string clientId,
            IList<Capability> supportedCapabilities,
            int defaultPort,
            string peerId)
        {
            Ensure.Argument.IsNotNull(p2pVersion, "p2pVersion");
            Ensure.Argument.IsNotNullOrEmpty(clientId, "clientId");
            Ensure.Argument.IsNotNullOrEmpty(supportedCapabilities, "supportedCapabilities");
            Ensure.Argument.IsNotNull(defaultPort, "port");
            Ensure.Argument.IsNotNullOrEmpty(peerId, "peerId");

            this.p2pVersion = p2pVersion;
            this.clientId = clientId;
            this.supportedCapabilities = supportedCapabilities;
            this.defaultPort = defaultPort;
            this.peerId = peerId;
        }

        public override P2PMessageCode MessageCode { get { return P2PMessageCode.Hello; } }

        public override byte[] Encoded
        {
            get
            {
                if (this.encodedMessage.Length == 0)
                {
                    this.EncodeMessage();
                }

                return this.encodedMessage;
            }
        }

        private void EncodeMessage()
        {
            var code = RLP.Encode((int)this.MessageCode);
            var version = RLP.Encode(this.p2pVersion);
            var client = RLP.Encode(this.clientId);

            var capabilities = new List<dynamic>(this.supportedCapabilities.Count);
            capabilities.AddRange(this.supportedCapabilities.Select(capability => new List<string>
                {
                    capability.Name, 
                    capability.Version.ToString(CultureInfo.InvariantCulture)
                }));

            //var capabilities = new byte[this.supportedCapabilities.Count][];
            //for (var i = 0; i < this.supportedCapabilities.Count(); i++)
            //{
            //    var capability = this.supportedCapabilities[i];
            //    capabilities[i] = RLP.Encode(new List<dynamic>
            //        {
            //            capability.Name, 
            //            capability.Version.ToString(CultureInfo.InvariantCulture)
            //        });
            //}

            var caps = RLP.Encode(capabilities);
            var port = RLP.Encode(this.defaultPort);
            var peer = RLP.Encode(this.peerId);

            this.encodedMessage = new List<byte[]>
                {
                    code,
                    version,
                    client,
                    caps,
                    port,
                    peer
                }.SelectMany(x => x).ToArray();
        }

        private void DecodeMessage()
        {
            var decoded = RLP.Decode(this.encodedMessage);
        }
    }
}
