using System;
using Ethereum.Network.Messaging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ethereum.Tests.Network
{
    [TestClass]
    public class P2PHelloMessageTests : TestSetup
    {
        [TestInitialize]
        public void Setup()
        {
            this.Initialise();
        }

        [TestMethod, Ignore]
        public void GetEncoded_ReturnsCorrectMessageCodeByte()
        {
            var message = new MessageFactory().Create(P2PMessageCode.Hello);

            var encoded = message.Encoded;

            encoded.Length.Should().Be(34);
            encoded[0].Should().Be(Convert.ToByte(P2PMessageCode.Hello));
        }

        [TestMethod, Ignore]
        public void DecodeHex()
        {
            //var testHex = @"f87a8002a5457468657265756d282b2b292f76302e372e392f52656c656173652f4c696e75782f672b2bccc58365746827c583736868018203e0b8401fbf1e41f08078918c9f7b6734594ee56d7f538614f602c71194db0a1af5a77f9b86eb14669fe7a8a46a2dd1b7d070b94e463f4ecd5b337c8b4d31bbf8dd5646";
            //var helloMsg = @"2240089100079F87780980AD457468657265756D282B2B292F5A65726F476F782F76302E342E302F6E6375727365732F4C696E75782F672B2B782765FB8405143FED4F81779805C3542DBB30547C89086B617F2B526F07EF16795473F794BD01C825A7D60E152385A7F47A341E441427D5C3B2B8BF12C9F1FF2BA5F9";
            //var a = @"2240089100000079f877800a80ad457468657265756d282b2b292f5a65726f476f782f76302e342e312f6e6375727365732f4c696e75782f672b2b0782765fb840d8d60c2580fa795cfc0313efdeba869d2194e79e7cb2b522f782ffa0392cbbab8d1bac301208b137e0de4998334f3bcf73fa117ef213f87417089feaf84c21b0";
            var hello2 = @"2240089100000050f84e11f84bc53681cc0a2982765fb840d8d60c2580fa795cfc0313efdeba869d2194e79e7cb2b522f782ffa0392cbbab8d1bac301208b137e0de4998334f3bcf73fa117ef213f87417089feaf84c21b0";
            var bytes = hello2.RemoveWhitespace().HexToBytes();

            //var result = RLP.DecodePacket(bytes);
        }
    }
}
