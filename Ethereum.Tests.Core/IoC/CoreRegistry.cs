using Ethereum.Core;
using Ethereum.Network;
using StructureMap.Configuration.DSL;

namespace Ethereum.Tests.IoC
{
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            For<ISubscriptionService>().Use<SubscriptionService>();
            For<IEventPublisher>().Use<EventPublisher>();
            For<IPeerDataStore>().Singleton().Use<PeerDataStore>();
        }
    }
}
