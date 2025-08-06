using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.ViewModels;

namespace WpfApp2.Models
{
    public class BmsSystemparametersReceive:BaseViewModel
    {
        // 系统状态参数

        // 通讯版本
        private ushort _CommunicationVersion;
        public ushort CommunicationVersion {
            get
            {
                return _CommunicationVersion;
            }
            set
            {
                if (_CommunicationVersion == value) return;
                _CommunicationVersion = value;
                OnPropertyChanged();
            } }

        // 测试模式
        private ushort _TestMode;
        public ushort TestMode
        {
            get => _TestMode;
            set
            {
                if (_TestMode == value) return;
                _TestMode = value;
                OnPropertyChanged();
            }
        }          
        
        // 复位开关状态
        private ushort _ResetSwitchStatus;
        public ushort ResetSwitchStatus
        {
            get => _ResetSwitchStatus;
            set
            {
                if (_ResetSwitchStatus == value) return;
                _ResetSwitchStatus = value;
                OnPropertyChanged();
            }
        }

        // 拨码开关值
        private ushort _DIPSwitchValue;
        public ushort DIPSwitchValue
        {
            get => _DIPSwitchValue;
            set
            {
                if (_DIPSwitchValue == value) return;
                _DIPSwitchValue = value;
                OnPropertyChanged();
            }
        }

        
        // Bms232通讯
        private ushort _Bms232Communication;
        public ushort Bms232Communication
        {
            get => _Bms232Communication;
            set
            {
                if (_Bms232Communication == value) return;
                _Bms232Communication = value;
                OnPropertyChanged();
            }
        }

        // Can通讯
        private ushort _CanCommunication;
        public ushort CanCommunication
        {
            get => _CanCommunication;
            set
            {
                if (_CanCommunication == value) return;
                _CanCommunication = value;
                OnPropertyChanged();
            }
        }

        // Bms并机通讯
        private ushort _BmsParallelCommunication;
        public ushort BmsParallelCommunication
        {
            get => _BmsParallelCommunication;
            set
            {
                if (_BmsParallelCommunication == value) return;
                _BmsParallelCommunication = value;
                OnPropertyChanged();
            }
        }

        // Bms逆变器通讯
        private ushort _BmsInverterCommunication;
        public ushort BmsInverterCommunication
        {
            get => _BmsInverterCommunication;
            set
            {
                if (_BmsInverterCommunication == value) return;
                _BmsInverterCommunication = value;
                OnPropertyChanged();
            }
        }

        // LED状态参数
        // 干节点1状态
        private ushort _Relay1Status;
        public ushort Relay1Status
        {
            get => _Relay1Status;
            set
            {
                if (_Relay1Status == value) return;
                _Relay1Status = value;
                OnPropertyChanged();
            }
        }

        // 干节点2状态
        private ushort _Relay2Status;
        public ushort Relay2Status
        {
            get => _Relay2Status;
            set
            {
                if (_Relay2Status == value) return;
                _Relay2Status = value;
                OnPropertyChanged();
            }
        }

        // 测试成功LED灯状态
        private ushort _TestSuccessLedStatus;
        public ushort TestSuccessLedStatus
        {
            get => _TestSuccessLedStatus;
            set
            {
                if (_TestSuccessLedStatus == value) return;
                _TestSuccessLedStatus = value;
                OnPropertyChanged();
            }
        }

        // 测试失败LED灯状态
        private ushort _TestFailureLedStatus;
        public ushort TestFailureLedStatus
        {
            get => _TestFailureLedStatus;
            set
            {
                if (_TestFailureLedStatus == value) return;
                _TestFailureLedStatus = value;
                OnPropertyChanged();
            }
        }

        // DC源参数
        // DC源开关
        private ushort _DcSourceSwitch;
        public ushort DcSourceSwitch
        {
            get => _DcSourceSwitch;
            set
            {
                if (_DcSourceSwitch == value) return;
                _DcSourceSwitch = value;
                OnPropertyChanged();
            }
        }

        // DC源电压
        private ushort _DcSourceVoltage;
        public ushort DcSourceVoltage
        {
            get => _DcSourceVoltage;
            set
            {
                if (_DcSourceVoltage == value) return;
                _DcSourceVoltage = value;
                OnPropertyChanged();
            }
        }

