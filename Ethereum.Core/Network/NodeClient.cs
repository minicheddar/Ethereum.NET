using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Ethereum.Network
{
    public sealed class NodeClient : IDisposable
    {
        private static readonly bool enabled;
        private readonly SocketPermission permission;
        private readonly IPEndPoint endPoint;
        private readonly Socket sender;
        private readonly BlockingCollection<string> incomingMessages;

        public NodeClient()
        {
            this.permission = new SocketPermission(NetworkAccess.Connect, TransportType.Tcp, string.Empty, Config.DefaultPort);
            this.permission.Demand();

            var ipHost = Dns.Resolve(Dns.GetHostName());
            var ipAddress = ipHost.AddressList[0];
            this.endPoint = new IPEndPoint(ipAddress, Config.DefaultPort);
            this.sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

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
                await this.sender.ConnectAsync(this.endPoint);
                var message = Convert.FromBase64String("message");

                var bytesSent = await this.sender.SendDataAsync(message, 0, message.Length, SocketFlags.None);
                while (true)
                {
                    //var bytesReceived = await handler.ReceiveDataAsync(buffer, 0, buffer.Length, SocketFlags.None);
                    //if (bytesReceived <= 0) break;

                    //var message = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                    //this.incomingMessages.Add(message);
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
