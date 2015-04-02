using System.Collections.Generic;

namespace Ethereum.Core
{
    public interface ISubscriptionService
    {
        IEnumerable<IEventHandler<T>> GetSubscriptions<T>();
    }
}
