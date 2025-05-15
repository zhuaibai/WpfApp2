using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using WpfApp2.Models;

namespace WpfApp2.ViewModels
{
    public class MachineTypeVM:BaseViewModel
    {
        //选择的机型
        public MachineType machineType;
        //保存地址
        private string path;

		
		//机器类型
		public string Machine
		{
			get { return machineType.MachineName; }
			set
			{
				machineType.MachineName = value;
				this.RaiseProperChanged(nameof(Machine));
			}
		}

		
		//选择的安时
		public string A_Hour
		{
			get { return machineType.A_Hour; }
			set
			{
				machineType.A_Hour = value;
				this.RaiseProperChanged(nameof(A_Hour));
			}
		}

		//选择的电池电压
		public string BatVol
		{
			get { return machineType.BatVol; }
			set
			{
				machineType.BatVol = value;
				this.RaiseProperChanged(nameof(BatVol));
			}
		}

		//可选的安时
        public ObservableCollection<string> AvailableA_Hours { get; }
		//可选的电池电压
        public ObservableCollection<string> AvailableBatVols { get; }

        //构造函数
        public MachineTypeVM(string targetPath)
        {
            path = targetPath;
            machineType = LoadMachine();
            AvailableA_Hours = new ObservableCollection<string> { "100", "150", "300" };
            AvailableBatVols = new ObservableCollection<string> { "24", "48", "220" };
        }


		//从配置文件中加载机器类型
		public MachineType LoadMachine()
		{
            try
            {
                if (File.Exists(path))
                {
                    var serializer = new XmlSerializer(typeof(MachineType));
                    using (var reader = new StreamReader(path))
                    {
                        return (MachineType)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载设置时出错: {ex.Message}");
                return new MachineType();
            }
            return new MachineType();
        }

        //保存机器类型到配置文件
        public void SaveSelectedMachine()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(MachineType));
                using (var writer = new StreamWriter(path))
                {
                    serializer.Serialize(writer, machineType);
                }
                Console.WriteLine("设置已保存");
                // MessageBox.Show("设置已保存,请重新打开串口生效!\r\nThe settings have been saved, please re-open the serial port to take effect!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存设置时出错: {ex.Message}");
                MessageBox.Show($"保存设置时出错:{ex.Message}");
            }
        }

    }
}