        // DC源电流
        private ushort _DcSourceCurrent;
        public ushort DcSourceCurrent
        {
            get => _DcSourceCurrent;
            set
            {
                if (_DcSourceCurrent == value) return;
                _DcSourceCurrent = value;
                OnPropertyChanged();
            }
        }

        // DC源1参数
        // 直流源1开关
        private ushort _DcSource1Switch;
        public ushort DcSource1Switch
        {
            get => _DcSource1Switch;
            set
            {
                if (_DcSource1Switch == value) return;
                _DcSource1Switch = value;
                OnPropertyChanged();
            }
        }
        // 直流源1电压
        private ushort _DcSource1Voltage;
        public ushort DcSource1Voltage
        {
            get => _DcSource1Voltage;
            set
            {
                if (_DcSource1Voltage == value) return;
                _DcSource1Voltage = value;
                OnPropertyChanged();
            }
        }
        // 直流源1电流
       
        private ushort _DcSource1Current;
        public ushort DcSource1Current
        {
            get => _DcSource1Current;
            set
            {
                if (_DcSource1Current == value) return;
                _DcSource1Current = value;
                OnPropertyChanged();
            }
        }

        // DC源2参数
        // 直流源2开关
        private ushort _DcSource2Switch;
        public ushort DcSource2Switch
        {
            get => _DcSource2Switch;
            set
            {
                if (_DcSource2Switch == value) return;
                _DcSource2Switch = value;
                OnPropertyChanged();
            }
        }
        // 直流源2电压
        private ushort _DcSource2Voltage;
        public ushort DcSource2Voltage
        {
            get => _DcSource2Voltage;
            set
            {
                if (_DcSource2Voltage == value) return;
                _DcSource2Voltage = value;
                OnPropertyChanged();
            }
        }
        // 直流源2电流
        private ushort _DcSource2Current;
        public ushort DcSource2Current
        {
            get => _DcSource2Current;
            set
            {
                if (_DcSource2Current == value) return;
                _DcSource2Current = value;
                OnPropertyChanged();
            }
        }

        // 低功耗参数
        // 低功耗电压
        private ushort _LowPowerVoltage;
        public ushort LowPowerVoltage
        {
            get => _LowPowerVoltage;
            set
            {
                if (_LowPowerVoltage == value) return;
                _LowPowerVoltage = value;
                OnPropertyChanged();
            }
        }

        // 低功耗电流
        private ushort _LowPowerCurrent;
        public ushort LowPowerCurrent
        {
            get => _LowPowerCurrent;
            set
            {
                if (_LowPowerCurrent == value) return;
                _LowPowerCurrent = value;
                OnPropertyChanged();
            }
        }

        // 电子负载参数
        // 电子负载模式
        private ushort _ElectronicLoadMode;
        public ushort ElectronicLoadMode
        {
            get => _ElectronicLoadMode;
            set
            {
                if (_ElectronicLoadMode == value) return;
                _ElectronicLoadMode = value;
                OnPropertyChanged();
            }
        }
        // 电子负载电流
        private ushort _ElectronicLoadCurrent;
        public ushort ElectronicLoadCurrent
        {
            get => _ElectronicLoadCurrent;
            set
            {
                if (_ElectronicLoadCurrent == value) return;
                _ElectronicLoadCurrent = value;
                OnPropertyChanged();
            }
        }

        // 预留参数
        // 预留1(电子负载输出状态)
        private ushort _Reserved1;
        public ushort Reserved1
        {
            get => _Reserved1;
            set
            {
                if (_Reserved1 == value) return;
                _Reserved1 = value;
                OnPropertyChanged();
            }
        }
        // 预留2
        private ushort _Reserved2;
        public ushort Reserved2
        {
            get => _Reserved2;
            set
            {
                if (_Reserved2 == value) return;
                _Reserved2 = value;
                OnPropertyChanged();
            }
        }
        // 预留3
        private ushort _Reserved3;
        public ushort Reserved3
        {
            get => _Reserved3;
            set
            {
                if (_Reserved3 == value) return;
                _Reserved3 = value;
                OnPropertyChanged();
            }
        }

