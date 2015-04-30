using Ethereum.Core;
using Ethereum.Network;
using Ethereum.Network.Messaging;
using StructureMap.Configuration.DSL;

namespace Ethereum.Tests.IoC
{
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            For<IPeerDataStore>().Singleton().Use<PeerDataStore>();
            For<INodeClient>().Singleton().Use<NodeClient>();
            For<INodeServer>().Singleton().Use<NodeServer>();

            For<IEventPublisher>().Use<EventPublisher>();
            For<ISubscriptionService>().Use<SubscriptionService>();
            For<IMessageEncoder>().Use<MessageEncoder>();
            For<IMessageDecoder>().Use<MessageDecoder>();
        }
    }
}
