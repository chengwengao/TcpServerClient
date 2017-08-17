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
        //开始和结束标记也可以是两个或两个以上的字节
        private readonly static byte[] BeginMark = new byte[] { 0x7e };
        private readonly static byte[] EndMark = new byte[] { 0x7e };
        public static int pkg_delimiter = 0x7e;// 标识位
        public static int cmd_terminal_register_resp = 0x8100;// 终端注册应答
        public static String string_encoding = "GBK";//字符编码格式
        public static String replyToken = "1234567890Z";//鉴权码

        public ConnectService(Socket socket)
        {
            clientSocket = socket;
        }

        public void Process()
        {
            //TODO:消息体最长2^10，此处buffer长度待商榷
            byte[] buffer = new byte[512];
            string content = "";
            string ipAddr = ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString();
            Console.WriteLine("a new connect from:{0}", ipAddr);

            try
            {
                int received = 0;
                while ((received = clientSocket.Receive(buffer)) != 0)
                {

                    content += BitConverter.ToString(buffer).Replace("-", " ");
                    //TODO:此处截取多余位数待优化
                    int index = content.ToUpper().LastIndexOf("7E");
                    content = content.Substring(0,index+3);
                    Console.WriteLine("received:{0}", content);
                    byte[] bytes = ExplainUtils.HexSpaceStringToByteArray(content);
                    int msgBodyProps = ParseIntFromBytes(bytes, 2 + 1, 2);
                    string terminalPhone = (this.ParseBcdStringFromBytes(bytes, 4 + 1, 6));
                    int flowId = ParseIntFromBytes(bytes, 10 + 1, 2);
                    //客户端消息应答
                    clientSocket.Send(rtnRespMsg( msgBodyProps, terminalPhone,  flowId));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            clientSocket.Close();
            Console.WriteLine("connect from {0} closed.", ipAddr);
        }
        //组装应答消息
        private byte[] rtnRespMsg(int msgBodyProps, string phone, int flowId)
        {
            //7E
            //8100            消息ID
            //0004            消息体属性
            //018512345678    手机号
            //0015            消息流水号
            //0015            应答流水号
            //04              结果(00成功, 01车辆已被注册, 02数据库中无该车辆, 03终端已被注册, 04数据库中无该终端)  无车辆与无终端有什么区别 ?
            //313C             鉴权码
            //7E

            MemoryStream ms = new MemoryStream();
            List<byte> byteSource = new List<byte>();
            // 1. 0x7e
            //ms.Write(ExplainUtils.integerTo1Bytes(pkg_delimiter), 0, 1);
            byte[] bt1 =  ExplainUtils.integerTo1Bytes(pkg_delimiter) ;
            // 2. 消息ID word(16)
            //ms.Write(ExplainUtils.integerTo2Bytes(cmd_terminal_register_resp), 0, 2);
            byte[] bt2 = ExplainUtils.integerTo2Bytes(cmd_terminal_register_resp);

            // 3. 终端手机号 bcd[6]
            //ms.Write(ExplainUtils.string2Bcd(phone), 0, 6);
            byte[] bt3 = ExplainUtils.string2Bcd(phone);
            // 4. 消息流水号 word(16),按发送顺序从 0 开始循环累加
            //ms.Write(ExplainUtils.integerTo2Bytes(flowId), 0, 2);
            byte[] bt4 = ExplainUtils.integerTo2Bytes(flowId);
            // 5. 应答流水号
            //ms.Write(ExplainUtils.integerTo2Bytes(flowId), 0, 2);
            byte[] bt5 = ExplainUtils.integerTo2Bytes(flowId);
            // 6. 成功
            //ms.Write(ExplainUtils.integerTo1Bytes(0), 0, 1);
            byte[] bt6 = ExplainUtils.integerTo1Bytes(0);
            // 7. 鉴权码
            //ms.Write(System.Text.Encoding.GetEncoding(string_encoding).GetBytes(replyToken), 0, replyToken.Length);
            byte[] bt7 = System.Text.Encoding.GetEncoding(string_encoding).GetBytes(replyToken);
            //8.消息体属性
            byte[] bt8 = ExplainUtils.integerTo2Bytes(bt7.Length + 1 + 2);

            byteSource.AddRange(bt1);
            byteSource.AddRange(bt2);
            byteSource.AddRange(bt8);
            byteSource.AddRange(bt3);
            byteSource.AddRange(bt4);
            byteSource.AddRange(bt5);
            byteSource.AddRange(bt6);
            byteSource.AddRange(bt7);
            // 9. BA 校验码
            // 校验码
            int checkSum = ExplainUtils.getCheckSum4JT808(byteSource.ToArray(), 1, (int)(byteSource.Count));
            //ms.Write(ExplainUtils.integerTo1Bytes(checkSum), 0, 1);
            byte[] bt9 = ExplainUtils.integerTo1Bytes(checkSum);
            // 11. 0x7e
            //ms.Write(ExplainUtils.integerTo1Bytes(pkg_delimiter), 0, 1);
            byteSource.AddRange(bt9);
            byteSource.AddRange(bt1);

            // 转义
            return ExplainUtils.DoEscape4Send(byteSource.ToArray(), 1, byteSource.ToArray().Length - 1);
        }

        private int ParseIntFromBytes(byte[] data, int startIndex, int length)
        {
            return this.ParseIntFromBytes(data, startIndex, length, 0);
        }

        private int ParseIntFromBytes(byte[] data, int startIndex, int length, int defaultVal)
        {
            try
            {
                // 字节数大于4,从起始索引开始向后处理4个字节,其余超出部分丢弃
                int len = length > 4 ? 4 : length;
                byte[] tmp = new byte[len];
                Buffer.BlockCopy(data, startIndex, tmp, 0, len);
                return ExplainUtils.byteToInteger(tmp);
            }
            catch (Exception e)
            {
                return defaultVal;
            }
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
