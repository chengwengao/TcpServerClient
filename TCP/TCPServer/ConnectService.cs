using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

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
            //TODO:消息体最长2^10，此处buffer长度待商榷
            byte[] buffer = new byte[1024];
            string content = "";
            string ipAddr = ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString();
            Console.WriteLine("a new connect from:{0}", ipAddr);

            try
            {
                int received = 0;
                while ((received = clientSocket.Receive(buffer)) != 0)
                {
                    
                    content = ExplainUtils.convertStrMsg(buffer);
                    //校验消息是否正确
                    if (!ExplainUtils.msgValid(ExplainUtils.strToToHexByte(content))) break;
                    Console.WriteLine("received:{0}", content);
                    byte[] bytes = ExplainUtils.HexSpaceStringToByteArray(content);
                    int msgBodyProps = ExplainUtils.ParseIntFromBytes(bytes, 2 + 1, 2);
                    string terminalPhone = (ExplainUtils.ParseBcdStringFromBytes(bytes, 4 + 1, 6));
                    int flowId = ExplainUtils.ParseIntFromBytes(bytes, 10 + 1, 2);
                    //客户端消息应答
                    clientSocket.Send(ExplainUtils.rtnRespMsg( msgBodyProps, terminalPhone,  flowId));
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
