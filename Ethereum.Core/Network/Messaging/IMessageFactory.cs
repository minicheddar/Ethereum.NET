namespace Ethereum.Network.Messaging
{
    public interface IMessageFactory
    {
        IMessage Create(P2PMessageCode code);
        IMessage Create(byte[] message);
    }
}