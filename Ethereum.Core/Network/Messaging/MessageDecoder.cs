namespace Ethereum.Network.Messaging
{
    public sealed class MessageDecoder
    {
        public IMessage Decode(byte[] message)
        {
            return new P2PHelloMessage(new byte[0]);
        }
    }
}
