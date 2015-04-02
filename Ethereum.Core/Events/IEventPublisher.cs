namespace Ethereum.Core
{
    public interface IEventPublisher
    {
        void Publish<T>(T eventMessage);
    }
}
