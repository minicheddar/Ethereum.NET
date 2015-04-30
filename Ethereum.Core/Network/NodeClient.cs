using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Ethereum.Network.Messaging;

namespace Ethereum.Network
{
    public sealed class NodeClient : INodeClient, IDisposable
    {
        private static readonly bool enabled = true;
        private readonly IMessageEncoding messageEncoder;
        private readonly SocketPermission permission;
        private readonly IPEndPoint endPoint;
        private readonly Socket sender;
        private readonly BlockingCollection<IMessage> outgoingMessages;

        public NodeClient(IMessageEncoding messageEncoder)
        {
            Ensure.Argument.IsNotNull(messageEncoder, "messageDecoder");

            this.messageEncoder = messageEncoder;

            this.permission = new SocketPermission(NetworkAccess.Connect, TransportType.Tcp, string.Empty, Config.DefaultPort);
            this.permission.Demand();

            var ipHost = Dns.Resolve(Dns.GetHostName());
            var ipAddress = ipHost.AddressList[0];
            this.endPoint = new IPEndPoint(ipAddress, Config.DefaultPort);
            this.sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            this.outgoingMessages = new BlockingCollection<IMessage>();

            this.sender.Connect(this.endPoint);
            //Task.Factory.StartNew(() =>
            //{
            //    while (!this.outgoingMessages.IsCompleted)
            //    {
            //        foreach (var msg in this.outgoingMessages.GetConsumingEnumerable())
            //        {
            //            this.SendMessage(msg);
            //        }
            //    }
            //}, TaskCreationOptions.LongRunning);
        }

        public async Task Start()
        {
            //while (enabled)
            //{
            //    //await this.sender.ConnectAsync(this.endPoint);
            //    this.sender.Connect(this.endPoint);
            //    var message = Convert.FromBase64String("message");

            //    var a = this.sender.Send(message, 0, message.Length, SocketFlags.None);

            //    //var bytesSent = await this.sender.SendDataAsync(message, 0, message.Length, SocketFlags.None);
            //    //while (true)
            //    //{
            //    //    //var bytesReceived = await handler.ReceiveDataAsync(buffer, 0, buffer.Length, SocketFlags.None);
            //    //    //if (bytesReceived <= 0) break;

            //    //    //var message = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
            //    //    //this.incomingMessages.Add(message);
            //    //}
            //}
        }

        public void SendMessage()
        {
            var message = this.messageEncoder.Encode(new MessageFactory().Create(P2PMessageCode.Hello));
            var bytesSent = this.sender.Send(message, 0, message.Length, SocketFlags.None);

            Console.WriteLine("Message length: " +  message.Length + "| Bytes sent: " + bytesSent);

            //var text = "this is a message";
            //var payload = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
            //var message = new byte[payload.Length + 1];
            //message[0] = Convert.ToByte(payload.Length);
            //Array.Copy(payload, 0, message, 1, payload.Length);

            //var a = this.sender.Send(message, 0, message.Length, SocketFlags.None);
            //Console.WriteLine("Message sent: " + text);
        }

        private void SendMessage(IMessage message)
        {
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
