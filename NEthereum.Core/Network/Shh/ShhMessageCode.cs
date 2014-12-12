using System.Collections.Generic;

namespace NEthereum.Network
{
    public static class ShhMessageCode
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
