namespace Ethereum.Network.Messaging
{
    public interface IMessageEncoding
    {
        int GetPayloadSize(byte[] message);

        byte[] Encode(IMessage message);

        IMessage Decode(byte[] message);
    }
}