using System.Collections.Generic;
using System.Linq;
using StructureMap;

namespace Ethereum.Core
{
    public class SubscriptionService : ISubscriptionService
    {
        public IEnumerable<IEventHandler<T>> GetSubscriptions<T>()
        {
            var consumers = ObjectFactory.GetAllInstances(typeof(IEventHandler<T>));

            return consumers.Cast<IEventHandler<T>>();
        }
    }
}
