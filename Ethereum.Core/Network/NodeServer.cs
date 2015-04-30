using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Ethereum.Network.Messaging;

namespace Ethereum.Network
{
    public sealed class NodeServer : INodeServer, IDisposable
    {
        private static readonly bool enabled = true;
        private readonly IMessageEncoding messageDecoder;
        private readonly SocketPermission permission;
        private readonly Socket listener;
        private readonly BlockingCollection<byte[]> incomingMessages;

        public NodeServer(IMessageEncoding messageDecoder)
        {
            Ensure.Argument.IsNotNull(messageDecoder, "messageDecoder");

            this.messageDecoder = messageDecoder;

            this.permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, string.Empty, Config.DefaultPort);
            this.permission.Demand();

            var ipHost = Dns.Resolve(Dns.GetHostName());
            var ipAddress = ipHost.AddressList[0];

            this.listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.listener.Bind(new IPEndPoint(ipAddress, Config.DefaultPort));
            this.listener.Listen(5);

            this.incomingMessages = new BlockingCollection<byte[]>();

            Task.Factory.StartNew(() =>
            {
                while (!this.incomingMessages.IsCompleted)
                {
                    foreach (var msg in this.incomingMessages.GetConsumingEnumerable())
                    {
                        this.DecodeMessage(msg);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public async Task Start()
        {
            //while (enabled)
            //{
            //    var handler = await this.listener.AcceptAsync();

            //    while (true)
            //    {
            //        var buffer = new byte[1024];
            //        var bytesReceived = await handler.ReceiveDataAsync(buffer, 0, buffer.Length, SocketFlags.None);
            //        if (bytesReceived <= 0) break;

            //        var message = Convert.ToBase64String(buffer, 0, bytesReceived);
            //        //this.incomingMessages.Add(buffer);
            //    }
            //}
        }

        public async Task Connect()
        {
            var handler = await this.listener.AcceptAsync();

            while (true)
            {
                var buffer = new byte[1024];
                var bytesReceived = await handler.ReceiveDataAsync(buffer, 0, buffer.Length, SocketFlags.None);
                if (bytesReceived < 8) continue;

                var payloadSize = this.messageDecoder.GetPayloadSize(buffer);
                if (payloadSize == 0) break;

                if (bytesReceived == payloadSize + 8)
                {
                    var payload = new byte[payloadSize];
                    Array.Copy(buffer, 8, payload, 0, payloadSize);

                    this.incomingMessages.Add(payload);
                }
            }
        }

        private void DecodeMessage(byte[] payload)
        {
            var message = this.messageDecoder.Decode(payload);

            // raise new incoming message event
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
