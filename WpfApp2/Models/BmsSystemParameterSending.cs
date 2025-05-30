using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Models
{
    public class BmsSystemParametersSending
    {
        // 系统状态参数
        public ushort CommunicationVersion { get; set; }      // 通讯版本
        public ushort TestMode { get; set; }                 // 测试模式
        public ushort ResetSwitchStatus { get; set; }        // 复位开关状态
        public ushort DIPSwitchStatus { get; set; }          // 拨码开关状态

        // 通讯状态参数
        public ushort Bms232Communication { get; set; }      // Bms232通讯
        public ushort CanCommunication { get; set; }         // Can通讯
        public ushort BmsParallelCommunication { get; set; } // Bms并机通讯
        public ushort BmsInverterCommunication { get; set; } // Bms逆变器通讯

        // LED状态参数
        public ushort Relay1Status { get; set; }             // 干节点1状态
        public ushort Relay2Status { get; set; }             // 干节点2状态
        public ushort TestSuccessLedStatus { get; set; }     // 测试成功LED灯状态
        public ushort TestFailureLedStatus { get; set; }     // 测试失败LED灯状态

        // DC源参数
        public ushort DcSourceSwitch { get; set; }           // DC源开关
        public ushort DcSourceControlVoltage { get; set; }   // DC源控制电压
        public ushort DcSourceControlCurrent { get; set; }   // DC源控制电流

        // MOS管控制参数
        public ushort ChargeDischargeMosfet1Control { get; set; } // 充放电MOS管1控制
        public ushort ReservedMosfetControl1 { get; set; }        // 预留MOS管控制
        public ushort ReservedMosfetControl2 { get; set; }        // 预留MOS管控制
        public ushort ChargeDischargeMosfet2Control { get; set; } // 充放电MOS管2控制

        // 继电器控制参数
        public ushort ChargeDischargeRelay1Control { get; set; } // 充放电继电器1控制
        public ushort ChargeDischargeRelay2Control { get; set; } // 充放电继电器2控制
        public ushort ChargeDischargeRelay3Control { get; set; } // 充放电继电器3控制
        public ushort ChargeDischargeRelay4Control { get; set; } // 充放电继电器4控制
        public ushort ChargeDischargeRelay5Control { get; set; } // 充放电继电器5控制

        //低功耗继电器控制参数
        public ushort LowerRelay1Control { get; set; }  // 低功耗继电器1控制
        public ushort Relay2Control { get; set; }       // 预留继电器2控制
        public ushort Relay3Control { get; set; }       // 预留电继电器3控制
        public ushort Relay4Control { get; set; }       // 预留电继电器4控制

        // 构造函数
        public BmsSystemParametersSending()
        {
            // 默认初始化为0
        }

        // 转换为大端在后的字节数组
        public byte[] ToByteArray()
        {
            byte[] result = new byte[56]; // 28个ushort，每个2字节
            int offset = 0;

            WriteUShort(CommunicationVersion, result, offset); offset += 2;
            WriteUShort(TestMode, result, offset); offset += 2;
            WriteUShort(ResetSwitchStatus, result, offset); offset += 2;
            WriteUShort(DIPSwitchStatus, result, offset); offset += 2;

            WriteUShort(Bms232Communication, result, offset); offset += 2;
            WriteUShort(CanCommunication, result, offset); offset += 2;
            WriteUShort(BmsParallelCommunication, result, offset); offset += 2;
            WriteUShort(BmsInverterCommunication, result, offset); offset += 2;

            WriteUShort(Relay1Status, result, offset); offset += 2;
            WriteUShort(Relay2Status, result, offset); offset += 2;
            WriteUShort(TestSuccessLedStatus, result, offset); offset += 2;
            WriteUShort(TestFailureLedStatus, result, offset); offset += 2;

            WriteUShort(DcSourceSwitch, result, offset); offset += 2;
            WriteUShort(DcSourceControlVoltage, result, offset); offset += 2;
            WriteUShort(DcSourceControlCurrent, result, offset); offset += 2;

            WriteUShort(ChargeDischargeMosfet1Control, result, offset); offset += 2;
            WriteUShort(ReservedMosfetControl1, result, offset); offset += 2;
            WriteUShort(ReservedMosfetControl2, result, offset); offset += 2;
            WriteUShort(ChargeDischargeMosfet2Control, result, offset); offset += 2;

            WriteUShort(ChargeDischargeRelay1Control, result, offset); offset += 2;
            WriteUShort(ChargeDischargeRelay2Control, result, offset); offset += 2;
            WriteUShort(ChargeDischargeRelay3Control, result, offset); offset += 2;
            WriteUShort(ChargeDischargeRelay4Control, result, offset); offset += 2;
            WriteUShort(ChargeDischargeRelay5Control, result, offset); offset += 2;

            WriteUShort(LowerRelay1Control, result, offset); offset += 2;
            WriteUShort(Relay2Control, result, offset); offset += 2;
            WriteUShort(Relay3Control, result, offset); offset += 2;
            WriteUShort(Relay4Control, result, offset); offset += 2;

            return result;
        }

        // 从大端在后的字节数组创建对象
        public static BmsSystemParametersSending FromByteArray(byte[] data)
        {
            if (data == null || data.Length < 56)
                throw new ArgumentException("数据长度不足56字节");

            BmsSystemParametersSending result = new BmsSystemParametersSending();
            int offset = 0;

            result.CommunicationVersion = ReadUShort(data, offset); offset += 2;
            result.TestMode = ReadUShort(data, offset); offset += 2;
            result.ResetSwitchStatus = ReadUShort(data, offset); offset += 2;
            result.DIPSwitchStatus = ReadUShort(data, offset); offset += 2;

            result.Bms232Communication = ReadUShort(data, offset); offset += 2;
            result.CanCommunication = ReadUShort(data, offset); offset += 2;
            result.BmsParallelCommunication = ReadUShort(data, offset); offset += 2;
            result.BmsInverterCommunication = ReadUShort(data, offset); offset += 2;

            result.Relay1Status = ReadUShort(data, offset); offset += 2;
            result.Relay2Status = ReadUShort(data, offset); offset += 2;
            result.TestSuccessLedStatus = ReadUShort(data, offset); offset += 2;
            result.TestFailureLedStatus = ReadUShort(data, offset); offset += 2;

            result.DcSourceSwitch = ReadUShort(data, offset); offset += 2;
            result.DcSourceControlVoltage = ReadUShort(data, offset); offset += 2;
            result.DcSourceControlCurrent = ReadUShort(data, offset); offset += 2;

            result.ChargeDischargeMosfet1Control = ReadUShort(data, offset); offset += 2;
            result.ReservedMosfetControl1 = ReadUShort(data, offset); offset += 2;
            result.ReservedMosfetControl2 = ReadUShort(data, offset); offset += 2;
            result.ChargeDischargeMosfet2Control = ReadUShort(data, offset); offset += 2;

            result.ChargeDischargeRelay1Control = ReadUShort(data, offset); offset += 2;
            result.ChargeDischargeRelay2Control = ReadUShort(data, offset); offset += 2;
            result.ChargeDischargeRelay3Control = ReadUShort(data, offset); offset += 2;
            result.ChargeDischargeRelay4Control = ReadUShort(data, offset); offset += 2;
            result.ChargeDischargeRelay5Control = ReadUShort(data, offset); offset += 2;

            result.LowerRelay1Control = ReadUShort(data, offset); offset += 2;
            result.Relay2Control = ReadUShort(data, offset); offset += 2;
            result.Relay3Control = ReadUShort(data, offset); offset += 2;
            result.Relay4Control = ReadUShort(data, offset); offset += 2;
            return result;
        }

        // 辅助方法：以大端在后方式写入ushort（低字节在前）
        private void WriteUShort(ushort value, byte[] buffer, int offset)
        {
            buffer[offset] = (byte)(value & 0xFF);       // 低字节在前
            buffer[offset + 1] = (byte)((value >> 8) & 0xFF); // 高字节在后
        }

        // 辅助方法：以大端在后方式读取ushort（低字节在前）
        private static ushort ReadUShort(byte[] buffer, int offset)
        {
            return (ushort)(buffer[offset] | (buffer[offset + 1] << 8));
        }
    }
}
