using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NEthereum.Network;
namespace NEthereum.Tests.Network
{
    [TestClass]
    public class P2PHelloMessageTests
    {
        [TestMethod]
        public void GetEncoded_ReturnsCorrectMessageCodeByte()
        {
            var message = new MessageFactory().CreateHelloMessage();

            var encoded = message.Encoded;

            encoded.Length.Should().Be(34);
            encoded[0].Should().Be(Convert.ToByte(P2PMessageCode.Hello));
        }

    }
}
