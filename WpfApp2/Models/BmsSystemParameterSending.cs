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

        // DC源1参数
        public ushort DcSource1Switch { get; set; }        // 直流源1开关
        public ushort DcSource1Voltage { get; set; }       // 直流源1电压
        public ushort DcSource1Current { get; set; }       // 直流源1电流

        // DC源2参数
        public ushort DcSource2Switch { get; set; }        // 直流源2开关
        public ushort DcSource2Voltage { get; set; }       // 直流源2电压
        public ushort DcSource2Current { get; set; }       // 直流源2电流

        // 电子负载参数
        public ushort ElectronicLoadMode { get; set; }     // 电子负载模式
        public ushort ElectronicLoadCurrent { get; set; }  // 电子负载电流

        // 预留参数
        public ushort Reserved1 { get; set; }              // 预留1
        public ushort Reserved2 { get; set; }              // 预留2
        public ushort Reserved3 { get; set; }              // 预留3

        // 设备状态参数
        public ushort ResistorBankMosfetStatus { get; set; }   // 电阻帮MOS状态
        public ushort ChargeRelayStatus { get; set; }          // 充电继电器状态
        public ushort DischargeRelayStatus { get; set; }       // 放电继电器状态
        public ushort ChargeCurrentLimitNegativeRelayStatus { get; set; }  // 充电限流负极继电器状态
        public ushort LowPowerRelayStatus { get; set; }        // 低功耗继电器状态
        public ushort Reserved1RelayStatus { get; set; }       // 预留1继电器状态
        public ushort Reserved2RelayStatus { get; set; }       // 预留2继电器状态
        public ushort Reserved3RelayStatus { get; set; }       // 预留3继电器状态

        //// MOS管控制参数
        //public ushort ChargeDischargeMosfet1Control { get; set; } // 充放电MOS管1控制
        //public ushort ReservedMosfetControl1 { get; set; }        // 预留MOS管控制
        //public ushort ReservedMosfetControl2 { get; set; }        // 预留MOS管控制
        //public ushort ChargeDischargeMosfet2Control { get; set; } // 充放电MOS管2控制

        //// 继电器控制参数
        //public ushort ChargeDischargeRelay1Control { get; set; } // 充放电继电器1控制
        //public ushort ChargeDischargeRelay2Control { get; set; } // 充放电继电器2控制
        //public ushort ChargeDischargeRelay3Control { get; set; } // 充放电继电器3控制
        //public ushort ChargeDischargeRelay4Control { get; set; } // 充放电继电器4控制
        //public ushort ChargeDischargeRelay5Control { get; set; } // 充放电继电器5控制

        ////低功耗继电器控制参数
        //public ushort LowerRelay1Control { get; set; }  // 低功耗继电器1控制
        //public ushort Relay2Control { get; set; }       // 预留继电器2控制
        //public ushort Relay3Control { get; set; }       // 预留电继电器3控制
        //public ushort Relay4Control { get; set; }       // 预留电继电器4控制

        // 构造函数
        public BmsSystemParametersSending()
        {
            // 默认初始化为0
        }

        // 转换为大端在后的字节数组
        public byte[] ToByteArray()
        {
            byte[] result = new byte[68]; // 34个ushort，每个2字节
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

            WriteUShort(DcSource1Switch, result, offset); offset += 2;
            WriteUShort(DcSource1Voltage, result, offset); offset += 2;
            WriteUShort(DcSource1Current, result, offset); offset += 2;

            WriteUShort(DcSource2Switch, result, offset); offset += 2;
            WriteUShort(DcSource2Voltage, result, offset); offset += 2;
            WriteUShort(DcSource2Current, result, offset); offset += 2;

            WriteUShort(ElectronicLoadMode, result, offset); offset += 2;
            WriteUShort(ElectronicLoadCurrent, result, offset); offset += 2;

            WriteUShort(Reserved1, result, offset); offset += 2;
            WriteUShort(Reserved2, result, offset); offset += 2;
            WriteUShort(Reserved3, result, offset); offset += 2;

            WriteUShort(ResistorBankMosfetStatus, result, offset); offset += 2;
            WriteUShort(ChargeRelayStatus, result, offset); offset += 2;
            WriteUShort(DischargeRelayStatus, result, offset); offset += 2;
            WriteUShort(ChargeCurrentLimitNegativeRelayStatus, result, offset); offset += 2;
            WriteUShort(LowPowerRelayStatus, result, offset); offset += 2;
            WriteUShort(Reserved1RelayStatus, result, offset); offset += 2;
            WriteUShort(Reserved2RelayStatus, result, offset); offset += 2;
            WriteUShort(Reserved3RelayStatus, result, offset); offset += 2;

            return result;
        }

        // 从大端在后的字节数组创建对象
        public static BmsSystemParametersSending FromByteArray(byte[] data)
        {
            if (data == null || data.Length < 68)
                throw new ArgumentException("数据长度不足68字节");

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

            result.DcSource1Switch = ReadUShort(data, offset); offset += 2;
            result.DcSource1Voltage = ReadUShort(data, offset); offset += 2;
            result.DcSource1Current = ReadUShort(data, offset); offset += 2;

            result.DcSource2Switch = ReadUShort(data, offset); offset += 2;
            result.DcSource2Voltage = ReadUShort(data, offset); offset += 2;
            result.DcSource2Current = ReadUShort(data, offset); offset += 2;

            result.ElectronicLoadMode = ReadUShort(data, offset); offset += 2;
            result.ElectronicLoadCurrent = ReadUShort(data, offset); offset += 2;

            result.Reserved1 = ReadUShort(data, offset); offset += 2;
            result.Reserved2 = ReadUShort(data, offset); offset += 2;
            result.Reserved3 = ReadUShort(data, offset); offset += 2;

            result.ResistorBankMosfetStatus = ReadUShort(data, offset); offset += 2;
            result.ChargeRelayStatus = ReadUShort(data, offset); offset += 2;
            result.DischargeRelayStatus = ReadUShort(data, offset); offset += 2;
            result.ChargeCurrentLimitNegativeRelayStatus = ReadUShort(data, offset); offset += 2;
            result.LowPowerRelayStatus = ReadUShort(data, offset); offset += 2;
            result.Reserved1RelayStatus = ReadUShort(data, offset); offset += 2;
            result.Reserved2RelayStatus = ReadUShort(data, offset); offset += 2;
            result.Reserved3RelayStatus = ReadUShort(data, offset); offset += 2;
            return result;
        }

        // 辅助方法：以大端在后方式写入ushort（低字节在前）
        private void WriteUShort(ushort value, byte[] buffer, int offset)
        {
            buffer[offset] = (byte)(value & 0xFF);       // 低字节在前
            buffer[offset + 1] = (byte)((value >> 8) & 0xFF); // 高字节在后
        }

        /// <summary>
        /// 把所有充放电继电器置0
        /// </summary>
        public void ReSetChargeDischargeRelayControlToZero()
        {
            //ChargeDischargeRelay1Control = 0;
            //ChargeDischargeRelay2Control = 0;
            //ChargeDischargeRelay3Control = 0;
            //ChargeDischargeRelay4Control = 0;
            //ChargeDischargeRelay5Control = 0;       
        }
        
        /// <summary>
        /// 把两个MOS管置0
        /// </summary>
        public void ReSetChargeDischargeMosfetControl()
        {
            //ChargeDischargeMosfet1Control = 0;
            //ChargeDischargeMosfet2Control = 0;
        }
        // 辅助方法：以大端在后方式读取ushort（低字节在前）
        private static ushort ReadUShort(byte[] buffer, int offset)
        {
            return (ushort)(buffer[offset] | (buffer[offset + 1] << 8));
        }
    }
}
