namespace Ethereum.Core
{
    public interface IEventHandler<in TEvent>
    {
        void Handle(TEvent args);
    }
}
