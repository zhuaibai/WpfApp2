using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
            Items = new List<Item>
            {
                new Item { Id = 1, Name = "项目 1", IsImportant = true },
                new Item { Id = 2, Name = "项目 2", IsImportant = false },
                new Item { Id = 3, Name = "项目 3", IsImportant = true }
            };

           
        }
        public List<Item> Items { get; set; }

        #region 主体界面
        private UserControl _ContentControl;

		public UserControl ContentControl
		{
			get {
				if(_ContentControl == null)
				{
					_ContentControl = new TestViewUC();
				}
				return _ContentControl; }
			set
			{
				_ContentControl = value;
				this.RaiseProperChanged(nameof(ContentControl));
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

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsImportant { get; set; }
    }
}
