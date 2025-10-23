using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Models;

namespace WpfApp2.Tools
{
    public class AnalyseSendCommand
    {
        /// <summary>
        /// 自动生成发送包方法【8位帧头 + 一位数据长度(89) + 数据 + CRC校验(数据长度+数据)】
        /// </summary>
        /// <param name="HeadBytes">帧头字符串</param>
        /// <param name="sendingCommands">发送的数据</param>
        /// <returns>需要发送的一帧</returns>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] GetSendBytes(string HeadBytes, ObservableCollection<SendingCommand> sendingCommands)
        {
            using var ms = new MemoryStream();//进行校验位的缓存
            using var msCRC = new MemoryStream();//最终报文缓存

            //一、 帧头
            byte[] bytes = new byte[]{0x01,0x10,0x00,0x82,0x00,0x5b,0xb6};
           // msCRC.Write(bytes);

            //二、 数据长度  固定182
            ms.Write(bytes);


            //三、 数据
            foreach (var sendingCommand in sendingCommands)
            {
                // 尝试解析字符串为整数
                if (!int.TryParse(sendingCommand.ReturnCount, out int value))
                {
                    throw new ArgumentException($"无法将 '{sendingCommand.AnalysisMethod}' 解析为整数。", nameof(sendingCommand.ReturnCount));
                }


                // 根据数值大小转换为1或2字节
                if (sendingCommand.Enable == "1")
                {
                    ms.WriteByte((byte)value); // 1字节表示
                    msCRC.WriteByte((byte)value);
                }
                else
                {

                    // 验证数值范围
                    if (value < 0)
                    {
                        value = 65536 + value;
                    }
                    
                    ms.WriteByte((byte)(value & 0xFF)); // 低字节
                    ms.WriteByte((byte)(value >> 8));  // 高字节
                    msCRC.WriteByte((byte)(value & 0xFF)); // 低字节
                    msCRC.WriteByte((byte)(value >> 8));  // 高字节
                }
            }

            //四、CRC校验

            ////先补全数据位(因为
            //for (int i = (int)ms.Position; i < 90; i++)
            //{
            //    ms.WriteByte(0xFF);
            //}
            //计算CRC校验值
            byte[] crc = SerialCommunicationService2.getCRC16(ms.ToArray());

            //byte[] crct = SerialCommunicationService2.getCRC(ms.ToArray());
            ms.Write(crc);

            //msCRC.Write(ms.ToArray());

            byte[] data = ms.ToArray();
            return data;
        }
    }
}
