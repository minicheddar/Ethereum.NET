namespace Ethereum.Network.Messaging
{
    public sealed class MessageEncoder : IMessageEncoder
    {
        public byte[] Encode(IMessage message)
        {
            return new byte[0];
        }
    }
}
