using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Tools
{
    public class ByteConverter
    {
        /// <summary>
        /// 把无符号数字转换为两个字节（大端在后）
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static byte[] NumberToBytes(ushort number)
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(number & 0xFF);       // 低字节（大端在后）
            bytes[1] = (byte)((number >> 8) & 0xFF); // 高字节（大端在后）
            return bytes;
        }

        /// <summary>
        /// 把两个字节（大端在后）转换回无符号数字
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ushort BytesToNumber(byte[] bytes)
        {
            if (bytes.Length != 2)
            {
                throw new ArgumentException("字节数组的长度必须为2。");
            }
            return (ushort)((bytes[1] << 8) | bytes[0]); // 高字节左移8位后与低字节相或
        }

        /// <summary>
        /// 把无符号数字转换为两个字节（大端在前）
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static byte[] NumberToBytesNormal(ushort number)
        {
            byte[] bytes = new byte[2];
            bytes[1] = (byte)(number & 0xFF);       // 低字节（大端在前）
            bytes[0] = (byte)((number >> 8) & 0xFF); // 高字节（大端在前）
            return bytes;
        }

        // 把两个字节（大端在前）转换回无符号数字
        public static ushort BytesToNumberNormal(byte[] bytes)
        {
            if (bytes.Length != 2)
            {
                
                throw new ArgumentException("字节数组的长度必须为2。");
                
            }
            return (ushort)((bytes[0] << 8) | bytes[1]); // 高字节左移8位后与低字节相或
        }

        /// <summary>
        /// 将日期数字（如20250714）转换为字节数组
        /// </summary>
        /// <param name="dateNumber">日期数字，格式为yyyyMMdd</param>
        /// <returns>转换后的4字节数组</returns>
        public static byte[] DateNumberToBytes(long dateNumber)
        {
            // 验证日期数字范围（确保是8位数字）
            if (dateNumber < 10000000 || dateNumber > 99999999)
            {
                dateNumber = 20250101;
            }

            // 转换为32位整数
            uint value = (uint)dateNumber;

            // 获取32位整数的字节表示（小端）
            byte[] bytes = BitConverter.GetBytes(value);

            // 交换高低字节：第一和第二字节交换，第三和第四字节交换
            // 原始字节顺序：[b0, b1, b2, b3]（小端）
            // 目标字节顺序：[b1, b0, b3, b2]
            return new byte[] { bytes[1], bytes[0], bytes[3], bytes[2] };
        }

        /// <summary>
        /// 将字节数组转换回日期数字（如20250714）
        /// </summary>
        /// <param name="bytes">4字节数组</param>
        /// <returns>转换后的日期数字</returns>
        public static long BytesToDateNumber(byte[] bytes)
        {
            if (bytes == null || bytes.Length != 4)
            {
                throw new ArgumentException("字节数组必须是4字节长度", nameof(bytes));
            }

            // 还原字节顺序：将[b1, b0, b3, b2]转换回[b0, b1, b2, b3]
            byte[] originalBytes = new byte[] { bytes[1], bytes[0], bytes[3], bytes[2] };

            // 转换为32位整数
            uint value = BitConverter.ToUInt32(originalBytes, 0);

            // 转换为长整数返回
            return (long)value;
        }

    }
}
