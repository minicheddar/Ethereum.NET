using System;
using System.Collections.Generic;
using System.Linq;
using Ethereum.Utilities;

namespace Ethereum.Network.Messaging
{
    public sealed class MessageEncoding : IMessageEncoding
    {
        private readonly BigEndianBitConverter bitConverter = new BigEndianBitConverter();
        private readonly IMessageFactory messageFactory;

        public MessageEncoding(IMessageFactory messageFactory)
        {
            Ensure.Argument.IsNotNull(messageFactory, "messageFactory");

            this.messageFactory = messageFactory;
        }

        public int GetPayloadSize(byte[] data)
        {
            if (StartsWithSyncToken(data) && data.Length >= 8)
            {
                return bitConverter.ToInt32(data, 4);
            }

            return 0;
        }

        public byte[] Encode(IMessage message)
        {
            var payload = message.Encoded;

            return new List<byte[]>
                {
                    Config.SyncToken,
                    this.bitConverter.GetBytes(payload.Length),
                    payload
                }.SelectMany(x => x).ToArray();
        }

        public IMessage Decode(byte[] message)
        {
            return this.messageFactory.Create(message);
        }

        private static bool StartsWithSyncToken(byte[] data)
        {
            return data.Length >= 4 &&
                   data[0] == Config.SyncToken[0] &&
                   data[1] == Config.SyncToken[1] &&
                   data[2] == Config.SyncToken[2] &&
                   data[3] == Config.SyncToken[3];
        }
    }
}
