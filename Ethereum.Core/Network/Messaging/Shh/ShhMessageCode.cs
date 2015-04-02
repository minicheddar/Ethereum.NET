using System.Collections.Generic;

namespace Ethereum.Network.Messaging
{
    public static class ShhMessageCode
    {
        public static int Status = 0x00;
        public static int Message = 0x01;

        public static IEnumerable<int> Values
        {
            get
            {
                yield return Status;
                yield return Message;
            }
        }
    }
}
