using System.Collections.Generic;

namespace Ethereum.Network
{
    public static class EthMessageCode
    {
        public static int Status = 0x00;
        public static int Transactions = 0x02;
        public static int GetBlockHashes = 0x03;
        public static int BlockHashes = 0x04;
        public static int GetBlocks = 0x05;
        public static int Blocks = 0x06;
        public static int NewBlock = 0x07;

        public static IEnumerable<int> Values
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
