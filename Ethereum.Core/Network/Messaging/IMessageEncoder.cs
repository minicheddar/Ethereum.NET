namespace Ethereum.Network.Messaging
{
    public interface IMessageEncoder
    {
        byte[] Encode(IMessage message);
    }
}