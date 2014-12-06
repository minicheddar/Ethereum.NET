using System.Collections.Generic;

namespace NEthereum.p2p.shh
{
    public static class ShhMessageCodes
    {
        public static string Status = "0x00";
        public static string Message = "0x01";

        public static IEnumerable<string> Values
        {
            get
            {
                yield return Status;
                yield return Message;
            }
        }
    }
}
