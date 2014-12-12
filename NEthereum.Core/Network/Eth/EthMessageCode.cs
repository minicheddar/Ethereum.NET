using System.Collections.Generic;

namespace NEthereum.Network
{
    public static class EthMessageCode
    {
        public static string Status = "0x00";
        public static string Transactions = "0x02";
        public static string GetBlockHashes = "0x03";
        public static string BlockHashes = "0x04";
        public static string GetBlocks = "0x05";
        public static string Blocks = "0x06";
        public static string NewBlock = "0x07";

        public static IEnumerable<string> Values
        {
            get
            {
                yield return Status;
                yield return Transactions;
                yield return GetBlockHashes;
                yield return BlockHashes;
                yield return GetBlocks;
                yield return Blocks;
                yield return NewBlock;
            }
        }
    }
}
