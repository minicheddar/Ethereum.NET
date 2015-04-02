namespace Ethereum.Network.Messaging
{
    public interface IMessageDecoder
    {
        IMessage Decode(byte[] message);
    }
}