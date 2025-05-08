using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp2.Command;
using WpfApp2.Models;
using WpfApp2.Tools;
using WpfApp2.UserControls;

namespace WpfApp2.ViewModels
{
    public class MainWindowVM:BaseViewModel
    {
		public MainWindowVM()
		{
			//初始化
			Init();
		}

		/// <summary>
		/// 初始化
		/// </summary>
		private void Init()
		{
			//初始化串口通讯设置
			SerialPort1 = new SerialPortSettingViewModel();
			SerialPort2 = new SerialPortSettingViewModel_2();

			//初始化UC
			TestUC = new TestViewUC();
			TestUC.DataContext = this;
			SetViewUC = new SetProtolViewUC();
			


            Items = new ObservableCollection<TestItem>
            {
                new TestItem { Id = 1, Name = "项目 1", IsImportant = true },
                new TestItem { Id = 2, Name = "项目 2", IsImportant = false },
                new TestItem { Id = 3, Name = "项目 3", IsImportant = true }
            };

            LogEntries = new ObservableCollection<LogEntry>
            {
                new LogEntry{ Message = "电仪测试完成",Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                new LogEntry{ Message = "电仪测试完成",Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                new LogEntry{ Message = "电仪测试完成",Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                new LogEntry{ Message = "电仪测试完成",Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                new LogEntry{ Message = "电仪测试完成",Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                new LogEntry{ Message = "电仪测试完成",Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                new LogEntry{ Message = "电仪测试完成",Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
            };
        }

        
        public ObservableCollection<TestItem> Items { get; set; }

        #region 日志
        /// <summary>
        /// 添加日志
        /// </summary>
        private void AddLog(string log)
        {
            var logEntry = new LogEntry
            {
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = log
            };
            LogEntries.Add(logEntry);
        }

        //日志项
        public ObservableCollection<LogEntry> LogEntries { get; set; }

		#endregion

		#region 主体界面

		SetProtolViewUC SetViewUC;
		TestViewUC TestUC;

        private UserControl _ContentControl;

		public UserControl ContentControl
		{
			get {
				if(_ContentControl == null)
				{
					_ContentControl = SetViewUC;
				}
				return _ContentControl; }
			set
			{
				_ContentControl = value;
				this.RaiseProperChanged(nameof(ContentControl));
			}
		}

        public ICommand SwitchUC
        {
            get
			{
				return new RelayCommand(SwitchContentUC);
			} 
		}

		private bool UC_Tga = false;
		private void SwitchContentUC()
		{
			if (UC_Tga)
			{
				ContentControl = SetViewUC;
				UC_Tga = false;
			}
			else
			{
				ContentControl = TestUC;
				UC_Tga = true;
			}
		}

        #endregion

        #region 串口工具

        //串口通讯设置一
        private SerialPortSettingViewModel _serialPort1;

		public SerialPortSettingViewModel SerialPort1	
		{
			get { return _serialPort1; }
			set
			{
                _serialPort1 = value;
				this.RaiseProperChanged(nameof(SerialPort1));
			}
		}

		//串口通讯设置二
		private SerialPortSettingViewModel_2 _serialPort2;

		public SerialPortSettingViewModel_2 SerialPort2
		{
			get { return _serialPort2; }
			set
			{
				_serialPort2 = value;
				this.RaiseProperChanged(nameof(SerialPort2));
			}
		}


		#endregion

	}

}
