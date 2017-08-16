using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPServer
{
    class Program
    {
        const int port = 9527;
        const int backlog = 20;

        static void Main(string[] args)
        {
            // create the socket
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // bind the listening socket to the port
            IPAddress hostIP = IPAddress.Any;
            IPEndPoint ep = new IPEndPoint(hostIP, port);
            serverSocket.Bind(ep);

            // start listening
            serverSocket.Listen(backlog);
            Console.WriteLine("listen on port:{0}", port);

            while (true)
            {
                // sync accept a connect
                Socket clientSocket = serverSocket.Accept();

                // create a thread to process the connect
                ConnectService connectService = new ConnectService(clientSocket);
                Thread thread = new Thread(new ThreadStart(connectService.Process));
                thread.Start();
            }
        }
    }
}
