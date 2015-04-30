using System;

namespace Ethereum.Network.Messaging
{
    /// <summary>
    /// Base class for all message types
    /// </summary>
    public abstract class Message : IMessage
    {
        private readonly Guid id;

        protected Message()
        {
            this.id = Guid.NewGuid();
        }

        public Guid Id
        {
            get { return this.id; }
        }

        public abstract P2PMessageCode MessageCode { get; }

        public abstract byte[] Encoded { get; }
    }
}
