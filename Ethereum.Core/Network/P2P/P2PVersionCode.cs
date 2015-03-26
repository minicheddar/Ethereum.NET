using System.Collections.Generic;

namespace Ethereum.Network
{
    public static class P2PVersionCode
    {
        public static string PoC1 = "0x00";
        public static string PoC2 = "0x01";
        public static string PoC3 = "0x07";
        public static string PoC4 = "0x09";
        public static string PoC5 = "0x17";
        public static string PoC6 = "0x1c";

        public static IEnumerable<string> Values
        {
            get
            {
                yield return PoC1;
                yield return PoC2;
                yield return PoC3;
                yield return PoC4;
                yield return PoC5;
                yield return PoC6;
            }
        }
    }
}
