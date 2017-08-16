using System;
using System.Collections.Generic;
using System.Text;

namespace TCPServer
{
    /// <summary>
    /// 报文解析工具类
    /// </summary>
    class ExplainUtils
    {
            /// <summary>
            /// 带空格16进制字符串转换为字节数组
            /// </summary>
            /// <param name="hexSpaceString">带空格的十六进制字符串</param>
            /// <returns></returns>
            public static byte[] HexSpaceStringToByteArray(string hexSpaceString)
            {
                hexSpaceString = hexSpaceString.Replace(" ", string.Empty);
                if (hexSpaceString.Length % 2 != 0)
                {
                    hexSpaceString += " ";
                }
                byte[] array = new byte[hexSpaceString.Length / 2];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = Convert.ToByte(hexSpaceString.Substring(i * 2, 2), 16);
                }
                return array;
            }
    }
}
