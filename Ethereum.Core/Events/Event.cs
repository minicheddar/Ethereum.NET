using System;

namespace Ethereum.Core
{
    public abstract class Event : IEvent
    {
        protected Event()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }
}
