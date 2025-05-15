using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WpfApp2.Command;
using WpfApp2.Models;
using WpfApp2.Models.Service;
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

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
		{
           
            //数据库初始化
            SQLiteHelper.ConnectionString = "Data Source=MyDatabase.db;Version=3;";
			InitTestItems();			

            //初始化串口通讯设置
            SerialPort1 = new SerialPortSettingViewModel();
			SerialPort2 = new SerialPortSettingViewModel_2();

			//初始化UC
			TestUC = new TestViewUC();
			TestUC.DataContext = this;
			SetViewUC = new SetProtolViewUC();

            //初始化命令
            StartCommand = new RelayCommand(StartBackgroundThread);
            StopCommand = new RelayCommand(StopBackgroundThread);

            LogEntries = new ObservableCollection<LogEntry>();

            //添加自动滚动功能
            TestUC.SetupScrolling();
            TestUC.SetupScrolling2();
        }
        #endregion

        #region 机器类型选择

        //地址
        static string MachineCfgPath = "MachineTypes/";
        private bool _isOption1Selected =true;
        private bool _isOption2Selected;
        private bool _isOption3Selected;
        private bool _isOption4Selected;
        //选择的机型
        private string SelectedValue="选项1";

        public bool IsOption1Selected
        {
            get => _isOption1Selected;
            set
            {
                _isOption1Selected = value;
                OnPropertyChanged(nameof(IsOption1Selected));
                if (value) // 当选项1被选中时
                {
                    // 处理选择逻辑
                    SelectedValue = "选项1";
                }
            }
        }
        public bool IsOption2Selected
        {
            get => _isOption2Selected;
            set
            {
                _isOption2Selected = value;
                OnPropertyChanged(nameof(IsOption2Selected));
                if (value) // 当选项2被选中时
                {
                    // 处理选择逻辑
                    SelectedValue = "选项2";
                }
            }
        }
        public bool IsOption3Selected
        {
            get => _isOption3Selected;
            set
            {
                _isOption3Selected = value;
                OnPropertyChanged(nameof(IsOption3Selected));
                if (value) // 当选项3被选中时
                {
                    // 处理选择逻辑
                    SelectedValue = "选项3";
                }
            }
        } 
        public bool IsOption4Selected
        {
            get => _isOption4Selected;
            set
            {
                _isOption4Selected = value;
                OnPropertyChanged(nameof(IsOption4Selected));
                if (value) // 当选项4被选中时
                {
                    // 处理选择逻辑
                    SelectedValue = "选项4";
                }
            }
        }

       

        //测试机器
        private MachineType _testMachine;
        public MachineType testMachine
        { get
            {
                return _testMachine;
            }
            set
            {
                _testMachine = value;
                OnPropertyChanged();
            }           
        }

        //机器类型一
        public MachineTypeVM machineTypeVM_1 { get; set; } = new MachineTypeVM(MachineCfgPath + "machine1.xml");
        //机器类型二
        public MachineTypeVM machineTypeVM_2 { get; set; } = new MachineTypeVM(MachineCfgPath+ "machine2.xml");
        //机器类型三
        public MachineTypeVM machineTypeVM_3 { get; set; } = new MachineTypeVM(MachineCfgPath+ "machine3.xml");
        //机器类型四
        public MachineTypeVM machineTypeVM_4 { get; set; } = new MachineTypeVM(MachineCfgPath+"machine4.xml");

        public ICommand SelectedMachine { get; set; }

        private void SelectMachine(object parameter)
        {
            string param = parameter as string;
            switch (parameter)
            {
                case "选项1":
                    testMachine = new MachineType
                    {
                        MachineName = machineTypeVM_1.Machine,
                        A_Hour = machineTypeVM_1.A_Hour,
                        BatVol = machineTypeVM_1.BatVol
                    };
                    machineTypeVM_1.SaveSelectedMachine();
                    break;
                case "选项2":
                    testMachine = new MachineType
                    {
                        MachineName = machineTypeVM_2.Machine,
                        A_Hour = machineTypeVM_2.A_Hour,
                        BatVol = machineTypeVM_2.BatVol
                    };
                    machineTypeVM_2.SaveSelectedMachine();
                    break;
                case "选项3":
                    testMachine = new MachineType
                    {
                        MachineName = machineTypeVM_3.Machine,
                        A_Hour = machineTypeVM_3.A_Hour,
                        BatVol = machineTypeVM_3.BatVol
                    };
                    machineTypeVM_3.SaveSelectedMachine();
                    break;
                case "选项4":
                    testMachine = new MachineType
                    {
                        MachineName = machineTypeVM_4.Machine,
                        A_Hour = machineTypeVM_4.A_Hour,
                        BatVol = machineTypeVM_4.BatVol
                    };
                    machineTypeVM_4.SaveSelectedMachine();
                    break;
                default:
                    testMachine = new MachineType
                    {
                        MachineName = "没选机型",
                        A_Hour = machineTypeVM_2.A_Hour,
                        BatVol = machineTypeVM_2.BatVol
                    };
                    break;
            }
        }

        #endregion

        #region 项目显示
        public ObservableCollection<TestItem> TestItems { get; set; }

        private  TestItemService _service;

		/// <summary>
		/// 初始化项目显示
		/// </summary>
		private void InitTestItems()
		{
            _service = new TestItemService();
            TestItems = new ObservableCollection<TestItem>();

            // 初始化数据库表
            _service.CreateTable();

            // 加载数据
            LoadTestItems();
        }

		/// <summary>
		/// 从数据库获取最新数据
		/// </summary>
        private void LoadTestItems()
        {
            TestItems.Clear();
            var items = _service.GetAllTestItems();
            foreach (var item in items)
            {
                TestItems.Add(item);
            }
        }
		
		/// <summary>
		/// 添加项目到数据库
		/// </summary>
		/// <param name="item"></param>
		private void AddTestItem(TestItem item)
		{
			if (item != null)
			{
				_service.AddTestItem(item);
			}
			LoadTestItems();
		}

        #endregion

        #region 日志

        //日志项
        public ObservableCollection<LogEntry> LogEntries { get; set; }

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

            Application.Current.Dispatcher.Invoke(() =>
            {

                LogEntries.Add(logEntry);
                if (LogEntries.Count > 100) LogEntries.RemoveAt(0);
            });
           SaveLogToFile($"【{logEntry.Time}】  {log}");
        }

       
        /// <summary>
        /// 保存一条日志到本地
        /// </summary>
        /// <param name="log"></param>
        private void SaveLogToFile(string log)
        {
            DateTime now = DateTime.Now;
            string logName = "日志";
            string yearMonth = now.ToString("yyyy-MM");
            string DayFolder = now.ToString("dd");

            // 构建日志文件夹路径
            string logFolder = Path.Combine(Directory.GetCurrentDirectory(), logName, yearMonth);
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            // 构建日志文件路径
            string logFilePath = Path.Combine(logFolder, $"log_{now:yyyyMM-dd}.txt");
            try
            {
                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(log);
                }
            }
            catch (Exception ex)
            {
                AddLog($"写入日志文件时出错: {ex.Message}");
            }
        }
        

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
                //测试界面跳转配置界面

                //关闭串口
                if (CloseCom())
                {
                    ContentControl = SetViewUC;
                    UC_Tga = false;
                }
                else
                {
                    MessageBox.Show("关闭串口失败");
                }
				
			}
			else
			{
                //配置界面跳转测试界面

                //更新串口信息
                SaveSerialInfo();
                //更新机器类型
                SelectMachine(SelectedValue);
                //打开串口
                if (OpenCom())
                {
                    //打开成功跳转测试界面
                    ContentControl = TestUC;
                    UC_Tga = true;
                }
               
			}
		}

        #endregion

        #region 串口工具



        /// <summary>
        /// 更新串口通讯配置
        /// </summary>
        private void SaveSerialInfo()
        {
            //保存串口通讯一配置
            SerialPort1.SaveSettings(this);
            //保存串口通讯二配置
            SerialPort2.SaveSettings(this);
            //重新加载配置
            SerialPort1.LoadSettings();
            SerialPort2.LoadSettings();

        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        private bool OpenCom()
        {
            //串口一通讯初始化
            SerialCommunicationService.InitiateCom(SerialPort1._settings);
            //窗口二通讯初始化
            SerialCommunicationService2.InitiateCom(SerialPort2._settings);
            try
            {
                bool Com1 = SerialCommunicationService.OpenCom();
                bool Com2 = SerialCommunicationService2.OpenCom();
                return Com1 && Com2;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"串口打开失败，请检查!\r\n{ex.Message}", "打开串口", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        private bool CloseCom()
        {
            return SerialCommunicationService.CloseCom() && SerialCommunicationService2.CloseCom();
        }

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

        #region 后台测试线程

        private CancellationTokenSource _cts = new CancellationTokenSource();//取消线程专用
        private ManualResetEventSlim _pauseEvent = new ManualResetEventSlim(true);//暂停线程专用
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); // 异步竞争	


        // 命令定义
        public RelayCommand StartCommand { get; set; }//启动

        public RelayCommand StopCommand { get; set; }//停止

        // 后台线程是否正在运行
        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 启动后台通信线程
        /// </summary>
        private void StartBackgroundThread()
        {

            //if (!SerialCommunicationService.IsOpen())
            //{
            //    MessageBox.Show(App.GetText("请先打开串口!"), "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            if (IsRunning) { return; }
            else
            {
                IsRunning = true;
                _cts = new CancellationTokenSource();
                _pauseEvent.Set();
                Task.Run(() => BackgroundWorker(_cts.Token));
                AddLog("后台通信线程已启动");
            }

        }

        /// <summary>
        /// 停止后台通信
        /// </summary>
        private void StopBackgroundThread()
        {
            _cts.Cancel();
            AddLog("后台通信停止请求已发送");
        }

        /// <summary>
        /// 后台工作线程主循环
        /// </summary>
        private async Task BackgroundWorker(CancellationToken token)
        {
            int i = -1;
            bool flag = false;
            try
            {
               
                while (!token.IsCancellationRequested)
                {
                   
                    if (i++ < 17)
                    {
                        TestItems[i].IsImportant = flag;
                    }
                    else
                    {
                        i=-1;
                        flag = !flag;
                    }
                    // 模拟常规通信
                    await Task.Delay(1000, token);

                    //AddLog($"[后台] 常规通信: {DateTime.Now:HH:mm:ss.fff}");
                }
            }
            catch (OperationCanceledException)
            {

                IsRunning = false;

            }
            catch (Exception ex)
            {
                IsRunning = false;
            }
            finally
            {
                AddLog("已停止");
            }
           
           
        }

        #endregion
    }

}
