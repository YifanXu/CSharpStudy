using System;
using System.Net;
using System.Net.Sockets;

namespace Lesson002_02_DataTransfer
{
    public class Chat : IDisposable
    {
        private Socket server;
        private Socket socket;

        public Chat(string host, int port)
        {
            this.socket = string.IsNullOrEmpty(host) ? CreateHost(port, out server) : CreateClient(host, port, out server);
        }

        public void Dispose()
        {
            if (this.socket != null)
            {
                this.socket.Dispose();
                this.socket = null;
            }

            if (this.server != null)
            {
                this.server.Dispose();
                this.server = null;
            }
        }

        public int Send(byte[] data)
        {
            return this.socket.Send(data);
        }

        public int Available
        {
            get
            {
                return this.socket.Available;
            }
        }

        public int Receive(byte[] buffer)
        {
            return this.socket.Receive(buffer);
        }

        private static Socket CreateClient(string host, int port, out Socket server)
        {
            server = null;
            var addressList = Dns.GetHostAddresses(host);
            if ((addressList == null) || (addressList.Length < 1))
            {
                throw new ArgumentException(string.Format("Bad host name: {0}", host));
            }

            var endpoint = new IPEndPoint(addressList[0], port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endpoint);
            Console.WriteLine("Connected");
            return socket;
        }

        private static Socket CreateHost(int port, out Socket server)
        {
            var localEndPoint = new IPEndPoint(IPAddress.Any, port);
            server = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);        
            server.Bind(localEndPoint);
            server.Listen(1);
            Console.WriteLine("Waiting for a connection...");
            // Program is suspended while waiting for an incoming connection.
            Socket socket = server.Accept();
            Console.WriteLine("Connected");
            return socket;
        }
    }
}