        // 设备状态参数
        // 电阻棒MOS状态
        private ushort _ResistorBankMosfetStatus;
        public ushort ResistorBankMosfetStatus
        {
            get => _ResistorBankMosfetStatus;
            set
            {
                if (_ResistorBankMosfetStatus == value) return;
                _ResistorBankMosfetStatus = value;
                OnPropertyChanged();
            }
        }
        // 充电继电器状态
        private ushort _ChargeRelayStatus;
        public ushort ChargeRelayStatus
        {
            get => _ChargeRelayStatus;
            set
            {
                if (_ChargeRelayStatus == value) return;
                _ChargeRelayStatus = value;
                OnPropertyChanged();
            }
        }
        // 放电继电器状态
        private ushort _DischargeRelayStatus;
        public ushort DischargeRelayStatus
        {
            get => _DischargeRelayStatus;
            set
            {
                if (_DischargeRelayStatus == value) return;
                _DischargeRelayStatus = value;
                OnPropertyChanged();
            }
        }
        // 充电限流负极继电器状态
        private ushort _ChargeCurrentLimitNegativeRelayStatus;
        public ushort ChargeCurrentLimitNegativeRelayStatus
        {
            get => _ChargeCurrentLimitNegativeRelayStatus;
            set
            {
                if (_ChargeCurrentLimitNegativeRelayStatus == value) return;
                _ChargeCurrentLimitNegativeRelayStatus = value;
                OnPropertyChanged();
            }
        }
        // 低功耗继电器状态
        private ushort _LowPowerRelayStatus;
        public ushort LowPowerRelayStatus
        {
            get => _LowPowerRelayStatus;
            set
            {
                if (_LowPowerRelayStatus == value) return;
                _LowPowerRelayStatus = value;
                OnPropertyChanged();
            }
        }
        // 预留1继电器状态
        private ushort _Reserved1RelayStatus;
        public ushort Reserved1RelayStatus
        {
            get => _Reserved1RelayStatus;
            set
            {
                if (_Reserved1RelayStatus == value) return;
                _Reserved1RelayStatus = value;
                OnPropertyChanged();
            }
        }

        // 预留2继电器状态
        private ushort _Reserved2RelayStatus;
        public ushort Reserved2RelayStatus
        {
            get => _Reserved2RelayStatus;
            set
            {
                if (_Reserved2RelayStatus == value) return;
                _Reserved2RelayStatus = value;
                OnPropertyChanged();
            }
        }
        // 预留3继电器状态
        private ushort _Reserved3RelayStatus;
        public ushort Reserved3RelayStatus
        {
            get => _Reserved3RelayStatus;
            set
            {
                if (_Reserved3RelayStatus == value) return;
                _Reserved3RelayStatus = value;
                OnPropertyChanged();
            }
        }


        //下边是舍弃方案
        //// 低功耗参数
        //public ushort LowPowerVoltage { get; set; }          // 低功耗电压
        //public ushort LowPowerCurrent { get; set; }          // 低功耗电流

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
        //public ushort Relay2Control { get; set; }       // 预留继电器1控制
        //public ushort Relay3Control { get; set; }       // 预留电继电器2控制
        //public ushort Relay4Control { get; set; }       // 预留电继电器3控制

        // 构造函数
        public BmsSystemparametersReceive()
        {
            // 默认初始化为0
        }

