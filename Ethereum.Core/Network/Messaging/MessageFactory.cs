using System;
using System.Collections.Generic;
using System.Globalization;

namespace Ethereum.Network.Messaging
{
    public sealed class MessageFactory : IMessageFactory
    {
        private readonly IList<Capability> capabilities = new[]
            {
                new Capability(Capability.ETH),
                //new Capability(Capability.SHH)
            };

        public IMessage Create(P2PMessageCode code)
        {
            switch (code)
            {
                case P2PMessageCode.Hello:
                    return new P2PHelloMessage(1, "Ethereum.NET", capabilities, Config.DefaultPort, "12345"); // TODO: Implement peerId (unique 512bit hash)

                default:
                    throw new ArgumentOutOfRangeException(
                        code.ToString(),
                        code,
                        string.Format(CultureInfo.InvariantCulture, "P2PMessageCode '{0}' not supported", code));
            }
        }

        public IMessage Create(byte[] message)
        {
            P2PMessageCode code;
            Enum.TryParse(message[0].ToString(), out code);

            switch (code)
            {
                case P2PMessageCode.Hello:
                    return new P2PHelloMessage(message);

                default:
                    throw new ArgumentOutOfRangeException(
                        code.ToString(),
                        code,
                        string.Format(CultureInfo.InvariantCulture, "P2PMessageCode '{0}' not supported", code));
            }
        }
    }
}
