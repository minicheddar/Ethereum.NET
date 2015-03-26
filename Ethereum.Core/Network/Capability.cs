using System;

namespace Ethereum.Network
{
    public sealed class Capability
    {
        private readonly string name;

        public static readonly string ETH = "eth";
        public static readonly string SHH = "shh";

        public Capability(string name)
        {
            Ensure.Argument.IsNotNullOrEmpty(name, "name");

            this.name = name;
        }

        public string Name
        {
            get { return this.name; }
        }

        public int Version
        {
            get { return this.name == ETH ? 34 : 1; }
        }
    }
}
