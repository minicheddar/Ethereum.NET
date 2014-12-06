using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NEthereum
{
    public sealed class PeerServer : IDisposable
    {
        private static readonly bool enabled;
        private readonly SocketPermission permission;
        private readonly IPEndPoint endPoint;
        private readonly Socket listener;
        private readonly BlockingCollection<string> incomingMessages;

        // https://github.com/ethereum/wiki/wiki/Ethereum-Wire-Protocol
        // https://github.com/ethereum/wiki/wiki/%C3%90%CE%9EVp2p-Wire-Protocol
        // https://github.com/ethereum/wiki/wiki/Light-client-protocol
        // http://codeidol.com/csharp/csharp-network/Asynchronous-Sockets/Using-Asynchronous-Sockets/
        public PeerServer()
        {
            this.permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, string.Empty, Config.Port);
            this.permission.Demand();

            var ipHost = Dns.Resolve(Dns.GetHostName());
            var ipAddress = ipHost.AddressList[0];
            this.endPoint = new IPEndPoint(ipAddress, Config.Port);

            this.listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.listener.Bind(this.endPoint);
            this.listener.Listen(5);

            this.incomingMessages = new BlockingCollection<string>();

            Task.Factory.StartNew(() =>
            {
                while (!this.incomingMessages.IsCompleted)
                {
                    foreach (var msg in this.incomingMessages.GetConsumingEnumerable())
                    {
                        this.ProcessMessage(msg);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public async Task Start()
        {
            while (enabled)
            {
                var handler = await this.listener.AcceptAsync();

                while (true)
                {
                    var buffer = new byte[1024];
                    var bytesReceived = await handler.ReceiveDataAsync(buffer, 0, buffer.Length, SocketFlags.None);
                    if (bytesReceived <= 0) break;

                    var message = Convert.ToBase64String(buffer, 0, bytesReceived);
                    this.incomingMessages.Add(message);
                }
            }
        }

        private void ProcessMessage(string incomingMsg)
        {
            // do work
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
