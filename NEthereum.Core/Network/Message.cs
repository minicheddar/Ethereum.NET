using System;

namespace NEthereum.Network
{
    /// <summary>
    /// Base class for all message types
    /// </summary>
    public abstract class Message
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
    }
}