        // 转换为大端在后的字节数组
        public byte[] ToByteArray()
        {
            byte[] result = new byte[72]; // 36个ushort，每个2字节
            int offset = 0;

            WriteUShort(CommunicationVersion, result, offset); offset += 2;
            WriteUShort(TestMode, result, offset); offset += 2;
            WriteUShort(ResetSwitchStatus, result, offset); offset += 2;
            WriteUShort(DIPSwitchValue, result, offset); offset += 2;

            WriteUShort(Bms232Communication, result, offset); offset += 2;
            WriteUShort(CanCommunication, result, offset); offset += 2;
            WriteUShort(BmsParallelCommunication, result, offset); offset += 2;
            WriteUShort(BmsInverterCommunication, result, offset); offset += 2;

            WriteUShort(Relay1Status, result, offset); offset += 2;
            WriteUShort(Relay2Status, result, offset); offset += 2;
            WriteUShort(TestSuccessLedStatus, result, offset); offset += 2;
            WriteUShort(TestFailureLedStatus, result, offset); offset += 2;

            WriteUShort(DcSourceSwitch, result, offset); offset += 2;
            WriteUShort(DcSourceVoltage, result, offset); offset += 2;
            WriteUShort(DcSourceCurrent, result, offset); offset += 2;

            WriteUShort(DcSource1Switch, result, offset); offset += 2;
            WriteUShort(DcSource1Voltage, result, offset); offset += 2;
            WriteUShort(DcSource1Current, result, offset); offset += 2;

            WriteUShort(DcSource2Switch, result, offset); offset += 2;
            WriteUShort(DcSource2Voltage, result, offset); offset += 2;
            WriteUShort(DcSource2Current, result, offset); offset += 2;

            WriteUShort(LowPowerVoltage, result, offset); offset += 2;
            WriteUShort(LowPowerCurrent, result, offset); offset += 2;

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
        public static BmsSystemparametersReceive FromByteArray(byte[] data)
        {
            if (data == null || data.Length < 72)
                throw new ArgumentException("数据长度不足72字节");

            BmsSystemparametersReceive result = new BmsSystemparametersReceive();
            int offset = 0;

            result.CommunicationVersion = ReadUShort(data, offset); offset += 2;
            result.TestMode = ReadUShort(data, offset); offset += 2;
            result.ResetSwitchStatus = ReadUShort(data, offset); offset += 2;
            result.DIPSwitchValue = ReadUShort(data, offset); offset += 2;

            result.Bms232Communication = ReadUShort(data, offset); offset += 2;
            result.CanCommunication = ReadUShort(data, offset); offset += 2;
            result.BmsParallelCommunication = ReadUShort(data, offset); offset += 2;
            result.BmsInverterCommunication = ReadUShort(data, offset); offset += 2;

            result.Relay1Status = ReadUShort(data, offset); offset += 2;
            result.Relay2Status = ReadUShort(data, offset); offset += 2;
            result.TestSuccessLedStatus = ReadUShort(data, offset); offset += 2;
            result.TestFailureLedStatus = ReadUShort(data, offset); offset += 2;

            result.DcSourceSwitch = ReadUShort(data, offset); offset += 2;
            result.DcSourceVoltage = ReadUShort(data, offset); offset += 2;
            result.DcSourceCurrent = ReadUShort(data, offset); offset += 2;

            result.DcSource1Switch = ReadUShort(data, offset); offset += 2;
            result.DcSource1Voltage = ReadUShort(data, offset); offset += 2;
            result.DcSource1Current = ReadUShort(data, offset); offset += 2;

            result.DcSource2Switch = ReadUShort(data, offset); offset += 2;
            result.DcSource2Voltage = ReadUShort(data, offset); offset += 2;
            result.DcSource2Current = ReadUShort(data, offset); offset += 2;

            result.LowPowerVoltage = ReadUShort(data, offset); offset += 2;
            result.LowPowerCurrent = ReadUShort(data, offset); offset += 2;

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

        // 辅助方法：以大端在后方式读取ushort（低字节在前）
        private static ushort ReadUShort(byte[] buffer, int offset)
        {
            return (ushort)(buffer[offset] | (buffer[offset + 1] << 8));
        }


        public ushort GetValueByName(string propertyName)
        {
            //switch (name)
            //{
            //    case "CommunicationVersion":
            //        return CommunicationVersion;
            //    case "TestMode":
            //        return TestMode;
            //    case "ResetSwitchStatus":
            //        return ResetSwitchStatus;
            //    case "DIPSwitchValue":
            //        return DIPSwitchValue;

            //    case "Bms232Communication":
            //        return Bms232Communication;
            //    case "CanCommunication":
            //        return CanCommunication;
            //    case "BmsParallelCommunication":
            //        return BmsParallelCommunication;
            //    case "BmsInverterCommunication":
            //        return BmsInverterCommunication;
            //}
            // 获取当前对象的类型信息
            Type type = this.GetType();

            // 查找属性（忽略大小写）
            PropertyInfo property = type.GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (property == null)
            {
                throw new ArgumentException($"属性 '{propertyName}' 不存在");
            }

            // 获取属性值并转换为ushort
            object value = property.GetValue(this);
            if (value is ushort ushortValue)
            {
                return ushortValue;
            }

            throw new InvalidOperationException($"属性 '{propertyName}' 不是ushort类型");
        }
    }
}
