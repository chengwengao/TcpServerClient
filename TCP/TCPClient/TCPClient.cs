using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using TCPServer;
using System.IO;
using System.Threading;

namespace TCPClient
{
    public partial class TCPClient : Form
    {
        string ipAddress = "127.0.0.1";
        int port = 9527;
        Socket clientSocket;
        byte[] buffer = new byte[512 * 1024];

        public TCPClient()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            ipAddressBox.Text = ipAddress;
            portBox.Text = port.ToString();
        }
        void Form_Closing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                ipAddress = ipAddressBox.Text;
                port = int.Parse(portBox.Text);

                IPAddress remoteIP = IPAddress.Parse(ipAddress);
                IPEndPoint remoteEP = new IPEndPoint(remoteIP, port);

                if (clientSocket != null)
                    clientSocket.Close();

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(remoteEP);
                AppendContent(string.Format("connect {0}:{1}", ipAddress, port));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(inputBox.Text))
                {
                    MessageBox.Show("please input content.");
                    return;
                }

                if (clientSocket == null || !clientSocket.Connected)
                {
                    MessageBox.Show("please connect first.");
                    return;
                }

                for (int i=0;i<10;i++)
                {
                    Thread thread = new Thread(new ThreadStart(sendMsg));
                    thread.Start();
                    Thread.Sleep(1500);
                }
                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void test1()
        {
            Console.Write("线程被执行!!!");
        }
        private void AppendContent(string str)
        {
            if (string.IsNullOrEmpty(contentBox.Text))
            {
                contentBox.Text = str;
            }
            else
            {
                contentBox.Text = string.Format("{0}\n{1}", contentBox.Text, str);
            }


        }
        //组装解析的消息
        private void sendMsg()
        {
            //AppendContent("发送消息：");
            //AppendContent(inputBox.Text);
            WriteLog("发送消息："+ inputBox.Text);
            byte[] byte1 = Encoding.UTF8.GetBytes(inputBox.Text);
            string content = ExplainUtils.convertStrMsg(byte1);
            //校验消息是否正确
            if (!ExplainUtils.msgValid(ExplainUtils.strToToHexByte(inputBox.Text))) {
                WriteLog("消息不正确！");
            }else
            {
                byte[] bytes = ExplainUtils.HexSpaceStringToByteArray(inputBox.Text);
                int msgBodyProps = ExplainUtils.ParseIntFromBytes(bytes, 2 + 1, 2);
                string terminalPhone = (ExplainUtils.ParseBcdStringFromBytes(bytes, 4 + 1, 6));
                int flowId = ExplainUtils.ParseIntFromBytes(bytes, 10 + 1, 2);
                //客户端消息应答
                bytes = ExplainUtils.rtnRespMsg(msgBodyProps, terminalPhone, flowId);
                clientSocket.Send(bytes);
                //AppendContent("解码消息：");
                string jiema = ExplainUtils.convertStrMsg(bytes);
                //AppendContent(jiema);
                WriteLog("解码消息：" + jiema);
            }
            inputBox.Text = "";
        }

        /// <summary>
        /// 简单的日志记录
        /// </summary>
        /// <param name="strLog"></param>
        public static void WriteLog(string strLog)
        {
            string sFilePath = "d:\\" + DateTime.Now.ToString("yyyyMM");
            string sFileName = "rizhi" + DateTime.Now.ToString("dd") + ".log";
            sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径
            if (!Directory.Exists(sFilePath))//验证路径是否存在
            {
                Directory.CreateDirectory(sFilePath);
                //不存在则创建
            }
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))
            //验证文件是否存在，有则追加，无则创建
            {
                fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   ---   " + strLog);
            sw.Close();
            fs.Close();
        }
    }
}
