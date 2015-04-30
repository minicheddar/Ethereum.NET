using System;
using System.Collections.Generic;

namespace Ethereum.Encoding
{
    internal class RLPMessage
    {
        public RLPMessage(byte[] input)
        {
            this.Decoded = new List<string>();
            this.Remainder = new ArraySegment<byte>(input);
        }

        public List<string> Decoded { get; set; }

        public ArraySegment<byte> Remainder { get; set; }
    }
}
