using System;
using System.Collections.Generic;

namespace Ethereum.Encoding
{
    internal class RLPMessage
    {
        public RLPMessage(byte[] input)
        {
            this.Data = new List<RLPItem>();
            this.Remainder = new ArraySegment<byte>(input);
        }

        public List<RLPItem> Data { get; set; }

        public ArraySegment<byte> Remainder { get; set; }
    }
}
