using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Ethereum.Network;
using Ethereum.Network.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ethereum.Tests.Network
{
    [TestClass]
    public class NodeTests : TestSetup
    {
        [TestMethod]
        public void TestMethod1()
        {
            Debug.Flush();
            var server = new NodeServer(new MessageEncoding(new MessageFactory()));
            var client = new NodeClient(new MessageEncoding(new MessageFactory()));

            server.Start();
            client.SendMessage();

            Thread.Sleep(1000);
        }
    }
}
