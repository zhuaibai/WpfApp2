using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.ViewModels;

namespace WpfApp2.Models
{
    public class TestData:BaseViewModel
    {

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

        // 首次标准充电电流
        private ushort _ChargeCom1Current;
        public ushort ChargeCom1Current
        {
            get => _ChargeCom1Current;
            set
            {
                if (_ChargeCom1Current == value) return;
                _ChargeCom1Current = value;
                OnPropertyChanged();
            }
        }

        // 结果标准充电电流
        private ushort _ChargeCom1CurrentResult;
        public ushort ChargeCom1CurrentResult
        {
            get => _ChargeCom1CurrentResult;
            set
            {
                if (_ChargeCom1CurrentResult == value) return;
                _ChargeCom1CurrentResult = value;
                OnPropertyChanged();
            }
        }

        //首次BMS板充电电流
        private ushort _ChargeCom2Current;

        public ushort ChargeCom2Current
        {
            get { return _ChargeCom2Current; }
            set
            {
                _ChargeCom2Current = value;
                this.RaiseProperChanged(nameof(ChargeCom2Current));
            }
        }

        //结果BMS板充电电流
        private ushort _ChargeCom2CurrentResult;

        public ushort ChargeCom2CurrentResult
        {
            get { return _ChargeCom2CurrentResult; }
            set
            {
                _ChargeCom2CurrentResult = value;
                this.RaiseProperChanged(nameof(ChargeCom2CurrentResult));
            }
        }

        //原先充电校准系数
        private ushort _ChargeAdjustNum;

        public ushort ChargeAdjustNum
        {
            get { return _ChargeAdjustNum; }
            set
            {
                _ChargeAdjustNum = value;
                this.RaiseProperChanged(nameof(ChargeAdjustNum));
            }
        }

        //写入充电校准系数
        private ushort _WriteChargeAdjustNum;

        public ushort WriteChargeAdjustNum
        {
            get { return _WriteChargeAdjustNum; }
            set
            {
                _WriteChargeAdjustNum = value;
                this.RaiseProperChanged(nameof(WriteChargeAdjustNum));
            }
        }

        // 首次标准放电电流
        private ushort _DisChargeCom1Current;
        public ushort DisChargeCom1Current
        {
            get => _DisChargeCom1Current;
            set
            {
                if (_DisChargeCom1Current == value) return;
                _DisChargeCom1Current = value;
                OnPropertyChanged();
            }
        }
        // 结果标准放电电流
        private ushort _DisChargeCom1CurrentResult;
        public ushort DisChargeCom1CurrentResult
        {
            get => _DisChargeCom1CurrentResult;
            set
            {
                if (_DisChargeCom1CurrentResult == value) return;
                _DisChargeCom1CurrentResult = value;
                OnPropertyChanged();
            }
        }

        //首次BMS板放电电流
        private ushort _DisChargeCom2Current;

        public ushort DisChargeCom2Current
        {
            get { return _DisChargeCom2Current; }
            set
            {
                _DisChargeCom2Current = value;
                this.RaiseProperChanged(nameof(DisChargeCom2Current));
            }
        }

        //结果BMS板放电电流
        private ushort _DisChargeCom2CurrentResult;

        public ushort DisChargeCom2CurrentResult
        {
            get { return _DisChargeCom2CurrentResult; }
            set
            {
                _DisChargeCom2CurrentResult = value;
                this.RaiseProperChanged(nameof(DisChargeCom2CurrentResult));
            }
        }

        //原先放电校准系数
        private ushort _DisChargeAdjustNum;

        public ushort DisChargeAdjustNum
        {
            get { return _DisChargeAdjustNum; }
            set
            {
                _DisChargeAdjustNum = value;
                this.RaiseProperChanged(nameof(DisChargeAdjustNum));
            }
        }

        //写入放电校准系数
        private ushort _WriteDisChargeAdjustNum;

        public ushort WriteDisChargeAdjustNum
        {
            get { return _WriteDisChargeAdjustNum; }
            set
            {
                _WriteDisChargeAdjustNum = value;
                this.RaiseProperChanged(nameof(WriteDisChargeAdjustNum));
            }
        }

        //限流前Dc2电流
        private ushort limitDc2Source;

        public ushort LimitDc2Source
        {
            get { return limitDc2Source; }
            set
            {
                limitDc2Source = value;
                this.RaiseProperChanged(nameof(LimitDc2Source));
            }
        }

        //限流后Dc2电流
        private ushort limitDc2SourceResult;

        public ushort LimitDc2SourceResult
        {
            get { return limitDc2SourceResult; }
            set
            {
                limitDc2SourceResult = value;
                this.RaiseProperChanged(nameof(LimitDc2SourceResult));
            }
        }

        //限流前bms电流
        private ushort limitBmsSource;

        public ushort LimitBmsSource
        {
            get { return limitBmsSource; }
            set
            {
                limitBmsSource = value;
                this.RaiseProperChanged(nameof(LimitBmsSource));
            }
        }

        //限流后bms电流
        private ushort limitBmsSourceResult;

        public ushort LimitBmsSourceResult
        {
            get { return limitBmsSourceResult; }
            set
            {
                limitBmsSourceResult = value;
                this.RaiseProperChanged(nameof(LimitBmsSourceResult));
            }
        }

    }
}
