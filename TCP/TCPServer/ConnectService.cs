using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPServer
{
    class ConnectService
    {
        private Socket clientSocket = null;

        public ConnectService(Socket socket)
        {
            clientSocket = socket;
        }

        public void Process()
        {
            byte[] buffer = new byte[512 * 1024];
            string content;

            string ipAddr = ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString();
            Console.WriteLine("a new connect from:{0}", ipAddr);

            try
            {
                int received = 0;
                while ((received = clientSocket.Receive(buffer)) != 0)
                {
                    content = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine("received:{0}", content);

                    content = string.Format("server echo:{0}", content);
                    buffer = Encoding.UTF8.GetBytes(content);
                    clientSocket.Send(buffer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            clientSocket.Close();
            Console.WriteLine("connect from {0} closed.", ipAddr);
        }
    }
}
