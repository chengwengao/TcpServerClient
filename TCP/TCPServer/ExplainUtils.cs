using System;
using System.Collections.Generic;
using System.IO;
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

        /**
         * 把byte[]转化位整形,通常为指令用
         * 
         * @param value
         * @return
         * @throws Exception
         */
        public static int byteToInteger(byte[] value)
        {
            int result;
            if (value.Length == 1)
            {
                result = oneByteToInteger(value[0]);
            }
            else if (value.Length == 2)
            {
                result = twoBytesToInteger(value);
            }
            else if (value.Length == 3)
            {
                result = threeBytesToInteger(value);
            }
            else if (value.Length == 4)
            {
                result = fourBytesToInteger(value);
            }
            else
            {
                result = fourBytesToInteger(value);
            }
            return result;
        }
        /**
         * 把一个byte转化位整形,通常为指令用
         * 
         * @param value
         * @return
         * @throws Exception
         */
        public static int oneByteToInteger(byte value)
        {
            return (int)value & 0xFF;
        }

        /**
         * 把一个2位的数组转化位整形
         * 
         * @param value
         * @return
         * @throws Exception
         */
        public static int twoBytesToInteger(byte[] value)
        {
            // if (value.length < 2) {
            // throw new Exception("Byte array too short!");
            // }
            int temp0 = value[0] & 0xFF;
            int temp1 = value[1] & 0xFF;
            return ((temp0 << 8) + temp1);
        }

        /**
         * 把一个3位的数组转化位整形
         * 
         * @param value
         * @return
         * @throws Exception
         */
        public static int threeBytesToInteger(byte[] value)
        {
            int temp0 = value[0] & 0xFF;
            int temp1 = value[1] & 0xFF;
            int temp2 = value[2] & 0xFF;
            return ((temp0 << 16) + (temp1 << 8) + temp2);
        }

        /**
         * 把一个4位的数组转化位整形,通常为指令用
         * 
         * @param value
         * @return
         * @throws Exception
         */
        public static int fourBytesToInteger(byte[] value)
        {
            // if (value.length < 4) {
            // throw new Exception("Byte array too short!");
            // }
            int temp0 = value[0] & 0xFF;
            int temp1 = value[1] & 0xFF;
            int temp2 = value[2] & 0xFF;
            int temp3 = value[3] & 0xFF;
            return ((temp0 << 24) + (temp1 << 16) + (temp2 << 8) + temp3);
        }

        /**
         * 把一个4位的数组转化位整形
         * 
         * @param value
         * @return
         * @throws Exception
         */
        public long fourBytesToLong(byte[] value)
        {
            // if (value.length < 4) {
            // throw new Exception("Byte array too short!");
            // }
            int temp0 = value[0] & 0xFF;
            int temp1 = value[1] & 0xFF;
            int temp2 = value[2] & 0xFF;
            int temp3 = value[3] & 0xFF;
            return (((long)temp0 << 24) + (temp1 << 16) + (temp2 << 8) + temp3);
        }

        /**
         * 把一个整形该为1位的byte数组
         * 
         * @param value
         * @return
         * @throws Exception
         */
        public static byte[] integerTo1Bytes(int value)
        {
            byte[] result = new byte[1];
            result[0] = (byte)(value & 0xFF);
            return result;
        }

        /**
         * 把一个整形改为2位的byte数组
         * 
         * @param value
         * @return
         * @throws Exception
         */
        public static byte[] integerTo2Bytes(int value)
        {
            byte[] result = new byte[2];
            result[0] = (byte)((value >> 8) & 0xFF);
            result[1] = (byte)(value & 0xFF);
            return result;
        }

        /**
         * 字符串==>BCD字节数组
         * 
         * @param str
         * @return BCD字节数组
         */
        public static byte[] string2Bcd(string str)
        {
            // 奇数,前补零
            if ((str.Length & 0x1) == 1)
            {
                str = "0" + str;
            }

            byte[] ret = new byte[str.Length / 2];
            byte[] bs = Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < ret.Length; i++)
            {

                byte high = ascII2Bcd(bs[2 * i]);
                byte low = ascII2Bcd(bs[2 * i + 1]);

                // TODO 只遮罩BCD低四位?
                ret[i] = (byte)((high << 4) | low);
            }
            return ret;
        }

        public static int getCheckSum4JT808(byte[] bs, int start, int end)
        {
            //if (start < 0 || end > bs.length)
            //    throw new ArrayIndexOutOfBoundsException("getCheckSum4JT808 error : index out of bounds(start=" + start
            //            + ",end=" + end + ",bytes length=" + bs.length + ")");
            int cs = 0;
            for (int i = start; i < end; i++)
            {
                cs ^= bs[i];
            }
            return cs;
        }

        /**
         * 
         * 发送消息时转义<br>
         * 
         * <pre>
         *  0x7e <====> 0x7d02
         * </pre>
         * 
         * @param bs
         *            要转义的字节数组
         * @param start
         *            起始索引
         * @param end
         *            结束索引
         * @return 转义后的字节数组
         * @throws Exception
         */
        public static byte[] DoEscape4Send(byte[] bs, int start, int end)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                for (int i = 0; i < start; i++)
                {
                    ms.WriteByte(bs[i]);
                }
                for (int i = start; i < end; i++)
                {
                    if (bs[i] == 0x7e)
                    {
                        ms.WriteByte(0x7d);
                        ms.WriteByte(0x02);
                    }
                    else
                    {
                        ms.WriteByte(bs[i]);
                    }
                }
                for (int i = end; i < bs.Length; i++)
                {
                    ms.WriteByte(bs[i]);
                }
                return ms.ToArray();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static byte ascII2Bcd(byte asc)
        {
            if ((asc >= '0') && (asc <= '9'))
                return (byte)(asc - '0');
            else if ((asc >= 'A') && (asc <= 'F'))
                return (byte)(asc - 'A' + 10);
            else if ((asc >= 'a') && (asc <= 'f'))
                return (byte)(asc - 'a' + 10);
            else
                return (byte)(asc - 48);
        }
    }
}
