using System;
using System.Collections.Generic;
namespace Ethereum.Encoding
{
    public class RLPItem
    {
        public RLPItem(byte[] item)
        {
            this.String = item;
            this.Collection = new List<byte[]>();
            this.IsCollection = false;
        }

        public byte[] String { get; private set; }

        public List<byte[]> Collection { get; private set; }

        public bool IsCollection { get; private set; }
    }
}
