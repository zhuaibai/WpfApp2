using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Tools
{
    public class CommunicateTool
    {
        // 拼接多个字节数组的方法
        public static byte[] ConcatByteArrays(params byte[][] arrays)
        {
            // 计算所有数组的总长度
            int totalLength = 0;
            foreach (var array in arrays)
            {
                if (array == null)
                    continue; // 跳过null数组
                totalLength += array.Length;
            }

            // 创建结果数组
            byte[] result = new byte[totalLength];
            int currentPosition = 0;

            // 依次将每个数组复制到结果数组中
            foreach (var array in arrays)
            {
                if (array == null)
                    continue; // 跳过null数组

                Buffer.BlockCopy(array, 0, result, currentPosition, array.Length);
                currentPosition += array.Length;
            }

            return result;
        }

        // 把无符号数字转换为两个字节（大端在后）
        public static byte[] NumberToBytes(ushort number)
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(number & 0xFF);       // 低字节（大端在后）
            bytes[1] = (byte)((number >> 8) & 0xFF); // 高字节（大端在后）
            return bytes;
        }

        // 把两个字节（大端在后）转换回无符号数字
        public static ushort BytesToNumber(byte[] bytes)
        {
            if (bytes.Length != 2)
            {
                throw new ArgumentException("字节数组的长度必须为2。");
            }
            return (ushort)((bytes[1] << 8) | bytes[0]); // 高字节左移8位后与低字节相或
        }

        
    }
}
