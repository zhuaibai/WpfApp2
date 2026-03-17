using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Tools
{
    public class CommunicateTool
    {
        /// <summary>
        /// 拼接多个字节数组的方法
        /// </summary>
        /// <param name="arrays"></param>
        /// <returns></returns>
        public static byte[] ConcatByteArrays(params byte[][] arrays)
        {
            // 计算所有数组的总长度
            int totalLength = 0;
            // 遍历传入的每个字节数组，累加它们的长度
            foreach (var array in arrays)
            {
                if (array == null)
                    continue; // 跳过null数组
                totalLength += array.Length;
            }

            
            // 创建一个新的字节数组，大小等于所有数组长度之和
            byte[] result = new byte[totalLength];
            int currentPosition = 0; // 记录当前已复制到的位置

            // 依次将每个数组复制到结果数组中
            foreach (var array in arrays)
            {
                if (array == null)
                    continue; // 跳过null数组

                // Buffer.BlockCopy 是高效的字节复制方法
                // 参数说明：源数组, 源起始位置, 目标数组, 目标起始位置, 要复制的字节数
                Buffer.BlockCopy(array, 0, result, currentPosition, array.Length);
                // 更新当前位置指针，指向下一个要写入的位置
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


        /// <summary>
        /// 判断两个值的差值是否小于1
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool AreWithinFive(int a, int b)
        {
            // 计算两个数的差值的绝对值
            int difference = Math.Abs(a - b);

            // 判断差值是否小于5
            return difference <= 2;
        }

        /// <summary>
        /// 将short转换为两个字节    
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ShortToBytes(short value)
        {
            // 方法1：使用BitConverter（平台相关的字节序）        
            return BitConverter.GetBytes(value);
            // 方法2：手动转换（小端字节序）        
            // byte[] bytes = new byte[2];        
            // bytes[0] = (byte)(value & 0xFF);        
            // bytes[1] = (byte)((value >> 8) & 0xFF);        
            // return bytes;
        }

        /// <summary>
        /// 将两个字节转换为short    
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static short BytesToShort(byte[] bytes, int startIndex = 0)
        {
            // 确保字节数组长度足够        
            if (bytes == null || bytes.Length < startIndex + 2)
            {
                return 0;
                throw new ArgumentException("字节数组长度不足");
            }
            // 方法1：使用BitConverter（平台相关的字节序）        
            //return BitConverter.ToInt16(bytes, startIndex);
            // 方法2：手动转换（小端字节序）        
            return (short)((bytes[startIndex + 1] << 8) | bytes[startIndex]);
        }

    }
}
