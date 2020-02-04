using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebSockets
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private static Socket ConnectSocket(string server, int port)
        {
            Socket socket = null;
            IPHostEntry hostEntry = null;

            hostEntry = Dns.GetHostEntry(server);

            foreach(IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint iPEndPoint = new IPEndPoint(address, port);
                Socket tmpSocket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                tmpSocket.Connect(iPEndPoint);

                if (tmpSocket.Connected)
                {
                    socket = tmpSocket;
                    break;
                }
            }

            return socket;
        }

        private static string SocketSendReceive(string server, int port)
        {
            string request = "GET / HTTP/1.1\r\nHost: " + server +
            "\r\nConnection: Close\r\n\r\n";

            Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
            Byte[] bytesReceived = new Byte[256];
            string page = "";

            using(Socket socket = ConnectSocket(server, port))
            {
                if(socket == null)
                {
                    return ("Connection failed");
                }

                socket.Send(bytesSent, bytesSent.Length, 0);

                int bytes = 0;
                page = "Default HTML page on " + server + ":\r\n";

                do
                {
                    bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
                    page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                }
                while (bytes > 0);
            }

            return page;
        }
    }
}
