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
            byte[] buffer = new byte[1 * 1024];
            string content = "";
            string ipAddr = ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString();
            Console.WriteLine("a new connect from:{0}", ipAddr);

            try
            {
                int received = 0;
                while ((received = clientSocket.Receive(buffer)) != 0)
                {

                    content += BitConverter.ToString(buffer).Replace("-", " ");

                    //content = Encoding.UTF8.GetString(buffer, 0, received);

                    //content = string.Format("server echo:{0}", content);
                    //buffer = Encoding.UTF8.GetBytes(content);
                    //clientSocket.Send(buffer);
                    Console.WriteLine("received:{0}", content);
                    byte[] bytes = ExplainUtils.HexSpaceStringToByteArray(content);
                    string terminalPhone = (this.ParseBcdStringFromBytes(bytes, 4 + 1, 6));
                    Console.WriteLine(terminalPhone);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            clientSocket.Close();
            Console.WriteLine("connect from {0} closed.", ipAddr);
        }

        private String ParseBcdStringFromBytes(byte[] data, int startIndex, int lenth)
        {
            return this.ParseBcdStringFromBytes(data, startIndex, lenth, null);
        }

        private String ParseBcdStringFromBytes(byte[] data, int startIndex, int lenth, String defaultVal)
        {
            try
            {
                byte[] tmp = new byte[lenth];
                Buffer.BlockCopy(data, startIndex, tmp, 0, lenth);
                return this.bcd2String(tmp);
            }
            catch (Exception e)
            {
                //log.Error(string.Format("解析BCD(8421码)出错:{0}", e.Message));
                //e.printStackTrace();
                return defaultVal;
            }
        }

        public String bcd2String(byte[] bytes)
        {
            StringBuilder temp = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                // 高四位
                temp.Append((bytes[i] & 0xf0) >> 4);
                // 低四位
                temp.Append(bytes[i] & 0x0f);
            }
            return temp.ToString().Substring(0, 1) == ("0") ? temp.ToString().Substring(1) : temp.ToString();
        }
    }
}
