using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.VisualBasic;
using WpfApp2.Command;
using WpfApp2.CustomMessageBox;
using WpfApp2.CustomMessageBox.Service;
using WpfApp2.Models;
using WpfApp2.Models.Service;
using WpfApp2.Tools;
using WpfApp2.UserControls;
using InputType = WpfApp2.CustomMessageBox.InputType;

namespace WpfApp2.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {
        public MainWindowVM(IMessageDialogService messageService)
        {
            //初始化
            Init();
            //消息框初始化
            _messageService = messageService;
            ShowMessageCommand = new RelayCommand(OnShowMessage);
        }

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {

            //数据库路径
            SQLiteHelper.ConnectionString = "Data Source=MyDatabase.db;Version=3;";
            InitTestItems();

            //初始化串口通讯设置
            SerialPort1 = new SerialPortSettingViewModel();
            SerialPort2 = new SerialPortSettingViewModel_2();

            //初始化UC
            TestUC = new TestViewUC();
            TestUC.DataContext = this;
            holeTset = new HoleTsetViewUC() { DataContext=this};
            SetViewUC = new SetProtolViewUC();

            //初始化命令
            StartCommand = new RelayCommand(StartBackgroundThread);
            StopCommand = new RelayCommand(StopBackgroundThread);

            LogEntries = new ObservableCollection<LogEntry>();

            //添加自动滚动功能
            TestUC.SetupScrolling();    //日志自动滚动
            BMS_Receive = new BmsSystemparametersReceive();
            //测试数据
            testData = new TestData();
            //整机测试
            SendingViewModel = new SendingCommandSettingsViewModel() { ShowBoubleWithTime = ShowBubbles };

        }
        #endregion

        #region 机器类型选择

        //地址
        static string MachineCfgPath = "MachineTypes/";
        private bool _isOption1Selected = true;
        private bool _isOption2Selected;
        private bool _isOption3Selected;
        private bool _isOption4Selected;
        //选择的机型
        private string SelectedValue = "选项1";

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

        //测试机器(详情)
        private MachineType _testMachine;
        public MachineType testMachine
        {
            get
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
        public MachineTypeVM machineTypeVM_2 { get; set; } = new MachineTypeVM(MachineCfgPath + "machine2.xml");
        //机器类型三
        public MachineTypeVM machineTypeVM_3 { get; set; } = new MachineTypeVM(MachineCfgPath + "machine3.xml");
        //机器类型四
        public MachineTypeVM machineTypeVM_4 { get; set; } = new MachineTypeVM(MachineCfgPath + "machine4.xml");

        public ICommand SelectedMachine { get; set; }

        /// <summary>
        /// 根据所选机器类型加载对应测试项
        /// </summary>
        /// <param name="parameter"></param>
        private void SelectMachine(object parameter)
        {
            string param = parameter as string;
            switch (parameter)
            {
                case "选项1":

                    //测试机器类型实例化
                    testMachine = new MachineType
                    {
                        MachineName = machineTypeVM_1.Machine,
                        A_Hour = machineTypeVM_1.A_Hour,
                        BatVol = machineTypeVM_1.BatVol
                    };

                    //更新机器类型选项
                    machineTypeVM_1.SaveSelectedMachine();

                    //选择相应的数据表
                    string sql = @"
                    CREATE TABLE IF NOT EXISTS TestItems (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    IsImportant INTEGER NOT NULL
                     )";

                    //从数据库中加载对应测试项
                    InitTestItems("TestItems");

                    break;
                case "选项2":
                    testMachine = new MachineType
                    {
                        MachineName = machineTypeVM_2.Machine,
                        A_Hour = machineTypeVM_2.A_Hour,
                        BatVol = machineTypeVM_2.BatVol
                    };
                    //更新及其类型选项
                    machineTypeVM_2.SaveSelectedMachine();

                    //选择相应的数据表
                    sql = @"
                    CREATE TABLE IF NOT EXISTS TestItems2 (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    IsImportant INTEGER NOT NULL
                     )";

                    //从数据库中加载对应测试项
                    InitTestItems("TestItems2");
                    break;
                case "选项3":
                    testMachine = new MachineType
                    {
                        MachineName = machineTypeVM_3.Machine,
                        A_Hour = machineTypeVM_3.A_Hour,
                        BatVol = machineTypeVM_3.BatVol
                    };
                    //更新及其类型选项
                    machineTypeVM_3.SaveSelectedMachine();

                    //选择相应的数据表
                    sql = @"
                    CREATE TABLE IF NOT EXISTS TestItems3 (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    IsImportant INTEGER NOT NULL
                     )";

                    //从数据库中加载对应测试项
                    InitTestItems("TestItems3");
                    break;
                case "选项4":
                    testMachine = new MachineType
                    {
                        MachineName = machineTypeVM_4.Machine,
                        A_Hour = machineTypeVM_4.A_Hour,
                        BatVol = machineTypeVM_4.BatVol
                    };
                    //更新及其类型选项
                    machineTypeVM_4.SaveSelectedMachine();

                    //选择相应的数据表
                    sql = @"
                    CREATE TABLE IF NOT EXISTS TestItems4 (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    IsImportant INTEGER NOT NULL
                     )";

                    //从数据库中加载对应测试项
                    InitTestItems("TestItems4");
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

        #region 测试项目显示
        public ObservableCollection<TestItem> TestItems { get; set; }

        private TestItemService _service;

        /// <summary>
        /// 加载对应数据库项目项目显示
        /// </summary>
        /// <param name="sql">创建数据库表的指令</param>
        private void InitTestItems(string table)
        {
            //加载新的数据表
            _service = new TestItemService(table);

            // 初始化数据库表
            _service.CreateTable();

            // 加载数据
            LoadTestItems();

            //测试项自动滚动
            TestUC.SetupScrolling2();
        }

        /// <summary>
		/// 初始化项目显示
		/// </summary>
		private void InitTestItems()
        {
            _service = new TestItemService("TestItems");
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
                if (LogEntries.Count > 300) LogEntries.RemoveAt(0);
            });
            SaveLogToFile($"【{logEntry.Time}】  {log}");
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        private void LogClear()
        {
            LogEntries.Clear();
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
        HoleTsetViewUC holeTset;//整机测试

        private UserControl _ContentControl;

        public UserControl ContentControl
        {
            get
            {
                if (_ContentControl == null)
                {
                    _ContentControl = SetViewUC;
                }
                return _ContentControl;
            }
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
                //根据机器类型加载数据库的测试项
                SelectMachine(SelectedValue);
                //打开串口
                if (OpenCom())
                {
                    //输入测试软件版本
                    testData.TestSofterWare = LShowTestMessage("请输入测试的软件版本", "软件版本", "至少4位数字");
                    if (string.IsNullOrEmpty(testData.TestSofterWare))
                    {
                        CloseCom();
                        return;
                    }
                    //打开成功跳转测试界面
                    if (SelectedValue == "选项4")
                    {
                        ContentControl = holeTset;
                    }else
                    ContentControl = TestUC;
                    UC_Tga = true;
                }

            }
        }

        #endregion

        #region 整机测试

        private SendingCommandSettingsViewModel sendingCommandSettingsViewModel;

        public SendingCommandSettingsViewModel SendingViewModel
        {
            get { return sendingCommandSettingsViewModel; }
            set
            {
                sendingCommandSettingsViewModel = value;
                this.RaiseProperChanged(nameof(SendingViewModel));
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
                LShowMessage($"串口打开失败，请检查!\r\n{ex.Message}", "打开串口", MessageIcon.Warning);

                CloseCom();
                return false;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
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

        //全局接收的状态显示
        private BmsSystemparametersReceive _BMS_Receive;

        public BmsSystemparametersReceive BMS_Receive
        {
            get { return _BMS_Receive; }
            set
            {
                _BMS_Receive = value;
                this.RaiseProperChanged(nameof(BMS_Receive));
            }
        }

        #endregion

        #region 后台测试线程

        private CancellationTokenSource _cts = new CancellationTokenSource();//取消线程专用
        private ManualResetEventSlim _pauseEvent = new ManualResetEventSlim(true);//暂停线程专用
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); // 异步竞争	                                 
        private BmsSystemParametersSending parametersSending = new BmsSystemParametersSending() { CommunicationVersion = 1001};
        //
        #region 开始、停止按钮的互相切换
        private Visibility _visibility = Visibility.Visible;

        //开始按钮可视
        public Visibility StartVisible
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibility2 = Visibility.Collapsed;

        //停止按钮可视化
        public Visibility StopVisible
        {
            get
            {
                return _visibility2;
            }
            set
            {
                _visibility2 = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 按钮可视化切换
        /// </summary>
        /// <param name="startShow">开始按钮是否显示</param>

        private void SwitchButtonVisible(bool startShow)
        {
            if (startShow)
            {
                StartVisible = Visibility.Visible;
                StopVisible = Visibility.Collapsed;
            }
            else
            {
                StopVisible = Visibility.Visible;
                StartVisible = Visibility.Collapsed;
            }
        }
        #endregion

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
        //测试数据结果保存
        private TestData _testData;

        public TestData testData
        {
            get { return _testData; }
            set
            {
                _testData = value;
                this.RaiseProperChanged(nameof(testData));
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
                //开始前清除一下日志
                LogClear();

                IsRunning = true;
                _cts = new CancellationTokenSource();
                _pauseEvent.Set();
                Task.Run(() => BackgroundWorker(_cts.Token));
                AddLog("后台通信线程已启动");
                //显示停止按钮
                SwitchButtonVisible(false);
            }
        }

        /// <summary>
        /// 停止后台通信
        /// </summary>
        private void StopBackgroundThread()
        {
            _cts.Cancel();
            AddLog("后台通信停止请求已发送");
            Application.Current.Dispatcher.Invoke(() => ShowBubble("正在停止，请稍等..."));
            //SwitchButtonVisible(true);
        }

        /// <summary>
        /// 后台工作线程主循环
        /// </summary>
        private async Task BackgroundWorker(CancellationToken token)
        {
            // 记录任务开始时间
            DateTime start = TimeTracker.Start();
            //把所有项置为待测试
            ReSetTestItems();
            int i = 0;//计数
            bool flag = false;//测试成功与否
            bool finallySucceess = false;
            parametersSending = new BmsSystemParametersSending() { CommunicationVersion = 1001,  DcSource1Current = 100, DcSource1Switch = 0, DcSource2Current = 100 }; //发送指令实体类初始化
            do
            {
                int passVoltage = 0;
                parametersSending.TestMode = 0;
                if (testMachine.BatVol == "24")
                {
                    parametersSending.DcSource1Voltage = 2500;
                    passVoltage = 250;
                }else if(testMachine.BatVol == "48")
                {
                    parametersSending.DcSource1Voltage = 5000;
                    passVoltage = 500;
                }
                int voltage = 0;
                int successCount = 0;
                do
                {
                    BMS_Receive = SendPacked(parametersSending);
                    if (BMS_Receive == null)
                    {
                        AddLog($"上位机通讯返回异常,请检查串口是否连接正确");
                        LShowMessage("上位机通讯返回异常,请检查串口是否连接正确", "异常", MessageIcon.Error);
                        IsRunning = false;
                        SwitchButtonVisible(true);
                        return;

                    }
                    AddLog($"设置源{parametersSending.DcSource1Voltage} ;源1电压{BMS_Receive.DcSource1Voltage}");
                    Thread.Sleep(1000);
                    BMS_Receive = SendPacked(parametersSending);
                    voltage++;
                    if(Math.Abs(BMS_Receive.DcSource1Voltage - passVoltage) < 50)
                    {
                        successCount++;
                    }
                    parametersSending.DcSource1Switch = 1;
                } while (voltage < 30 && successCount<3);
                if (successCount < 3)
                {
                    AddLog($"源1电压达不到开机要求");
                    LShowMessage("请检查源1是否连接正确", "异常", MessageIcon.Error);
                    IsRunning = false;
                    SwitchButtonVisible(true);
                    return;
                }
                 parametersSending.Reserved1RelayStatus = 1; parametersSending.Reserved2RelayStatus = 1;parametersSending.Reserved3RelayStatus = 1;
                //先开机，确保开机属性
                BMS_Receive = SendPacked(parametersSending);
                Thread.Sleep(1000);
                parametersSending.LowPowerRelayStatus = 1;
                BMS_Receive = SendPacked(parametersSending);
                if (BMS_Receive == null)
                {
                    AddLog($"上位机通讯返回异常,请检查串口是否连接正确");
                    LShowMessage("上位机通讯返回异常,请检查串口是否连接正确", "异常", MessageIcon.Error);
                    IsRunning = false;
                    SwitchButtonVisible(true);
                    return;
                    
                }
                if (SelectedValue == "选项4")
                {
                    flag = true;
                    break;
                }               
                if (BMS_Receive.DcSource1Switch == 1 && BMS_Receive.LowPowerRelayStatus == 1 && BMS_Receive.Reserved1RelayStatus == 1 && BMS_Receive.Reserved2RelayStatus == 1 && BMS_Receive.Reserved3RelayStatus == 1)
                {
                    flag = true;
                }
                else
                {
                    if (i > 3)
                        AddLog("没达到开机测试条件");
                    i++;
                    if (i == 10)
                        break;
                }
                Thread.Sleep(1000);
            } while (!flag);
            //在测试前写入参数
            //flag = WriteBeforeTest();
            //WriteAfterTest();
            //ResetBeforeTest();
            try
            {
                if (!flag)
                {
                    StopBackgroundThread();
                }
                while (!token.IsCancellationRequested)
                {
                    for (i = 0; i < TestItems.Count;)
                    {
                        if (token.IsCancellationRequested)
                            break;
                        //单独把合格设为true，界面显示正在测试颜色
                        TestItems[i].IsImportant = true;
                        //修改为正在测试
                        TestItems[i].Flag = 0;
                        //逐项进行测试
                        flag = TestProgress(TestItems[i].Name);
                        if (token.IsCancellationRequested)
                            break;
                        if (!flag)
                        {
                            AddLog($"{TestItems[i].Name}测试没通过");
                            //修改测试结果(true = 通过, false = 失败)
                            TestItems[i].IsImportant = false;
                            //修改成已测试
                            TestItems[i].Flag = 1;
                            //测试不合格
                            //MessageBoxResult boxResult = MessageBox.Show("是否继续？", "测试暂停", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            MessageResult boxResult = LShowMessage($"{TestItems[i].Name}测试没通过，是否继续？ 【确定】：继续测试本项；【取消】：停止本次测试", "测试暂停", MessageIcon.Question);
                            if (boxResult == MessageResult.Cancel)
                            {
                                StopBackgroundThread();
                                break;
                            }
                        }
                        else
                        {
                            AddLog($"{TestItems[i].Name}测试通过");
                            //修改测试结果(true = 通过, false = 失败)
                            TestItems[i].IsImportant = true;
                            //修改成已测试
                            TestItems[i].Flag = 1;
                            //测试合格，下一项
                            i++;
                        }
                    }
                    if (i == TestItems.Count && flag)
                    {
                        Application.Current.Dispatcher.Invoke(() => ShowBubble("测试通过，正在关闭电子元件..."));
                        AddLog("测试结束，测试结果：通过！");
                        finallySucceess = true;
                    }
                    else
                    {
                        AddLog("测试结束，测试结果：不合格！");
                        finallySucceess = false;
                    }
                    //关闭所有电子元件
                    _cts.Cancel();
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                AddLog(ex.ToString());

                AddLog("异常停止");

            }
            finally
            {
                ExitTestMode();
                SwitchButtonVisible(true);
                IsRunning = false;
                AddLog("已停止");
                // 获取耗时（毫秒）
                //long elapsedMs = TimeTracker.GetElapsedMilliseconds(start);
                //AddLog($"任务耗时: {elapsedMs} 毫秒");
                // 获取耗时（可读格式）
                TimeSpan elapsed = TimeTracker.GetElapsedTime(start);
                //AddLog($"任务耗时: {elapsed.TotalSeconds:F2} 秒");
                //TimeSpan elmin = TimeTracker.GetElapsedTime(start);
                AddLog($"任务耗时: {elapsed.Minutes}分{elapsed.Seconds:00}秒");
                if (finallySucceess)
                    LShowMessage("测试通过！\r\n(可进行更换板子)", "恭喜", MessageIcon.Pass);
                else
                {
                    LShowMessage("测试失败\r\n(可进行板子更换)", "结束", MessageIcon.Error);
                }
            }
        }

        /// <summary>
        /// 测试项目
        /// </summary>
        /// <returns></returns>
        private bool TestProgress(string progressName)
        {
            switch (progressName)
            {
                case "BMS232通讯":
                    AddLog("正在测试BMS232通讯");

                    //进入测试模式1
                    bool interSuccess = false;
                    int ERROR_COUNT = 0;
                    interSuccess = EnterTestMode();
                    BMS_Receive = SendPacked(parametersSending);
                    AddLog($"设置源{parametersSending.DcSource1Voltage} ;源1电压{BMS_Receive.DcSource1Voltage}");
                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            interSuccess = Bms232CommunicationTset();//BMS232通讯
                            Thread.Sleep(1000);
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("BMS232通讯异常");
                            return false;
                        }
                        //写入出厂日期
                        //WriteProductDate(
                        return true;
                    }
                    else
                    {
                        //进入测试模式失败
                        AddLog("进入测试模式异常");
                        return false;
                    }
                case "重置参数":
                    //重置参数
                    BMS_Receive = SendPacked(parametersSending);
                    bool reset = ResetBeforeTest();
                    return reset;
                case "激活状态":
                    BMS_Receive = SendPacked(parametersSending);
                    AddLog($"设置源{parametersSending.DcSource1Voltage} ;源1电压{BMS_Receive.DcSource1Voltage}");
                    //读取是否激活
                    if (!WriteBeforeTest())
                    {
                        LShowMessage("当前BMS板未激活，请先激活", "警告", MessageIcon.Warning);
                        return false;
                    }
                    else
                        return true;
                case "系统时间":
                    //读取系统时间
                    BMS_Receive = SendPacked(parametersSending);
                    AddLog($"设置源{parametersSending.DcSource1Voltage} ;源1电压{BMS_Receive.DcSource1Voltage}");
                    ReadTime();
                    //写入系统时间
                    bool writeSuccess = WriteTimeAfterTest();
                    if (!writeSuccess)
                    {
                        LShowMessage($"写入时间失败:{testData.Time}", "警告", MessageIcon.Warning);
                        return false;
                    }
                    //读取系统时间
                    ReadTime();
                    //写入设计容量100%，满充容量100%，剩余容量30%
                    writeSuccess = WriteSOC();
                    if (!writeSuccess)
                    {
                        LShowMessage($"写入剩余容量30%失败", "警告", MessageIcon.Warning);
                        return false;
                    }
                    return true;
                case "软件版本":
                    BMS_Receive = SendPacked(parametersSending);
                    AddLog($"设置源{parametersSending.DcSource1Voltage} ;源1电压{BMS_Receive.DcSource1Voltage}");
                    //读取出厂日期、软件版本、硬件版本
                    bool flag = ReadThreeData();
                    if (!flag)
                    {
                        LShowMessage("读取软件版本失败", "警告", MessageIcon.Warning);
                        return false;
                    }

                    if (testData.TestSofterWare != testData.SoftwareVersion)
                    {
                        LShowMessage($"软件版本冲突，当前板子软件版本为：{testData.SoftwareVersion},测试软件版本为：{testData.TestSofterWare}", "警告", MessageIcon.Warning);
                        return false;
                    }
                    else
                        return true;
                case "电芯电压":
                    //读取电芯电压
                    if (ReadDianxinVoltage())
                    {
                        string result = testData.AnalyseDianxinVoltage(testMachine.BatVol);
                        if (result != "")
                        {
                            AddLog($"电芯电压异常:{result}");
                            LShowMessage($"电芯电压异常:{result}", "电芯电压异常", MessageIcon.Warning);
                            return false;
                        }
                        return true;
                    }
                    else
                        return false;
                case "CAN通讯":

                    AddLog("正在测试CAN通讯");
                    ERROR_COUNT = 0;
                    //已进入测试模式
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = CanCommunicationTset();//CAN通讯
                        Thread.Sleep(1000);
                    } while (!interSuccess && ERROR_COUNT < 10);
                    if (!interSuccess)
                    {
                        AddLog("CAN测试异常过多");
                        return false;
                    }
                    return true;
                case "BMS逆变器通讯":

                    AddLog("正在测试BMS逆变器通讯");
                    //进入测试模式1
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    interSuccess = EnterTestMode();

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            ERROR_COUNT++;
                            interSuccess = BmsInverterCommunicationTset();//BMS逆变器通讯
                            Thread.Sleep(1000);
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("BMS逆变器通讯测试异常过多");
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        //进入测试模式失败
                        AddLog("进入测试模式异常");
                        return false;
                    }
                case "BMS并机通讯":
                    AddLog("正在测试BMS并机通讯");
                    //MessageResult boxResult = LShowMessage("准备测试BMS并机通讯，请把最左边的拨码打开，确保拨码值为1！", "测试暂停", MessageIcon.Information);
                    //进入测试模式1
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    interSuccess = EnterTestMode();

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            ERROR_COUNT++;
                            interSuccess = BmsParallelCommunicationTset();//BMS并机通讯
                            Thread.Sleep(1000);
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("BMS并机通讯测试异常过多");
                            AddLog("请确保拨码值为1");
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        //进入测试模式失败
                        AddLog("进入测试模式异常");
                        return false;
                    }
                case "复位开关":
                    AddLog("正在测试复位开关");
                    //打开复位继电器
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = ResetSwitchStatusTest(1);
                        if (!interSuccess)
                        {
                            AddLog("复位继电器打开失败");
                        }
                        else
                            AddLog("复位继电器打开成功");
                        Thread.Sleep(500);
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次
                    if (!interSuccess)
                    {
                        return false;
                    }
                    //boxResult = LShowMessage("准备测试复位开关，请确保复位开关按下！", "测试暂停",MessageIcon.Information);
                    //进入测试模式2
                    //查看是否打开复位开关
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    string receive = "";
                    do
                    {
                        ERROR_COUNT++;
                        //interSuccess = InterTestMode(2);
                        receive = SerialCommunicationService2.SendCommand("RSTSWCHK", 7);
                        if (receive == "RSTSWON")
                        {
                            interSuccess = true;
                            parametersSending.ResetSwitchStatus = 1;
                            AddLog($"复位开关：{receive}");
                            SendPacked(parametersSending);
                            //发完复0
                            parametersSending.ResetSwitchStatus = 0;
                        }
                        else if (receive == "RSTSWOF")
                        {
                            AddLog($"复位开关：{receive}");
                            AddLog("复位开关是关闭状态");
                            parametersSending.ResetSwitchStatus = 0;
                            SendPacked(parametersSending);
                        }
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次
                    if (!interSuccess)
                    {
                        return false;
                    }

                    //关闭复位继电器
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = ResetSwitchStatusTest(0);
                        if (!interSuccess)
                        {
                            AddLog("复位继电器关闭失败");
                        }
                        else
                            AddLog("复位继电器关闭成功");
                        Thread.Sleep(500);
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

                    return interSuccess;
                case "拨码开关":
                    AddLog("正在测试拨码开关");

                    //打开拨码继电器
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = OpenOrCloseDIPSwitchValueTest(1);
                        if (!interSuccess)
                        {
                            AddLog("拨码继电器打开失败");
                        }
                        else
                            AddLog("拨码继电器打开成功");
                        Thread.Sleep(500);
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次
                    if (!interSuccess)
                    {
                        return false;
                    }
                    //boxResult = LShowMessage("准备测试BMS并机通讯，确保四个拨码位全部打开！", "测试暂停",MessageIcon.Information);
                    //进入测试模式3
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    do
                    {
                        ERROR_COUNT++;
                        //interSuccess = InterTestMode(3);
                        receive = SerialCommunicationService2.SendCommand("ADDSWCHK", 7);
                        if (receive == "ADD0X15")
                        {
                            AddLog($"拨码值为：{receive}，通过");
                            interSuccess = true;
                        }
                        Thread.Sleep(200);
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次
                    if (!interSuccess)
                    {
                        AddLog($"拨码值为：{receive}，不是‘ADD0X15’，不通过");
                        return false;
                    }

                    DIPSwitchValueTest();//拨码开关
                    //boxResult = LShowMessage("BMS并机通讯已通过，请关闭右侧三个拨码开关！", "测试暂停", MessageIcon.Information);
                    Thread.Sleep(300);
                    //关闭拨码继电器
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = OpenOrCloseDIPSwitchValueTest(0);
                        if (!interSuccess)
                        {
                            AddLog("拨码继电器关闭失败");
                        }
                        else
                            AddLog("拨码继电器关闭成功");
                        Thread.Sleep(500);
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次
                    return interSuccess;
                case "干节点功能":
                    //进入测试模式4
                    AddLog("正在测试干节点功能");
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    do
                    {
                        ERROR_COUNT++;
                        //interSuccess = InterTestMode(4);
                        receive = SerialCommunicationService2.SendCommand("DRYCONPULL", 10);
                        if (receive == "DRYCONPULL")
                        {
                            interSuccess = true;
                        }
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            ERROR_COUNT++;
                            receive = SerialCommunicationService2.SendCommand("DRYCONPULL", 10);
                            if (receive == "DRYCONPULL")
                            {
                                interSuccess = true;
                            }
                            flag = RelayStatus() && interSuccess;//测试干节点开关
                            interSuccess = flag;
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("测试异常");
                            return false;
                        }
                        //干节点测试成功，退出测试模式
                        AddLog("干节点测试合格，即将退出测试模式");

                        //退出测试模式
                        interSuccess = false;
                        ERROR_COUNT = 0;
                        do
                        {
                            ERROR_COUNT++;
                            //interSuccess = RelayStatus();//测试干节点开关
                            receive = SerialCommunicationService2.SendCommand("QUITTESTMODE", 12);
                            if (receive == "QUITTESTMODE")
                            {
                                interSuccess = true;
                            }
                        } while (!interSuccess && ERROR_COUNT < 10);
                        //bool exitTest = ExitTestMode();
                        if (!interSuccess)
                        {
                            AddLog("退出测试模式失败");
                            return false;
                        }
                        AddLog("退出测试模式成功");
                        return true;
                    }
                    else
                    {
                        //进入测试模式失败
                        AddLog("进入测试模式4异常");
                        return false;
                    }
                case "低功耗检测":
                    AddLog("正在进行低功耗检测");
                    ERROR_COUNT = 0;
                    interSuccess = false;
                    bool final = false;
                    //先关预留继电器4
                    do
                    {
                        if (ERROR_COUNT++ == 10)
                        {
                            return false;
                        }
                        //
                        parametersSending.Reserved3RelayStatus = 0;
                        interSuccess = CloseParameter(parametersSending.Reserved3RelayStatus, "Reserved3RelayStatus", "预留继电器4");
                        Thread.Sleep(1000);
                    } while (!interSuccess);
                    AddLog("等待5秒");
                    Thread.Sleep(5000);
                    //五秒后关低功耗继电器
                    ERROR_COUNT = 0;
                    interSuccess = false;

                    do
                    {
                        if (ERROR_COUNT++ == 10)
                        {
                            return false;
                        }
                        //关闭低功耗继电器
                        interSuccess = LowPowerVoltageAndCurrent(0);
                    } while (!interSuccess);
                    AddLog("等待6S");
                    Thread.Sleep(6000);
                    //采低功耗电压
                    ERROR_COUNT = 0;
                    interSuccess = false;
                    //低功耗检测
                    do
                    {
                        if (ERROR_COUNT++ == 20)
                            break;

                        //获取低功耗电压、电流
                        BMS_Receive = SendPacked(parametersSending);
                        BMS_Receive.LowPowerCurrent = (ushort)(BMS_Receive.LowPowerVoltage * 1000000 / 450000);
                        AddLog($"低功耗检测：电压{BMS_Receive.LowPowerVoltage},电流：{BMS_Receive.LowPowerCurrent}");
                        //记录测试数据
                        testData.LowPowerCurrent = BMS_Receive.LowPowerCurrent; testData.LowPowerVoltage = BMS_Receive.LowPowerVoltage;
                        if (BMS_Receive.LowPowerCurrent <= 450)
                        {
                            if (BMS_Receive.LowPowerVoltage != 0 && ERROR_COUNT >= 5)
                            {
                                interSuccess = true;
                                final = true;
                            }
                        }
                        else
                        {
                            final = false;
                            break;
                        }
                        Thread.Sleep(2000);
                    } while (!interSuccess);

                    //开低功耗继电器
                    ERROR_COUNT = 0;
                    interSuccess = false;
                    do
                    {
                        if (ERROR_COUNT++ == 10)
                        {
                            return false;
                        }
                        //关闭低功耗继电器
                        interSuccess = LowPowerVoltageAndCurrent(1);
                    } while (!interSuccess);

                    ERROR_COUNT = 0;
                    bool interSuccess2 = false;
                    //开启预留继电器4
                    do
                    {
                        if (ERROR_COUNT++ == 10)
                        {
                            return false;
                        }
                        //
                        parametersSending.Reserved3RelayStatus = 1;
                        interSuccess2 = OpenParameter(parametersSending.Reserved3RelayStatus, "Reserved3RelayStatus", "预留继电器4");
                        Thread.Sleep(1000);
                    } while (!interSuccess2);

                    if (!EnterTestMode())
                    {
                        AddLog("开机失败,请检查");
                    }
                    Thread.Sleep(1000);
                    if (!interSuccess || !final || !interSuccess2)
                    {

                        AddLog("低功耗检测电压、电流,异常, 请检查");
                        return false;
                    }
                    return true;
                case "充电电流":
                    ERROR_COUNT = 0;
                    interSuccess = false;
                    interSuccess = ChargeCurrentTest(20);
                    if (!interSuccess)
                    {
                        AddLog("充电电流测试失败，正在关闭相应电子元件");
                        int failCount = 0;
                        bool succeed = false;
                        do
                        {
                            if (failCount++ == 10)
                            {
                                return false;
                            }
                            //关闭电子负载开关
                            parametersSending.Reserved1 = 0;
                            succeed = CloseParameter(parametersSending.Reserved1, "Reserved1", "电子负载开关");
                            Thread.Sleep(1000);
                        } while (!succeed);
                        Thread.Sleep(1000);
                        failCount = 0;
                        succeed = false;
                        //关闭充电继电器
                        do
                        {
                            if (failCount++ == 10)
                            {
                                return false;
                            }
                            //关闭充电继电器
                            parametersSending.ChargeRelayStatus = 0;
                            parametersSending.ElectronicLoadCurrent = 1;
                            succeed = CloseParameter(parametersSending.ChargeRelayStatus, "ChargeRelayStatus", "充电继电器");
                            Thread.Sleep(1000);
                        } while (!succeed);
                        return false;
                        
                    }
                    else
                    {
                        AddLog("充电电流测试成功");
                    }
                    return true;
                case "放电电流":
                    ERROR_COUNT = 0;
                    interSuccess = false;

                    //
                    int[] testDisCurrents = new int[] { 5, 13, 20 };
                    interSuccess = DisChargeCurrentTest(20);
                    if (!interSuccess)
                    {
                        AddLog("放电电流测试失败,正在关闭相应电子元件");
                        int failCount = 0;
                        bool succeed = false;
                        do
                        {
                            if (failCount++ == 10)
                            {
                                return false;
                            }
                            //关闭电子负载开关
                            parametersSending.Reserved1 = 0;
                            succeed = CloseParameter(parametersSending.Reserved1, "Reserved1", "电子负载开关");
                            Thread.Sleep(2000);
                        } while (!succeed);

                        failCount = 0;
                        succeed = false;
                        do
                        {
                            if (failCount++ == 10)
                            {
                                return false;
                            }
                            //关闭放电继电器
                            parametersSending.DischargeRelayStatus = 0;
                            parametersSending.ElectronicLoadCurrent = 1;
                            succeed = CloseParameter(parametersSending.DischargeRelayStatus, "DischargeRelayStatus", "放电继电器");
                            Thread.Sleep(1000);
                        } while (!succeed);
                        return false;
                        
                    }
                    else
                    {
                        AddLog("放电电流测试成功");
                    }

                    return true;
                case "充电限流检测":
                    ERROR_COUNT = 0;
                    interSuccess = false;
                    AddLog("准备测试充电限流");
                    //开启低功耗继电器
                    interSuccess = ChargeCurrentLimitTest();
                    //没过的话关闭对应电子元件
                    if (!interSuccess)
                    {
                        AddLog("充电限流检测失败！正在关闭相关电子元件");
                        int failCount = 0;
                        bool succeed = false;
                        do
                        {
                            if (failCount++ == 10)
                            {
                                return false;
                            }
                            //关闭电阻棒MOS管
                            parametersSending.ResistorBankMosfetStatus = 0;
                            succeed = CloseParameter(parametersSending.ResistorBankMosfetStatus, "ResistorBankMosfetStatus", "确保电阻棒MOS管");
                            Thread.Sleep(1000);
                        } while (!succeed);

                        failCount = 0;
                        succeed = false;
                        do
                        {
                            if (failCount++ == 10)
                            {
                                return false;
                            }
                            //限流负极继电器
                            parametersSending.ChargeCurrentLimitNegativeRelayStatus = 0;
                            succeed = OpenParameter(parametersSending.ChargeCurrentLimitNegativeRelayStatus, "ChargeCurrentLimitNegativeRelayStatus", "充电限流负极继电器");
                            Thread.Sleep(1000);
                        } while (!succeed);

                        failCount = 0;
                        succeed = false;
                        do
                        {
                            if (failCount++ == 10)
                            {
                                return false;
                            }
                            //关闭电子负载开关
                            parametersSending.Reserved1 = 0;
                            succeed = CloseParameter(parametersSending.Reserved1, "Reserved1", "确保电子负载开关");
                            Thread.Sleep(1000);
                        } while (!succeed);

                        failCount = 0;
                        succeed = false;
                        //关闭充电继电器
                        do
                        {
                            if (failCount++ == 10)
                            {
                                return false;
                            }
                            //关闭充电继电器
                            parametersSending.ChargeRelayStatus = 0;
                            parametersSending.ElectronicLoadCurrent = 1;
                            succeed = CloseParameter(parametersSending.ChargeRelayStatus, "ChargeRelayStatus", "充电继电器");
                            Thread.Sleep(1000);
                        } while (!succeed);


                        //关闭限流开关
                        failCount = 0;
                        succeed = false;
                        AddLog("正在关闭BMS限流开关");
                        do
                        {
                            if (failCount++ == 10)
                            {
                                AddLog("关闭限流开关不成功");
                                return false;
                            }
                            succeed = CloseLimitedCurrent();
                            Thread.Sleep(1000);
                        } while (!succeed);

                    }
                    return interSuccess;
                case "整机-BMS232通讯":
                    AddLog("正在测试BMS232通讯");
                    //进入测试模式1
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    interSuccess = EnterTestMode();

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            interSuccess = Bms232CommunicationTset();//BMS232通讯
                            Thread.Sleep(1000);
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("BMS232通讯异常");
                            return false;
                        }
                        //读取系统时间
                        ReadTime();
                        //读取出厂日期、软件版本、硬件版本
                        flag = ReadThreeData();
                        if (!flag)
                        {
                            LShowMessage("读取软件版本失败", "警告", MessageIcon.Warning);
                            return false;
                        }

                        if (testData.TestSofterWare != testData.SoftwareVersion)
                        {
                            LShowMessage($"软件版本冲突，当前板子软件版本为：{testData.SoftwareVersion},测试软件版本为：{testData.TestSofterWare}", "警告", MessageIcon.Warning);
                            return false;
                        }
                        //读取是否激活
                        if (!WriteBeforeTest())
                        {
                            LShowMessage("当前BMS板未激活，请先激活", "警告", MessageIcon.Warning);
                            return false;
                        }
                        //读取电芯电压
                        if (ReadDianxinVoltage())
                        {
                            string result = testData.AnalyseDianxinVoltage(testMachine.BatVol);
                            if (result != "")
                            {
                                AddLog($"电芯电压异常:{result}");
                                LShowMessage($"电芯电压异常:{result}", "电芯电压异常", MessageIcon.Warning);
                                return false;
                            }
                            return true;
                        }
                        else
                            return false;

                        //写入出厂日期
                        //WriteProductDate(
                       
                    }
                    else
                    {
                        //进入测试模式失败
                        AddLog("进入测试模式异常");
                       
                        return false;
                    }   
                case "整机-CAN通讯":
                    AddLog("正在测试CAN通讯");
                    ERROR_COUNT = 0;
                    //已进入测试模式
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = CanCommunicationTset();//CAN通讯
                        Thread.Sleep(1000);
                    } while (!interSuccess && ERROR_COUNT < 10);
                    if (!interSuccess)
                    {
                        AddLog("CAN测试异常过多");
                        LShowMessage("请确保拨码开关值为1", "ERROR", MessageIcon.Error);
                        return false;
                    }
                    return true;
                case "整机-BMS逆变器通讯":
                    AddLog("正在测试BMS逆变器通讯");
                    //进入测试模式1
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    interSuccess = EnterTestMode();

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            ERROR_COUNT++;
                            interSuccess = BmsInverterCommunicationTset();//BMS逆变器通讯
                            Thread.Sleep(500);
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("BMS逆变器通讯测试异常过多");
                            LShowMessage("请确保拨码开关值为1", "ERROR", MessageIcon.Error);
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        //进入测试模式失败
                        AddLog("进入测试模式异常");
                        LShowMessage("请确保拨码开关值为1", "ERROR", MessageIcon.Error);
                        return false;
                    }                  
                case "整机-BMS并机通讯":
                    AddLog("正在测试BMS并机通讯");
                    //MessageResult boxResult = LShowMessage("准备测试BMS并机通讯，请把最左边的拨码打开，确保拨码值为1！", "测试暂停", MessageIcon.Information);

                    //进入测试模式1
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    interSuccess = EnterTestMode();

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            ERROR_COUNT++;
                            interSuccess = BmsParallelCommunicationTset();//BMS并机通讯
                            Thread.Sleep(1000);
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("BMS并机通讯测试异常过多");
                            AddLog("请确保拨码值为1");
                            LShowMessage("请确保拨码开关值为1", "ERROR", MessageIcon.Error);
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        LShowMessage("请确保拨码开关值为1", "ERROR", MessageIcon.Error);
                        //进入测试模式失败
                        AddLog("进入测试模式异常");
                        return false;
                    }
                default:
                    return false;
            }
        }


        /// <summary>
        /// 测试项复位
        /// </summary>
        private void ReSetTestItems()
        {
            foreach (TestItem item in TestItems)
            {
                item.Flag = 0;
                item.IsImportant = false;
            }
            //复位测试参数
            testData.ReSetParameters();
            BMSCurrent = 0;

        }

        #endregion

        #region 测试项目实际操作方法

        #region 串口一(上位机串口)

        byte[] Head = new byte[] { 0x01, 0x03, 0x24 }; //帧头



        /// <summary>
        /// BMS232通讯
        /// </summary>
        /// <returns></returns>
        private bool Bms232CommunicationTset()
        {
            ////测试模式置1
            //parametersSending.TestMode = 1;

            ////拼接字符串
            //byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            //sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC(sengdingPack));

            ////发送字符串
            //byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 65);

            ////解析
            //BmsSystemparametersReceive bms = AnalyseBmsReceive(result);

            //测试模式置           
            parametersSending.TestMode = 1;
            parametersSending.Bms232Communication = 1;

            //拼接报文                                                               
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());                    //帧头 + 数据
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack)); //帧头 + 数据 + CRC校验码 

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            return true;
            //解析成帧对象
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断BMS232通讯是否正常
                if (BMS_Receive.Bms232Communication == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// CAN通讯
        /// </summary>
        /// <returns></returns>
        private bool CanCommunicationTset()
        {
            //测试模式置1
            parametersSending.TestMode = 1;

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断CAN通讯是否正常
                if (BMS_Receive.CanCommunication == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// BMS逆变器通讯
        /// </summary>
        /// <returns></returns>
        private bool BmsInverterCommunicationTset()
        {
            //测试模式置1
            parametersSending.TestMode = 1;

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断BMS逆变器通讯是否正常
                if (BMS_Receive.BmsInverterCommunication == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// BMS并机通讯
        /// </summary>
        /// <returns></returns>
        private bool BmsParallelCommunicationTset()
        {
            //测试模式置1
            parametersSending.TestMode = 1;

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断BMS逆变器通讯是否正常
                if (BMS_Receive.BmsParallelCommunication == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 复位开关
        /// </summary>
        /// <returns></returns>
        private bool ResetSwitchStatusTest(ushort open)
        {
            //测试模式置2
            parametersSending.Reserved3 = open;

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断复位开关是否正常
                if (BMS_Receive.Reserved3 == 1 && open == 1)
                {
                    return true;
                }
                else if (BMS_Receive.Reserved3 == 0 && open == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 拨码开关
        /// </summary>
        /// <returns></returns>
        private bool OpenOrCloseDIPSwitchValueTest(ushort open)
        {
            //测试模式置3

            parametersSending.Reserved2 = open;

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);
            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断复位开关是否正常
                if (BMS_Receive.Reserved2 == 1 && open == 1)
                {
                    return true;
                }
                else if (BMS_Receive.Reserved2 == 0 && open == 0)
                {
                    return true;
                }
            }
            return false;

        }


        /// <summary>
        /// 拨码开关结果发送
        /// </summary>
        /// <returns></returns>
        private bool DIPSwitchValueTest()
        {
            //测试模式置3
            parametersSending.TestMode = 3;
            parametersSending.DIPSwitchStatus = 15;

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            return true;

        }

        /// <summary>
        /// 干节点开关
        /// </summary>
        /// <returns></returns>
        private bool RelayStatus()
        {


            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断干节点开关是否正常
                if (BMS_Receive.Relay1Status == 1 && BMS_Receive.Relay2Status == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 充电电流校准
        /// </summary>
        /// <param name="level">等级</param>
        /// <returns></returns>
        private bool ChargeCurrentTest(int level)
        {
            Thread.Sleep(1000);
            int successCount = 0;
            //前置步骤
            //成功后关闭MOS管，恢复原样
            int failCount = 0;
            bool succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭电子负载开关
                parametersSending.Reserved1 = 0;
                succeed = CloseParameter(parametersSending.Reserved1, "Reserved1", "确保电子负载开关");
                Thread.Sleep(1000);
            } while (!succeed);

            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭电阻棒MOS管
                parametersSending.ResistorBankMosfetStatus = 0;
                parametersSending.ElectronicLoadCurrent = 20;
                succeed = CloseParameter(parametersSending.ResistorBankMosfetStatus, "ResistorBankMosfetStatus", "确保电阻棒MOS管");
                Thread.Sleep(1000);
            } while (!succeed);
            

            Thread.Sleep(2000);
            failCount = 0;
            succeed = false;
            //第一步 开启充电继电器
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //开启充电继电器
                parametersSending.ChargeRelayStatus = 1;
                parametersSending.ElectronicLoadCurrent = 20;
                succeed = OpenParameter(parametersSending.ChargeRelayStatus, "ChargeRelayStatus", "充电继电器");
                Thread.Sleep(1000);
            } while (!succeed);



            //第二步 关闭放电继电器
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭放电继电器
                parametersSending.DischargeRelayStatus = 0;
                succeed = CloseParameter(parametersSending.DischargeRelayStatus, "DischargeRelayStatus", "放电继电器");
                Thread.Sleep(500);
            } while (!succeed);



            //第三步 关闭充电限流负极继电器
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭充电限流负极继电器
                parametersSending.ChargeCurrentLimitNegativeRelayStatus = 0;
                succeed = CloseParameter(parametersSending.ChargeCurrentLimitNegativeRelayStatus, "ChargeCurrentLimitNegativeRelayStatus", "充电限流负极继电器");
                Thread.Sleep(500);
            } while (!succeed);


            //第四步 关闭电阻棒MOS管
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭电阻棒MOS管
                parametersSending.ResistorBankMosfetStatus = 0;
                succeed = CloseParameter(parametersSending.ResistorBankMosfetStatus, "ResistorBankMosfetStatus", "电阻棒MOS管");
                Thread.Sleep(500);
            } while (!succeed);

            //第五步 读取当前负载模式为CC就设置电子负载电流为30A
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //读取当前负载模式为CC
                parametersSending.ElectronicLoadMode = 1;
                succeed = OpenParameter(parametersSending.ElectronicLoadMode, "ElectronicLoadMode", "负载模式为CC");
                Thread.Sleep(500);
            } while (!succeed);

            failCount = 0;
            succeed = false;
            AddLog("准备设置电子负载电流为20A");
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //设置电子负载电流为30A
                parametersSending.ElectronicLoadCurrent = 20;
                BMS_Receive = SendPacked(parametersSending);
                Thread.Sleep(500);
                succeed = true;
            } while (!succeed);

            //判断条件
            BMS_Receive = SendPacked(parametersSending);
            if (!(BMS_Receive.ChargeRelayStatus == 1 && BMS_Receive.DischargeRelayStatus == 0 && BMS_Receive.ResistorBankMosfetStatus == 0 && BMS_Receive.ChargeCurrentLimitNegativeRelayStatus == 0 && BMS_Receive.Reserved1 == 0))
            {
                string charge = BMS_Receive.ChargeRelayStatus != 1 ? "充电继电器没开启" : "";
                string disCharge = BMS_Receive.DischargeRelayStatus != 0 ? "放电继电器没关闭" : "";
                string resistorBankMosfetStatus = BMS_Receive.ResistorBankMosfetStatus != 0 ? "电阻棒MOS管没关闭" : "";
                string chargeCurrentLimitNegativeRelayStatus = BMS_Receive.ChargeCurrentLimitNegativeRelayStatus != 0 ? "充电限流负极继电器没关闭" : "";
                string relay1Status = BMS_Receive.Reserved1 != 0 ? "电子负载没关闭" : "";
                AddLog($"前置条件不符！");
                AddLog($"{charge}");
                AddLog($"{disCharge}");
                AddLog($"{resistorBankMosfetStatus}");
                AddLog($"{chargeCurrentLimitNegativeRelayStatus}");
                AddLog($"{relay1Status}");

                return false;
            }
            Thread.Sleep(1000);

            //第六步 打开电子负载开关
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //打开电子负载开关
                parametersSending.Reserved1 = 1;
                succeed = OpenParameter(parametersSending.Reserved1, "Reserved1", "电子负载开关");
                Thread.Sleep(1000);
            } while (!succeed);

            Thread.Sleep(1000);
            //先设置电流为5A，
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //设置电子负载电流为10A
                parametersSending.ElectronicLoadCurrent = 20;
                BMS_Receive = SendPacked(parametersSending);
                int Current = BMS_Receive.ElectronicLoadCurrent;
                AddLog($"当前电流为{Current}");
                //判断是否达到
                if (Math.Abs(Current - 2000) < 20)
                {
                    succeed = true;
                }
                Thread.Sleep(2000);
            } while (!succeed);

            //Thread.Sleep(1000);
            ////设置电流为13A，
            //failCount = 0;
            //succeed = false;
            //do
            //{
            //    if (failCount++ == 10)
            //    {
            //        return false;
            //    }
            //    //设置电子负载电流为15A
            //    parametersSending.ElectronicLoadCurrent = 20;
            //    BMS_Receive = SendPacked(parametersSending);
            //    int Current = BMS_Receive.ElectronicLoadCurrent;
            //    AddLog($"当前电流为{Current}");
            //    //判断是否达到
            //    if (Math.Abs(Current - 2000) < 20)
            //    {
            //        succeed = true;
            //    }
            //    Thread.Sleep(2000);
            //} while (!succeed);

            Thread.Sleep(1000);
            //设置电流为20A，
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //设置电子负载电流为20A
                parametersSending.ElectronicLoadCurrent = 20;
                BMS_Receive = SendPacked(parametersSending);
                int Current = BMS_Receive.ElectronicLoadCurrent;
                AddLog($"当前电流为{Current}");
                //判断是否达到
                if (Math.Abs(Current - 2000) < 10)
                {
                    succeed = true;
                }
                Thread.Sleep(2000);
            } while (!succeed);

            //第七步 采集电子负载电流(准确值)与BMS板电流(调校值)
            //Thread.Sleep(2000);
            int com2Current = 0;
            int com1Current = 0;

            failCount = 0;
            succeed = false;
            do
            {
                BMS_Receive = SendPacked(parametersSending);
                com2Current = GetCurrentFormBMS();
                com1Current = BMS_Receive.ElectronicLoadCurrent;
                //记录当前的电流
                // testData.ChargeCom1CurrentResult = (ushort)com1Current;
                BMSCurrent = (ushort)com2Current;
                AddLog($"com1标准电流{com1Current};com2电流{com2Current}");
                //记录刚开始的电流和校准系数
                if (failCount == 0)
                {
                    testData.ChargeCom1Current = (ushort)com1Current; testData.ChargeCom2Current = (ushort)com2Current;
                    testData.ChargeAdjustNum = ReadChargeCurrentAdjustParameter();
                }


                //电流为0的情况
                if (com1Current == 0 || com2Current == 0)
                {
                    ushort chargeAdj = ReadChargeCurrentAdjustParameter();
                    AddLog($"当前充电校准系数:{chargeAdj}");
                    ChargeCurrentAdj = chargeAdj;
                    if (chargeAdj == 0)
                    {
                        bool su = WriteChargeCurrentAdjustParameter(1000);
                        string meg = su ? "成功" : "失败";
                        AddLog($"写入充电校准系数1000{meg}");
                        SetChargeCurrentAdj = 1000;
                    }
                    failCount++;
                    Thread.Sleep(1000);
                    continue;
                }

                if (Math.Abs(com1Current - 2000) > 10)
                {
                    AddLog("标准电流超过预期范围，不校准");
                }
                //电流都不为0的情况
                else if (CommunicateTool.AreWithinFive(com2Current, com1Current))
                {

                    ushort adjustReceive = ReadChargeCurrentAdjustParameter();
                    AddLog($"当前充电校准系数:{adjustReceive}");
                    //通过时校准系数
                    testData.WriteChargeAdjustNum = adjustReceive;
                    SetChargeCurrentAdj = adjustReceive;
                    testData.ChargeCom1CurrentResult = (ushort)com1Current;
                    testData.ChargeCom2CurrentResult = (ushort)com2Current;
                    successCount++;
                    //相差在合适范围，返回成功
                    AddLog("通过");
                    if (successCount > 3)
                        succeed = true;
                    Thread.Sleep(2000);
                }
                else
                {

                    //有差距，获取当前充电校准系数
                    ushort adjustReceive = ReadChargeCurrentAdjustParameter();

                    AddLog($"当前充电校准系数:{adjustReceive}");
                    ChargeCurrentAdj = adjustReceive;
                    int adjustSend = 0;
                    if (adjustReceive != 0)
                    {
                        //计算校准系数
                        adjustSend = GetAdjustParatemer(com1Current, com2Current, adjustReceive);
                    }
                    else
                    {
                        adjustSend = 1000;
                    }


                    //写入校准系数
                    bool su = WriteChargeCurrentAdjustParameter((ushort)adjustSend);
                    SetChargeCurrentAdj = (ushort)adjustSend;
                    if (su)
                    {
                        AddLog($"写入充电校准系数成功:{adjustSend}");
                        testData.WriteChargeAdjustNum = (ushort)adjustSend;
                    }
                    else
                    {
                        AddLog($"写入充电校准系数失败:{adjustSend}");
                    }
                    Thread.Sleep(2000);
                }
                failCount++;
            } while (!succeed && failCount < 30);

            if (!succeed)
            {
                AddLog("校准失败");
                return false;
            }
            else
            {
                AddLog("充电校准成功！");
                //成功后关闭MOS管，恢复原样
                failCount = 0;
                succeed = false;
                do
                {
                    if (failCount++ == 10)
                    {
                        return false;
                    }
                    //关闭电子负载开关
                    parametersSending.Reserved1 = 0;
                    succeed = CloseParameter(parametersSending.Reserved1, "Reserved1", "电子负载开关");
                    Thread.Sleep(1000);
                } while (!succeed);
                Thread.Sleep(1000);
                failCount = 0;
                succeed = false;
                //关闭充电继电器
                do
                {
                    if (failCount++ == 10)
                    {
                        return false;
                    }
                    //关闭充电继电器
                    parametersSending.ChargeRelayStatus = 0;
                    parametersSending.ElectronicLoadCurrent = 1;
                    succeed = CloseParameter(parametersSending.ChargeRelayStatus, "ChargeRelayStatus", "充电继电器");
                    Thread.Sleep(1000);
                } while (!succeed);
            }
            return true;
        }

        /// <summary>
        /// 放电电流校准
        /// </summary>
        /// <param name="level">等级</param>
        /// <returns></returns>
        private bool DisChargeCurrentTest(int level)
        {

            int failCount = 0;
            bool succeed = false;
            int successCount = 0;
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭电阻棒MOS管
                parametersSending.ResistorBankMosfetStatus = 0;
                succeed = CloseParameter(parametersSending.ResistorBankMosfetStatus, "ResistorBankMosfetStatus", "确保电阻棒MOS管");
                Thread.Sleep(500);
            } while (!succeed);

            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭电子负载开关
                parametersSending.Reserved1 = 0;
                succeed = CloseParameter(parametersSending.Reserved1, "Reserved1", "确保电子负载开关");
                Thread.Sleep(500);
            } while (!succeed);

            failCount = 0;
            succeed = false;
            //第一步 关闭充电继电器
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭充电继电器
                parametersSending.ChargeRelayStatus = 0;
                parametersSending.ElectronicLoadCurrent = 20;
                succeed = CloseParameter(parametersSending.ChargeRelayStatus, "ChargeRelayStatus", "充电继电器");
                Thread.Sleep(500);
            } while (!succeed);



            //第二步 打开放电继电器
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //打开放电继电器
                parametersSending.DischargeRelayStatus = 1;
                parametersSending.ElectronicLoadCurrent = 20;
                succeed = OpenParameter(parametersSending.DischargeRelayStatus, "DischargeRelayStatus", "放电继电器");
                Thread.Sleep(1000);
            } while (!succeed);

            Thread.Sleep(1000);

            //第三步 关闭充电限流负极继电器
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭充电限流负极继电器
                parametersSending.ChargeCurrentLimitNegativeRelayStatus = 0;
                succeed = CloseParameter(parametersSending.ChargeCurrentLimitNegativeRelayStatus, "ChargeCurrentLimitNegativeRelayStatus", "充电限流负极继电器");
                Thread.Sleep(200);
            } while (!succeed);


            //第四步 关闭电阻棒MOS管
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭电阻棒MOS管
                parametersSending.ResistorBankMosfetStatus = 0;
                succeed = CloseParameter(parametersSending.ResistorBankMosfetStatus, "ResistorBankMosfetStatus", "电阻棒MOS管");
                Thread.Sleep(200);
            } while (!succeed);

            //第五步 读取当前负载模式为CC就设置电子负载电流为30A
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //读取当前负载模式为CC
                parametersSending.ElectronicLoadMode = 1;
                succeed = OpenParameter(parametersSending.ElectronicLoadMode, "ElectronicLoadMode", "负载模式为CC");
                Thread.Sleep(500);
            } while (!succeed);

            failCount = 0;
            succeed = false;
            AddLog("准备设置电子负载电流为20A");
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //设置电子负载电流为30A
                parametersSending.ElectronicLoadCurrent = 20;
                BMS_Receive = SendPacked(parametersSending);
                if (BMS_Receive != null)
                {
                    AddLog($"当前电子负载电流为{BMS_Receive.ElectronicLoadCurrent}");
                    succeed = true;
                }
                else
                    succeed = false;
                Thread.Sleep(500);
            } while (!succeed);

            //判断条件
            BMS_Receive = SendPacked(parametersSending);
            if (!(BMS_Receive.ChargeRelayStatus == 0 && BMS_Receive.DischargeRelayStatus == 1 && BMS_Receive.ResistorBankMosfetStatus == 0 && BMS_Receive.ChargeCurrentLimitNegativeRelayStatus == 0 && BMS_Receive.Reserved1 == 0))
            {
                string charge = BMS_Receive.ChargeRelayStatus != 0 ? "充电继电器没关闭" : "";
                string disCharge = BMS_Receive.DischargeRelayStatus != 1 ? "放电继电器没开启" : "";
                string resistorBankMosfetStatus = BMS_Receive.ResistorBankMosfetStatus != 0 ? "电阻棒MOS管没关闭" : "";
                string chargeCurrentLimitNegativeRelayStatus = BMS_Receive.ChargeCurrentLimitNegativeRelayStatus != 0 ? "充电限流负极继电器没关闭" : "";
                string relay1Status = BMS_Receive.Reserved1 != 0 ? "电子负载没关闭" : "";
                AddLog($"前置条件不符！");
                AddLog($"{charge}");
                AddLog($"{disCharge}");
                AddLog($"{resistorBankMosfetStatus}");
                AddLog($"{chargeCurrentLimitNegativeRelayStatus}");
                AddLog($"{relay1Status}");
                return false;
            }

            //第六步 打开电子负载开关
            Thread.Sleep(1000);

            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //打开电子负载开关
                parametersSending.Reserved1 = 1;
                succeed = OpenParameter(parametersSending.Reserved1, "Reserved1", "电子负载开关");
                Thread.Sleep(1000);
            } while (!succeed);

            Thread.Sleep(2000);

            //先设置电流为5A，
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 5)
                {
                    return false;
                }
                //设置电子负载电流为30A
                parametersSending.ElectronicLoadCurrent = 20;
                BMS_Receive = SendPacked(parametersSending);
                int Current = BMS_Receive.ElectronicLoadCurrent;
                AddLog($"当前电流为{Current}");
                //判断是否达到
                if (Math.Abs(Current - 2000) < 20)
                {
                    succeed = true;
                }
                Thread.Sleep(1000);
            } while (!succeed);

            //Thread.Sleep(1000);
            ////设置电流为13A，
            //failCount = 0;
            //succeed = false;
            //do
            //{
            //    if (failCount++ == 10)
            //    {
            //        return false;
            //    }
            //    //设置电子负载电流为30A
            //    parametersSending.ElectronicLoadCurrent = 20;
            //    BMS_Receive = SendPacked(parametersSending);
            //    int Current = BMS_Receive.ElectronicLoadCurrent;
            //    AddLog($"当前电流为{Current}");
            //    //判断是否达到
            //    if (Math.Abs(Current - 2000) < 20)
            //    {
            //        succeed = true;
            //    }
            //    Thread.Sleep(2000);
            //} while (!succeed);

            Thread.Sleep(1000);
            //设置电流为20A，
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 5)
                {
                    return false;
                }
                //设置电子负载电流为30A
                parametersSending.ElectronicLoadCurrent = 20;
                BMS_Receive = SendPacked(parametersSending);
                int Current = BMS_Receive.ElectronicLoadCurrent;
                AddLog($"当前电流为{Current}");
                //判断是否达到
                if (Math.Abs(Current - 2000) < 10)
                {
                    succeed = true;
                }
                Thread.Sleep(1000);
            } while (!succeed);

            //第七步 读取电流(串口2是需要校准的，串口1是万瑞达的).对比差值，读充电系数，写充电系数
            Thread.Sleep(1000);
            int com2Current = 0;
            int com1Current = 0;

            failCount = 0;
            succeed = false;
            int AdSuccess = 0;//短路保护系数读取成功次数
            do
            {
                BMS_Receive = SendPacked(parametersSending);
                com2Current = GetCurrentFormBMS();
                BMSCurrent = (ushort)com2Current;
                com1Current = BMS_Receive.ElectronicLoadCurrent;
                AddLog($"com1电流{com1Current};com2电流{com2Current}");
                //记录第一次的数据
                if (failCount == 0)
                {
                    testData.DisChargeCom1Current = (ushort)com1Current;
                    testData.DisChargeCom2Current = (ushort)com2Current;
                    testData.DisChargeAdjustNum = ReadDischargeCurrentAdjustParameter();
                }
                if (com1Current == 0 || com2Current == 0)
                {
                    ushort dischargeAdj = ReadDischargeCurrentAdjustParameter();
                    AddLog($"当前充电校准系数:{dischargeAdj}");
                    DischargeCurrentAdj = dischargeAdj;
                    if (dischargeAdj == 0)
                    {
                        bool su = WriteDischargeCurrentAdjustParameter(1000);
                        string meg = su ? "成功" : "失败";
                        AddLog($"写入充电校准系数1000{meg}");
                        SetDischargeCurrentAdj = 1000;
                        Thread.Sleep(1000);
                    }
                    failCount++;
                    continue;
                }

                if (Math.Abs(com1Current - 2000) > 10)
                {
                    AddLog("标准电流超过预期范围,不校准");
                    Thread.Sleep(500);
                }
                else if (CommunicateTool.AreWithinFive(com2Current, com1Current))
                {
                    ushort adjustReceive = ReadDischargeCurrentAdjustParameter();
                    AddLog($"当前放电校准系数:{adjustReceive}");
                    DischargeCurrentAdj = adjustReceive;
                    //记录最终结果数据
                    testData.DisChargeCom1CurrentResult = (ushort)com1Current;
                    testData.DisChargeCom2CurrentResult = (ushort)com2Current;
                    testData.WriteDisChargeAdjustNum = adjustReceive;
                    //相差在合适范围，返回成功
                    AddLog("通过");
                    successCount++;
                    //相差在合适范围，返回成功
                    if (successCount > 3)
                        succeed = true;
                    int ad = ReadAdParameter();
                    if (0<ad &&ad < 400)
                    {
                        AdSuccess++;
                    }
                    AddLog($"短路检测系数:{ad}");
                    Thread.Sleep(2000);
                }
                else
                {

                    //有差距，获取当前放电校准系数
                    ushort adjustReceive = ReadDischargeCurrentAdjustParameter();
                    AddLog($"当前放电校准系数:{adjustReceive}");
                    DischargeCurrentAdj = adjustReceive;

                    int adjustSend;
                    if (adjustReceive == 0)
                    {
                        adjustSend = 1000;
                    }
                    else
                        //计算校准系数
                        adjustSend = GetAdjustParatemer(com1Current, com2Current, adjustReceive);
                    SetDischargeCurrentAdj = (ushort)adjustSend;
                    //写入校准系数
                    bool su = WriteDischargeCurrentAdjustParameter((ushort)adjustSend);
                    if (su)
                    {
                        AddLog($"写入放电校准系数成功:{adjustSend}");
                        testData.WriteDisChargeAdjustNum = (ushort)adjustSend;
                    }
                    else
                    {
                        AddLog($"写入放电校准系数失败:{adjustSend}");
                    }
                    Thread.Sleep(2000);
                }
                failCount++;
            } while (!succeed && failCount < 15);

            if (!succeed)
            {
                AddLog("校准失败");
                
                return false;
            }
            else
            {
                //成功后关闭MOS管，恢复原样
                AddLog("放电校准成功！");
                //成功后关闭电子负载开关，恢复原样
                failCount = 0;
                succeed = false;
                do
                {
                    if (failCount++ == 10)
                    {
                        return false;
                    }
                    //关闭电子负载开关
                    parametersSending.Reserved1 = 0;
                    succeed = CloseParameter(parametersSending.Reserved1, "Reserved1", "电子负载开关");
                    Thread.Sleep(1000);
                } while (!succeed);
                Thread.Sleep(1000);

                failCount = 0;
                succeed = false;
                do
                {
                    if (failCount++ == 10)
                    {
                        return false;
                    }
                    //关闭放电继电器
                    parametersSending.DischargeRelayStatus = 0;
                    parametersSending.ElectronicLoadCurrent = 1;
                    succeed = CloseParameter(parametersSending.DischargeRelayStatus, "DischargeRelayStatus", "放电继电器");
                    Thread.Sleep(1000);
                } while (!succeed);

                //1004软件版本启用
                if (testData.TestSofterWare != "1003")
                {
                    if (AdSuccess < 3)
                    {
                        AddLog("短路保护检测异常，请检查");
                        LShowMessage("短路保护检测异常", "短路保护", MessageIcon.Error);
                        return false;
                    }
                    else
                    {
                        AddLog("短路保护检测测试通过");
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// 计算校准系数
        /// </summary>
        /// <param name="com1Real"></param>
        /// <param name="com2Test"></param>
        /// <param name="currentAdj"></param>
        /// <returns></returns>
        private int GetAdjustParatemer(int com1Real, int com2Test, ushort currentAdj)
        {
            // 计算com2的原始值 (com2当前值 / 比值)
            double originalCom2 = com2Test / (currentAdj / 1000.0);

            // 计算com1与com2原始值的比值
            double ratio = com1Real / originalCom2;

            // 将比值放大100倍并返回整数部分
            return (int)(ratio * 1000) + 1;
        }

        /// <summary>
        /// 充电限流检测
        /// </summary>
        /// <returns></returns>
        private bool ChargeCurrentLimitTest()
        {
            int failCount = 0;
            bool succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭电阻棒MOS管
                parametersSending.ResistorBankMosfetStatus = 0;
                succeed = CloseParameter(parametersSending.ResistorBankMosfetStatus, "ResistorBankMosfetStatus", "确保电阻棒MOS管");
                Thread.Sleep(1000);
            } while (!succeed);

            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭电子负载开关
                parametersSending.Reserved1 = 0;
                succeed = CloseParameter(parametersSending.Reserved1, "Reserved1", "确保电子负载开关");
                Thread.Sleep(500);
            } while (!succeed);

            //关闭限流开关
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    AddLog("确保限流开关关闭不成功");
                    return false;
                }
                succeed = CloseLimitedCurrent();
                Thread.Sleep(1000);
            } while (!succeed);

            failCount = 0;
            succeed = false;
            //第一步 打开充电继电器
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //充电打开继电器
                parametersSending.ChargeRelayStatus = 1;
                succeed = OpenParameter(parametersSending.ChargeRelayStatus, "ChargeRelayStatus", "充电继电器");
                Thread.Sleep(1000);
            } while (!succeed);

            Thread.Sleep(1000);

            //第二步 关闭放电继电器
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭放电继电器
                parametersSending.DischargeRelayStatus = 0;
                succeed = CloseParameter(parametersSending.DischargeRelayStatus, "DischargeRelayStatus", "放电继电器");
                Thread.Sleep(1000);
            } while (!succeed);

            //第三步 关闭电子负载开关
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //打开电子负载开关
                parametersSending.Reserved1 = 0;
                succeed = CloseParameter(parametersSending.Reserved1, "Reserved1", "电子负载开关");
                Thread.Sleep(1000);
            } while (!succeed);

            //第四步 设置dc1和dc2的电流电压
            parametersSending.DcSource1Current = 4000;//(50A)
            if (testMachine.BatVol == "24")
            {
                parametersSending.DcSource1Voltage = 2500;//(25V)
                parametersSending.DcSource2Voltage = 2700;//(27V)
            }
            else if (testMachine.BatVol == "48")
            {
                parametersSending.DcSource1Voltage = 5000;//(50V)
                parametersSending.DcSource2Voltage = 5200;//(52V)
            }
            parametersSending.DcSource2Current = 4000;//(40A)
            

            //第五步 dc源1/2开
            failCount = 0;
            succeed = false;

            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //打开dc源1
                parametersSending.DcSource1Switch = 1;
                succeed = OpenParameter(parametersSending.DcSource1Switch, "DcSource1Switch", "直流源1");
                Thread.Sleep(500);
            } while (!succeed);
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //打开dc源2
                parametersSending.DcSource2Switch = 1;
                succeed = OpenParameter(parametersSending.DcSource2Switch, "DcSource2Switch", "直流源2");
                Thread.Sleep(500);
            } while (!succeed);

            Thread.Sleep(2000);

            BMS_Receive = SendPacked(parametersSending);
            if (!(BMS_Receive.DcSource1Current <= 200 && BMS_Receive.DcSource2Current <= 200))
            {
                AddLog($"dc1电流{BMS_Receive.DcSource1Current},dc2电流{BMS_Receive.DcSource2Current}，超过预定区间");
                return false;
            }

            //第六步 打开充电限流负极继电器
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //打开dc源1
                parametersSending.ChargeCurrentLimitNegativeRelayStatus = 1;
                succeed = OpenParameter(parametersSending.ChargeCurrentLimitNegativeRelayStatus, "ChargeCurrentLimitNegativeRelayStatus", "充电限流负极继电器");
                Thread.Sleep(1000);
            } while (!succeed);

            Thread.Sleep(2000);

            //判断条件
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    AddLog($"获取前置条件失败！");
                    return false;
                }
                BMS_Receive = SendPacked(parametersSending);
                if (BMS_Receive != null)
                {
                    succeed = true;
                }
                Thread.Sleep(1000);
            } while (!succeed);

            if (!(BMS_Receive.ChargeRelayStatus == 1 && BMS_Receive.DischargeRelayStatus == 0 && BMS_Receive.ResistorBankMosfetStatus == 0 && BMS_Receive.ChargeCurrentLimitNegativeRelayStatus == 1 && BMS_Receive.Reserved1 == 0))
            {
                AddLog($"前置条件不符！");

                if (BMS_Receive == null)
                {
                    AddLog("返回空");
                    return false;
                }
                if(BMS_Receive.ChargeRelayStatus != 1)
                {
                    AddLog("充电继电器没开启");
                }
                if (BMS_Receive.DischargeRelayStatus != 0)
                {
                    AddLog("放电继电器没关闭");
                }
                if (BMS_Receive.ResistorBankMosfetStatus != 0)
                {
                    AddLog("电阻棒MOS管没关闭");
                }
                if (BMS_Receive.ChargeCurrentLimitNegativeRelayStatus!=1)
                {
                    AddLog("充电限流负极继电器没开启");
                }
                if (BMS_Receive.Reserved1 != 0)
                {
                    AddLog("电子负载没关闭");
                }
                
                string charge = BMS_Receive.ChargeRelayStatus != 1 ? "充电继电器没开启" : "";
                string disCharge = BMS_Receive.DischargeRelayStatus != 0 ? "放电继电器没关闭" : "";
                string resistorBankMosfetStatus = BMS_Receive.ResistorBankMosfetStatus != 0 ? "电阻棒MOS管没关闭" : "";
                string chargeCurrentLimitNegativeRelayStatus = BMS_Receive.ChargeCurrentLimitNegativeRelayStatus != 1 ? "充电限流负极继电器没开启" : "";
                string relay1Status = BMS_Receive.Reserved1 != 0 ? "电子负载没关闭" : "";
                
                //AddLog($"{charge}");
                //AddLog($"{disCharge}");
                //AddLog($"{resistorBankMosfetStatus}");
                //AddLog($"{chargeCurrentLimitNegativeRelayStatus}");
                //AddLog($"{relay1Status}");
                return false;
            }

            //第七步 打开电阻棒MOS管
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //打开电阻棒MOS管
                parametersSending.ResistorBankMosfetStatus = 1;
                succeed = OpenParameter(parametersSending.ResistorBankMosfetStatus, "ResistorBankMosfetStatus", "电阻棒MOS管");
                Thread.Sleep(500);
            } while (!succeed);
            Thread.Sleep(2000);


            //第八步 采集当前dc2的电流 
            int firstCurrent = 0;
            failCount = 0;
            succeed = false;
            AddLog("采集限流前dc2电流");
            do
            {
                if (failCount++ == 10)
                {
                    AddLog("限流前电流电流达不到指定区间");
                    return false;
                }
                BMS_Receive = SendPacked(parametersSending);

                ushort bms_current = GetCurrentFormBMS();
                if (BMS_Receive != null)
                {
                    testData.LimitDc2Source = BMS_Receive.DcSource2Current;
                    testData.LimitBmsSource = bms_current;
                    if (BMS_Receive.DcSource2Current >= 2500 && bms_current >= 2500)
                    {
                        succeed = true;
                    }
                    firstCurrent = BMS_Receive.DcSource2Current;
                    AddLog($"限流前dc2电流:{firstCurrent}A,BMS电流:{bms_current}");

                }
                Thread.Sleep(1000);
            } while (!succeed);

            //第九步 打开限流开关

            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    AddLog("限流开关开启失败");
                    return false;
                }
                //打开限流开关
                succeed = OpenLimitedCurrent();
                Thread.Sleep(100);
            } while (!succeed);
            AddLog("限流开关已打开");

            Thread.Sleep(2000);

            //第十步 第二次采集dc2的电流
            int secondCurrent = 0;
            failCount = 0;
            succeed = false;
            AddLog("采集限流后dc2电流");
            do
            {
                if (failCount++ == 10)
                {
                    AddLog("限流开启后，电流不能下降到指定区间");
                    return false;
                }
                BMS_Receive = SendPacked(parametersSending);
                ushort bms_SecondCurrent = GetCurrentFormBMS();
                if (BMS_Receive != null)
                {
                    testData.LimitDc2SourceResult = BMS_Receive.DcSource2Current;
                    testData.LimitBmsSourceResult = bms_SecondCurrent;
                    if (BMS_Receive.DcSource2Current <= 1200 && BMS_Receive.DcSource2Current >= 800)
                    {
                        if (bms_SecondCurrent <= 1200 && bms_SecondCurrent >= 800)
                        {
                            Thread.Sleep(2000);
                            succeed = true;
                        }
                    }
                    secondCurrent = BMS_Receive.DcSource2Current;
                    AddLog($"限流后dc2电流:{secondCurrent}A");
                }
                Thread.Sleep(1000);
            } while (!succeed);



            //结束充电限流
            //关闭电阻棒MOS管
            failCount = 0;
            succeed = false;
            AddLog("充电限流校验结束，正在关闭相关元件");
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                parametersSending.ResistorBankMosfetStatus = 0;
                succeed = CloseParameter(parametersSending.ResistorBankMosfetStatus, "ResistorBankMosfetStatus", "电阻棒MOS管");
                Thread.Sleep(1000);
            } while (!succeed);
            Thread.Sleep(1000);

            //关闭充电负极限流继电器
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                parametersSending.ChargeCurrentLimitNegativeRelayStatus = 0;
                succeed = CloseParameter(parametersSending.ChargeCurrentLimitNegativeRelayStatus, "ChargeCurrentLimitNegativeRelayStatus", "充电限流负极继电器");
                Thread.Sleep(1000);
            } while (!succeed);

            failCount = 0;
            succeed = false;
            //关闭充电继电器
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                //关闭充电继电器
                parametersSending.ChargeRelayStatus = 0;
                parametersSending.ElectronicLoadCurrent = 1;
                succeed = CloseParameter(parametersSending.ChargeRelayStatus, "ChargeRelayStatus", "充电继电器");
                Thread.Sleep(1000);
            } while (!succeed);

            //关闭限流开关
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                succeed = CloseLimitedCurrent();
            } while (!succeed);

            Thread.Sleep(1000);
            parametersSending.DcSource1Current = 100;
            parametersSending.DcSource2Current = 100;
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    return false;
                }
                BMS_Receive = SendPacked(parametersSending);
                if (BMS_Receive != null)
                {

                    succeed = true;

                }
                Thread.Sleep(1000);
            } while (!succeed);

            //测试成功清除历史记录
            ClearHistoryAfterTest();

            return true;
        }

        /// <summary>
        /// 退出测试模式(关闭相应的电子元件)
        /// </summary>
        /// <returns></returns>
        public bool ExitTestMode()
        {

            AddLog("正在执行停止程序，关闭相关继电器");
            int failCount = 0;
            bool succeed = false;
            parametersSending.DcSource1Switch = 0;
            parametersSending.DcSource2Switch = 0;
            parametersSending.Reserved2 = 0;
            parametersSending.Reserved3 = 0;
            parametersSending.ElectronicLoadCurrent = 1;

            //关闭限流
            failCount = 0;
            succeed = false;
            do
            {
                if (failCount++ == 10)
                {
                    AddLog("关闭限流失败");
                    break;
                }
                succeed = CloseLimitedCurrent();
                Thread.Sleep(200);
            } while (!succeed);
            AddLog("限流已关闭");

            BMS_Receive = SendPacked(parametersSending);

            if (BMS_Receive?.ResistorBankMosfetStatus == 1)
            {
                //关闭电阻棒
                do
                {
                    if (failCount++ == 10)
                    {
                        break;
                    }
                    //关闭电阻棒MOS管
                    parametersSending.ResistorBankMosfetStatus = 0;
                    succeed = CloseParameter(parametersSending.ResistorBankMosfetStatus, "ResistorBankMosfetStatus", "电阻棒MOS管");
                    Thread.Sleep(1000);
                } while (!succeed);
                if (succeed == false)
                {
                    AddLog("关闭电阻棒MOS失败，请检查硬件是否损坏！");
                    LShowMessage("关闭电阻棒MOS失败，请检查硬件是否损坏！", "警告", MessageIcon.Error);
                }
            }

            if (BMS_Receive?.Reserved1 == 1)
            {
                //关闭电子负载继电器
                failCount = 0;
                succeed = false;
                do
                {
                    if (failCount++ == 10)
                    {
                        break;
                    }
                    //关闭电子负载开关
                    parametersSending.Reserved1 = 0;
                    succeed = CloseParameter(parametersSending.Reserved1, "Reserved1", "电子负载开关");
                    Thread.Sleep(1000);
                } while (!succeed);
                if (succeed == false)
                {
                    AddLog("关闭电子负载继电器失败，请检查硬件是否损坏！");
                    LShowMessage("关闭电子负载继电器失败，请检查硬件是否损坏！", "警告", MessageIcon.Error);
                }
            }


            if (BMS_Receive?.DischargeRelayStatus == 1)
            {
                //关闭放电继电器
                failCount = 0;
                succeed = false;
                do
                {
                    if (failCount++ == 10)
                    {
                        break;
                    }
                    //关闭放电继电器
                    parametersSending.DischargeRelayStatus = 0;
                    succeed = CloseParameter(parametersSending.DischargeRelayStatus, "DischargeRelayStatus", "放电继电器");
                    Thread.Sleep(1000);
                } while (!succeed);
                if (succeed == false)
                {
                    AddLog("关闭放电继电器失败，请检查硬件是否损坏！");
                    LShowMessage("关闭放电继电器失败，请检查硬件是否损坏！", "警告", MessageIcon.Error);
                }
            }

            if (BMS_Receive?.ChargeRelayStatus == 1)
            {
                //关闭充电继电器
                failCount = 0;
                succeed = false;
                do
                {
                    if (failCount++ == 10)
                    {
                        break;
                    }
                    //关闭充电继电器
                    parametersSending.ChargeRelayStatus = 0;
                    succeed = CloseParameter(parametersSending.ChargeRelayStatus, "ChargeRelayStatus", "充电继电器");
                    Thread.Sleep(1000);
                } while (!succeed);
                if (succeed == false)
                {
                    AddLog("关闭充电继电器失败，请检查硬件是否损坏！");
                    LShowMessage("关闭充电继电器失败，请检查硬件是否损坏！", "警告", MessageIcon.Error);
                }
            }

            if (BMS_Receive?.ChargeCurrentLimitNegativeRelayStatus == 1)
            {
                //关闭充电限流负极继电器
                failCount = 0;
                succeed = false;
                do
                {
                    if (failCount++ == 10)
                    {
                        break;
                    }
                    //关闭充电限流负极继电器
                    parametersSending.ChargeCurrentLimitNegativeRelayStatus = 0;
                    succeed = CloseParameter(parametersSending.ChargeCurrentLimitNegativeRelayStatus, "ChargeCurrentLimitNegativeRelayStatus", "充电限流负极继电器");
                    Thread.Sleep(1000);
                } while (!succeed);
                if (succeed == false)
                {
                    AddLog("关闭充电限流负极继电器失败，请检查硬件是否损坏！");
                    LShowMessage("关闭充电限流负极继电器失败，请检查硬件是否损坏！", "警告", MessageIcon.Error);
                }
            }



            parametersSending.LowPowerRelayStatus = 0;
            parametersSending.Reserved1RelayStatus = 0;
            parametersSending.Reserved2RelayStatus = 0;
            parametersSending.Reserved3RelayStatus = 0;


            BMS_Receive = SendPacked(parametersSending);
            Thread.Sleep(1000);
            BMS_Receive = SendPacked(parametersSending);

            if (BMS_Receive?.LowPowerRelayStatus == 1)
            {
                //关闭低功耗继电器
                failCount = 0;
                succeed = false;
                do
                {
                    if (failCount++ == 10)
                    {
                        break;
                    }
                    succeed = LowPowerVoltageAndCurrent(0);
                    Thread.Sleep(1000);
                } while (!succeed);
                if (succeed == false)
                {
                    AddLog("关闭低功耗继电器失败，请检查硬件是否损坏！");
                    LShowMessage("关闭低功耗继电器失败，请检查硬件是否损坏！", "警告", MessageIcon.Error);
                }
            }

            if (BMS_Receive?.Reserved1RelayStatus == 1)
            {
                //关闭预留继电器2
                failCount = 0;
                succeed = false;
                do
                {
                    if (failCount++ == 10)
                    {
                        break;
                    }
                    succeed = Relay2ControlTest(0);
                    Thread.Sleep(1000);
                } while (!succeed);
                if (succeed == false)
                {
                    AddLog("关闭关闭预留继电器2失败，请检查硬件是否损坏！");
                    LShowMessage("关闭关闭预留继电器2失败，请检查硬件是否损坏！", "警告", MessageIcon.Error);
                }
            }

            if (BMS_Receive?.Reserved2RelayStatus == 1)
            {
                //关闭预留继电器3
                failCount = 0;
                succeed = false;
                do
                {
                    if (failCount++ == 10)
                    {
                        break;
                    }
                    succeed = Relay3ControlTest(0);
                    Thread.Sleep(1000);
                } while (!succeed);
                if (succeed == false)
                {
                    AddLog("关闭关闭预留继电器3失败，请检查硬件是否损坏！");
                    LShowMessage("关闭关闭预留继电器3失败，请检查硬件是否损坏！", "警告", MessageIcon.Error);
                }
            }

            if (BMS_Receive?.Reserved3RelayStatus == 1)
            {
                //关闭预留继电4
                failCount = 0;
                succeed = false;
                do
                {
                    if (failCount++ == 10)
                    {
                        break;
                    }
                    succeed = Relay4ControlTest(0);
                    Thread.Sleep(1000);

                } while (!succeed);
                if (succeed == false)
                {
                    AddLog("关闭关闭预留继电器4失败，请检查硬件是否损坏！");
                    LShowMessage("关闭关闭预留继电器4失败，请检查硬件是否损坏！", "警告", MessageIcon.Error);
                }
            }
            AddLog("停止程序已执行完");

            return true;
        }

        /// <summary>
        /// 关闭或打开低功耗继电器
        /// </summary>
        /// <returns></returns>
        private bool LowPowerVoltageAndCurrent(ushort open)
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //低功耗继电器控制打开
            parametersSending.LowPowerRelayStatus = open;

            int failCount = 0;
            bool succeed = false;
            if (open == 0)
            {
                do
                {
                    if (failCount++ == 10)
                    {
                        return false;
                    }
                    //关闭充电继电器
                    succeed = CloseParameter(parametersSending.LowPowerRelayStatus, "LowPowerRelayStatus", "低功耗继电器");
                    Thread.Sleep(500);
                } while (!succeed);
            }
            else
            {
                do
                {
                    if (failCount++ == 10)
                    {
                        return false;
                    }
                    //关闭充电继电器

                    succeed = OpenParameter(parametersSending.LowPowerRelayStatus, "LowPowerRelayStatus", "低功耗继电器");
                    Thread.Sleep(500);
                } while (!succeed);
            }
            return true;
        }

        /// <summary>
        /// 预留继电器1
        /// </summary>
        /// <returns></returns>
        private bool Relay2ControlTest(ushort open)
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //预留1耗继电器控制打开
            parametersSending.Reserved1RelayStatus = open;

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.Reserved1RelayStatus != open)
                    {
                        AddLog("预留继电器1操作失败");
                    }
                    else
                    {
                        AddLog($"预留继电器1操作成功");
                        return true;
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }

        /// <summary>
        /// 预留继电器2
        /// </summary>
        /// <returns></returns>
        private bool Relay3ControlTest(ushort open)
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //低功耗继电器控制打开
            parametersSending.Reserved2RelayStatus = open;

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.Reserved2RelayStatus != open)
                    {
                        AddLog("预留继电器2操作失败");
                    }
                    else
                    {
                        AddLog($"预留继电器2操作成功");
                        return true;
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }

        /// <summary>
        /// 预留继电器3
        /// </summary>
        /// <returns></returns>
        private bool Relay4ControlTest(ushort open)
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //低功耗继电器控制打开
            parametersSending.Reserved3RelayStatus = open;

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.Reserved3RelayStatus != open)
                    {
                        AddLog("预留继电器3操作失败");
                    }
                    else
                    {
                        AddLog($"预留继电器3操作成功");
                        return true;
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }




        #region DC源

        /// <summary>
        /// DC电源开关
        /// </summary>
        private ushort dcSourceSwitch;

        public ushort DcSourceSwitch
        {
            get { return dcSourceSwitch; }
            set
            {
                dcSourceSwitch = value;
                this.RaiseProperChanged(nameof(DcSourceSwitch));
            }
        }

        /// <summary>
        /// 打开DC源开关
        /// </summary>
        /// <returns></returns>
        private bool OpenDcSourceSwitch()
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //DC控制开关控制打开
            parametersSending.DcSource1Switch = 1;
            parametersSending.DcSource1Current = (ushort)(DcSourceControlCurrent * 100);
            parametersSending.DcSource1Voltage = (ushort)(DcSourceControlVoltage * 100);

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.DcSource1Switch != 1)
                    {
                        AddLog("DC控制开关打开失败");
                        DcSourceSwitch = 0;
                    }
                    else
                    {
                        DcSourceSwitch = 1;
                        AddLog($"DC控制开关打开成功");
                        return true;
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }

        /// <summary>
        /// 关闭DC源开关
        /// </summary>
        /// <returns></returns>
        private bool CloseDcSourceSwitchTest()
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //DC控制开关控制打开
            parametersSending.DcSource1Switch = 0;
            parametersSending.DcSource1Current = (ushort)(DcSourceControlCurrent * 100);
            parametersSending.DcSource1Voltage = (ushort)(DcSourceControlVoltage * 100);
            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.DcSource1Switch != 0)
                    {
                        AddLog("DC控制开关关闭失败");
                    }
                    else
                    {
                        DcSourceSwitch = 0;
                        AddLog($"DC控制开关关闭成功");
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }


        /// <summary>
        /// DC源控制电压
        /// </summary>
        private float _DcSourceControlVoltage;

        public float DcSourceControlVoltage
        {
            get { return _DcSourceControlVoltage; }
            set
            {
                _DcSourceControlVoltage = value;
                this.RaiseProperChanged(nameof(_DcSourceControlVoltage));
            }
        }

        /// <summary>
        /// DC源2控制电压
        /// </summary>
        private float _DcSource2ControlVoltage;

        public float DcSource2ControlVoltage
        {
            get { return _DcSource2ControlVoltage; }
            set
            {
                _DcSource2ControlVoltage = value;
                this.RaiseProperChanged(nameof(_DcSource2ControlVoltage));
            }
        }


        /// <summary>
        /// 设置DC源控制电压
        /// </summary>
        /// <returns></returns>
        private bool DcSourceControlVoltageTest()
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //设置DC源控制电压
            parametersSending.DcSourceControlVoltage = (ushort)DcSourceControlVoltage;

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 65);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.DcSourceVoltage != DcSourceControlVoltage / 10)//发1200，实际12，返回120
                    {
                        AddLog("设置DC源控制电压失败");
                    }
                    else
                    {
                        AddLog($"设置DC源控制电压成功");
                        return true;
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }


        /// <summary>
        /// DC源控制电流
        /// </summary>
        private float _DcSourceControlCurrent;

        public float DcSourceControlCurrent
        {
            get { return _DcSourceControlCurrent; }
            set
            {
                _DcSourceControlCurrent = value;
                this.RaiseProperChanged(nameof(DcSourceControlCurrent));
            }
        }

        /// <summary>
        /// DC源控制电流
        /// </summary>
        private float _DcSource2ControlCurrent;

        public float DcSource2ControlCurrent
        {
            get { return _DcSource2ControlCurrent; }
            set
            {
                _DcSource2ControlCurrent = value;
                this.RaiseProperChanged(nameof(DcSource2ControlCurrent));
            }
        }

        /// <summary>
        /// 设置DC源控制电流
        /// </summary>
        /// <returns></returns>
        private bool DcSourceControlCurrentTest()
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //设置DC源控制电压
            parametersSending.DcSourceControlCurrent = (ushort)DcSourceControlCurrent;

            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 65);

            //解析
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.DcSourceCurrent != DcSourceControlCurrent)//实际0.1，返回10
                    {
                        AddLog("DC源电流设置失败");
                    }
                    else
                    {
                        AddLog($"DC源电流设置成功");
                        return true;
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }

        #endregion

        #region 直流源1

        /// <summary>
        /// 直流源1开关
        /// </summary>
        private ushort dcSource1Switch;

        public ushort DcSource1Switch
        {
            get { return dcSource1Switch; }
            set
            {
                dcSource1Switch = value;
                this.RaiseProperChanged(nameof(DcSource1Switch));
            }
        }

        /// <summary>
        /// 打开直流源1开关
        /// </summary>
        /// <returns></returns>
        private bool OpenDcSource1Switch()
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //DC控制开关控制打开
            parametersSending.DcSource1Switch = 1;
            //发送字符串 、解析
            BMS_Receive = SendPacked(parametersSending);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.DcSource1Switch != 1)
                    {
                        AddLog("直流源1开关打开失败");
                        DcSource1Switch = 0;
                    }
                    else
                    {
                        DcSource1Switch = 1;
                        AddLog($"直流源1开关打开成功");
                        return true;
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }

        /// <summary>
        /// 关闭直流源1开关
        /// </summary>
        /// <returns></returns>
        private bool CloseDcSource1SwitchTest()
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //DC控制开关控制打开
            parametersSending.DcSource1Switch = 0;
            //解析
            BMS_Receive = SendPacked(parametersSending);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.DcSource1Switch != 0)
                    {
                        AddLog("DC控制开关关闭失败");
                        DcSource1Switch = 1;
                    }
                    else
                    {
                        DcSource1Switch = 0;
                        AddLog($"DC控制开关关闭成功");
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }


        #endregion

        #region 直流源2

        /// <summary>
        /// 直流源2开关
        /// </summary>
        private ushort dcSource2Switch;

        public ushort DcSource2Switch
        {
            get { return dcSource2Switch; }
            set
            {
                dcSource2Switch = value;
                this.RaiseProperChanged(nameof(DcSource2Switch));
            }
        }

        /// <summary>
        /// 打开直流源2开关
        /// </summary>
        /// <returns></returns>
        private bool OpenDcSource2Switch()
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //DC控制开关控制打开
            parametersSending.DcSource2Switch = 1;
            parametersSending.DcSource2Current = (ushort)(DcSource2ControlCurrent * 100);
            parametersSending.DcSource2Voltage = (ushort)(DcSource2ControlVoltage * 100);
            //发送字符串 、解析
            BMS_Receive = SendPacked(parametersSending);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.DcSource2Switch != 1)
                    {
                        AddLog("直流源1开关打开失败");
                        DcSource2Switch = 0;
                    }
                    else
                    {
                        DcSource2Switch = 1;
                        AddLog($"直流源1开关打开成功");
                        return true;
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }

        /// <summary>
        /// 关闭直流源2开关
        /// </summary>
        /// <returns></returns>
        private bool CloseDcSource2SwitchTest()
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //DC控制开关控制打开
            parametersSending.DcSource2Switch = 0;
            parametersSending.DcSource2Current = (ushort)(DcSource2ControlCurrent * 100);
            parametersSending.DcSource2Voltage = (ushort)(DcSource2ControlVoltage * 100);
            //解析
            BMS_Receive = SendPacked(parametersSending);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.DcSource2Switch != 0)
                    {
                        AddLog("直流源2关闭失败");
                        DcSource2Switch = 1;
                    }
                    else
                    {
                        DcSource2Switch = 0;
                        AddLog($"直流源2关闭成功");
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }


        #endregion

        #region 电子负载
        /// <summary>
        /// 电子负载开
        /// </summary>
        /// <returns></returns>
        private bool OpenElectronicLoadMode()
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //DC控制开关控制打开
            parametersSending.ElectronicLoadMode = 1;

            //发送字符串 、解析
            BMS_Receive = SendPacked(parametersSending);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.ElectronicLoadMode != 1)
                    {
                        AddLog("电子负载模式打开失败");

                    }
                    else
                    {

                        AddLog($"电子负载模式打开成功");
                        return true;
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }

        /// <summary>
        /// 电子负载关
        /// </summary>
        /// <returns></returns>
        private bool CloseElectronicLoadMode()
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //DC控制开关控制打开
            parametersSending.ElectronicLoadMode = 0;
            //解析
            BMS_Receive = SendPacked(parametersSending);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.ElectronicLoadMode != 0)
                    {
                        AddLog("电子负载模式关闭失败");

                    }
                    else
                    {

                        AddLog($"电子负载模式关闭成功");
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }

        #endregion

        #region 通用开关方法
        /// <summary>
        /// 发送打开属性方法
        /// </summary>
        /// <param name="parameter">属性</param>
        /// <param name="name">属性名</param>
        /// <param name="msg">中文描述</param>
        /// <returns></returns>
        private bool OpenParameter(ushort parameter, string name, string msg)
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //打开
            parameter = 1;
            //发送字符串 、解析
            BMS_Receive = SendPacked(parametersSending);

            //判断
            if (BMS_Receive == null)
            {
                AddLog($"{msg}打开失败");
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.GetValueByName(name) != 1)
                    {
                        AddLog($"{msg}打开失败");

                    }
                    else
                    {

                        AddLog($"{msg}打开成功");
                        return true;
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }

        /// <summary>
        /// 发送打开属性方法
        /// </summary>
        /// <param name="parameter">属性</param>
        /// <param name="name">属性名</param>
        /// <param name="msg">中文描述</param>
        private bool CloseParameter(ushort parameter, string name, string msg)
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;

            parameter = 0;
            //发送、解析
            BMS_Receive = SendPacked(parametersSending);

            //判断
            if (BMS_Receive == null)
            {
                AddLog($"{msg}关闭失败");
                return false;
            }
            else
            {
                //判断是否是普通模式
                if (BMS_Receive.TestMode == 0)
                {
                    if (BMS_Receive.GetValueByName(name) != 0)
                    {
                        AddLog($"{msg}关闭失败");

                    }
                    else
                    {

                        AddLog($"{msg}关闭成功");
                        return true;
                    }
                }
                else
                {
                    //不是普通模式
                    AddLog("返回普通模式失败");
                }
            }
            return false;
        }


        #endregion


        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="bmsSending"></param>
        /// <returns></returns>
        private BmsSystemparametersReceive SendPacked(BmsSystemParametersSending bmsSending)
        {
            //拼接字符串
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, bmsSending.ToByteArray());
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack));

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 77);

            //解析
            BmsSystemparametersReceive bmsReceive = AnalyseBmsReceive(result);

            //充放电电流为2393，打印
            if(bmsReceive!=null && bmsReceive.ElectronicLoadCurrent == 2393)
            {
                string hex = BitConverter.ToString(result);
                AddLog($"电流为2393：字节数组{hex}");
            }
            return bmsReceive;
        }


        /// <summary>
        /// 解析返回数据
        /// </summary>
        /// <param name="result">数据报文</param>
        /// <returns>指令实体类(null代表异常)</returns>
        private BmsSystemparametersReceive AnalyseBmsReceive(byte[] result)
        {
            //解析字符串
            if (result.Length == 0)
            {
                AddLog("空字节异常");
                return null;
            }
            else if (result.Length == 1)
            {
                if (result[0] == 0x01)
                {
                    AddLog("返回报文CRC校验不通过");
                }
                else if (result[0] == 0x02)
                {
                    AddLog("返回报文超时");
                }
                return null;
            }
            else if (result.Length == 72)
            {
                BmsSystemparametersReceive bms = BmsSystemparametersReceive.FromByteArray(result);
                return bms;
            }
            return null;
        }

        #endregion

        #region 串口二(bms板)

        /// <summary>
        /// 进入测试模式
        /// </summary>
        /// <returns></returns>
        private bool EnterTestMode()
        {
            //进入测试模式1
            bool interSuccess = false;
            int ERROR_COUNT = 0;
            do
            {
                ERROR_COUNT++;
                string receive = SerialCommunicationService2.SendCommand("ENTERTESTMODE", 13);
                if (receive.Equals("ENTERTESTMODE"))
                    interSuccess = true;
                //interSuccess = InterTestMode(1);
            } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

            if (!interSuccess)
            {
                AddLog("进入测试模式失败");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 测试前写入激活
        /// </summary>
        /// <returns></returns>
        private bool WriteBeforeTest()
        {
            bool isActive = false;
            //写入参数
            bool interSuccess = false;
            int ERROR_COUNT = 0;
            AddLog("查看是否激活");
            do
            {
                ERROR_COUNT++;
                byte[] receive = SerialCommunicationService2.SendTestCommand(new byte[] { 0x01, 0x03, 0x01, 0x40, 0x00, 0x01, 0x84, 0x22 }, 7);
                if (receive.Length == 1)
                {
                    if (receive[0] == 1)
                        AddLog("CRC检验不通过");
                    else
                        AddLog("超时返回");
                }
                else if (receive.Length == 2 && receive[0] == 0x10 && receive[1] == 0x00)
                {
                    interSuccess = true;
                    testData.IsActivate = "已激活";
                    isActive = true;
                    AddLog("已激活");
                }
                else
                {
                    AddLog("未激活");
                    testData.IsActivate = "未激活";
                    isActive = false;
                    interSuccess = true;
                }
                //interSuccess = InterTestMode(1);
                Thread.Sleep(500);
            } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

            if (!interSuccess)
            {
                AddLog("未识别是否激活");
                return false;
            }
            return isActive;
        }

        /// <summary>
        /// 测试前重置参数
        /// </summary>
        /// <returns></returns>
        private bool ResetBeforeTest()
        {
            //写入参数
            bool interSuccess = false;
            int ERROR_COUNT = 0;
            AddLog("正在重置参数");
            //重置参数
            //发 01 20 00 02 00 01 e0 0d
            do
            {
                ERROR_COUNT++;
                byte[] receive = SerialCommunicationService2.SendTestCommand2(new byte[] { 0x01, 0x20, 0x00, 0x02, 0x00, 0x01, 0xe0, 0x0d }, 8);
                if (receive.Length == 0)
                {
                    AddLog("重置参数：指令返回异常");
                }
                else if (receive.Length == 1 && receive[0] == 1)
                {
                    AddLog("重置参数：返回校验码不合");
                }
                else if ((receive[0] == 2))
                {
                    AddLog("重置参数：返回超时");
                }
                if (receive.Length == 8)
                    interSuccess = true;
                //interSuccess = InterTestMode(1);
                Thread.Sleep(200);
            } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

            if (!interSuccess)
            {
                AddLog("重置参数失败");
                return false;
            }
            AddLog("重置参数成功");
            return true;
        }

        /// <summary>
        /// 清除历史记录
        /// </summary>
        /// <returns></returns>
        private bool ClearHistoryAfterTest()
        {
            //写入参数
            bool interSuccess = false;
            int ERROR_COUNT = 0;
            AddLog("正在清除历史记录");
            //重置参数
            //发 01 20 00 02 00 01 e0 0d
            do
            {
                ERROR_COUNT++;
                byte[] receive = SerialCommunicationService2.SendTestCommand2(new byte[] { 0x01, 0x20, 0x00, 0x03, 0x00, 0x01, 0xB1, 0xCD }, 8);
                if (receive.Length == 0)
                {
                    AddLog("重置参数：指令返回异常");
                }
                else if (receive.Length == 1 && receive[0] == 1)
                {
                    AddLog("重置参数：返回校验码不合");
                }
                else if ((receive[0] == 2))
                {
                    AddLog("重置参数：返回超时");
                }
                if (receive.Length == 8)
                    interSuccess = true;
                //interSuccess = InterTestMode(1);
                Thread.Sleep(200);
            } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

            if (!interSuccess)
            {
                AddLog("清除历史记录失败");
                return false;
            }
            AddLog("清除历史记录成功");
            return true;
        }

        /// <summary>
        /// 读取系统时间
        /// </summary>
        /// <returns></returns>
        private bool ReadTime()
        {
            //写入参数
            bool interSuccess = false;
            int ERROR_COUNT = 0;
            //发 01 03 00 6e 00 03 64 16
            //收 01 03 06 19 08 07 0E 11 2A 2F 3F
            do
            {
                ERROR_COUNT++;
                byte[] receive = SerialCommunicationService2.SendTestCommand2(new byte[] { 0x01, 0x03, 0x00, 0x6e, 0x00, 0x03, 0x64, 0x16 }, 11);
                if (receive.Length == 0)
                {
                    AddLog("读取时间：指令返回异常");
                }
                else if (receive.Length == 1 && receive[0] == 1)
                {
                    AddLog("读取时间：返回校验码不合");
                }
                else if ((receive[0] == 2))
                {
                    AddLog("读取时间：返回超时");
                }
                if (receive.Length == 11)
                {
                    interSuccess = true;
                    testData.Time = $"20{receive[3]}/{receive[4]}/{receive[5]}  {receive[6]}:{receive[7]}:{receive[8]}";
                }

                //interSuccess = InterTestMode(1);
                Thread.Sleep(200);
            } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

            if (!interSuccess)
            {
                AddLog("读取时间失败");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 测试后写入系统时间时间
        /// </summary>
        /// <returns></returns>
        private bool WriteTimeAfterTest()
        {
            AddLog("正在写入系统时间");
            //写入参数
            bool interSuccess = false;
            int ERROR_COUNT = 0;
            //获取当前时间，转换成字节格式
            byte[] head = new byte[] { 0x01, 0x10, 0x00, 0x6e, 0x00, 0x03, 0x06 };
            byte[] writeTime = GetFormattedDateTime();
            byte[] readTime = Tools.CommunicateTool.ConcatByteArrays(head, writeTime);
            byte[] crc16 = SerialCommunicationService2.getCRC16(readTime);
            readTime = CommunicateTool.ConcatByteArrays(readTime, crc16);
            do
            {
                ERROR_COUNT++;
                byte[] receive = SerialCommunicationService2.SendTestCommand2(readTime, 15);
                if (receive.Length == 0)
                {
                    AddLog("写入时间：写入时间返回异常");
                }
                if (receive.Length == 15)
                    interSuccess = true;
                //interSuccess = InterTestMode(1);
                Thread.Sleep(200);
            } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

            if (!interSuccess)
            {
                AddLog("写入时间失败");
                return false;
            }
            AddLog("写入成功");
            return true;
        }

        /// <summary>
        /// 测试后写入剩余容量
        /// </summary>
        /// <returns></returns>
        private bool WriteSOC()
        {
            AddLog("正在写入剩余容量");
            //写入参数
            bool interSuccess = false;
            int ERROR_COUNT = 0;
            //获取当前时间，转换成字节格式
            byte[] com = new byte[] { 0x01, 0x10, 0x00, 0xFC, 0x00, 0x04, 0x08, 0x27, 0x10, 0x27, 0x10, 0x0B, 0xB8, 0x00, 0x00, 0xF2, 0x41 };
            do
            {
                ERROR_COUNT++;
                byte[] receive = SerialCommunicationService2.SendTestCommand2(com, 17);
                if (receive.Length == 0)
                {
                    AddLog("写入剩余容量：写入剩余容量返回异常");
                }
                if (receive.Length == 17)
                    interSuccess = true;
                //interSuccess = InterTestMode(1);
                Thread.Sleep(200);
            } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

            if (!interSuccess)
            {
                AddLog("写入剩余容量失败");
                return false;
            }
            AddLog("写入剩余容量成功");
            return true;
        }

        /// <summary>
        /// 读取【版本号】【硬件版本】【生产日期】
        /// </summary>
        /// <returns></returns>
        private bool ReadThreeData()
        {
            AddLog("读取【版本号】【硬件版本】【生产日期】");
            //写入参数
            bool interSuccess = false;
            int ERROR_COUNT = 0;
            //发：01 03 01 1b 00 0e b5 f5
            //返：第4 5 6 7个字节是软件版本号
            //   8 9 10 11是硬件版本  16 17 18 19 字节是生产日期
            //收 01 03 1C 【0A 00 03 00 】【0A 00 01 00】 00 00 01 00 【35 01 5A 00】 00 00 01 00 00 00 01
            //            (10)(0)(3)(0)(10)(0)(1)(0)(53)(1)(90)(0)     (0135005A)
            //00 00 00 01 00 87 CC （33字节）
            do
            {
                ERROR_COUNT++;
                byte[] receive = SerialCommunicationService2.SendTestCommand2(new byte[] { 0x01, 0x03, 0x01, 0x1b, 0x00, 0x0e, 0xb5, 0xf5 }, 33);
                if (receive.Length == 0)
                {
                    AddLog("读取【版本号】【硬件版本】【生产日期】：指令返回异常");
                }
                else if (receive.Length == 1 && receive[0] == 1)
                {
                    AddLog("读取【版本号】【硬件版本】【生产日期】：返回校验码不合");
                }
                else if ((receive[0] == 2))
                {
                    AddLog("读取【版本号】【硬件版本】【生产日期】：返回超时");
                }
                if (receive.Length == 33)
                {
                    interSuccess = true;

                    //软件版本
                    testData.SoftwareVersion = ByteConverter.BytesToNumber(new byte[] { receive[3], receive[4] }).ToString("D2") + ByteConverter.BytesToNumber(new byte[] { receive[5], receive[6] }).ToString("D2");

                    //硬件版本
                    testData.HardwareWareVersion = ByteConverter.BytesToNumber(new byte[] { receive[7], receive[8] }).ToString("D2") + ByteConverter.BytesToNumber(new byte[] { receive[9], receive[10] }).ToString("D2");
                    //出厂日期
                    //testData.ProductDate = ByteConverter.BytesToDateNumber(new byte[] { receive[15], receive[16], receive[17], receive[18] }).ToString();
                    AddLog($"【软件版本】：{testData.SoftwareVersion}");
                    AddLog($"【硬件版本】：{testData.HardwareWareVersion}");
                    //AddLog($"【出厂日期】：{testData.ProductDate}");
                    if (testData.TestSofterWare == testData.SoftwareVersion)
                    {
                        return true;
                    }

                }

                //interSuccess = InterTestMode(1);
                Thread.Sleep(200);
            } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

            if (!interSuccess)
            {
                AddLog("读取失败");
                return false;
            }
            return true;
        }


        /// <summary>
        /// 读取电芯电压
        /// </summary>
        /// <returns></returns>
        private bool ReadDianxinVoltage()
        {
            AddLog("读取电芯电压");
            //写入参数
            bool interSuccess = false;
            int ERROR_COUNT = 0;
            //发：01 03 00 02 00 10 e5 c6
            //返：01 03 20 6C 0B 1B 0C 1F 0C 29 0C C0 0C 28 0C 10 0C 4F 0C 49 0C 56 0C 3E 0C 24 0C 2E 0C 32 0C 2E 0C 90 0C C4 EC
            do
            {
                ERROR_COUNT++;
                byte[] receive = SerialCommunicationService2.SendTestCommand2(new byte[] { 0x01, 0x03, 0x00, 0x02, 0x00, 0x10, 0xe5, 0xc6 }, 37);
                if (receive.Length == 0)
                {
                    AddLog("读取电芯电压：指令返回异常");
                }
                else if (receive.Length == 1 && receive[0] == 1)
                {
                    AddLog("读取电芯电压：返回校验码不合");
                }
                else if ((receive[0] == 2))
                {
                    AddLog("读取电芯电压：返回超时");
                }
                if (receive.Length == 37)
                {
                    interSuccess = true;
                    testData.GetDianxinVoltage(receive);
                    AddLog($"电芯电压1-8:{testData.DianxinVoltage[0]};{testData.DianxinVoltage[1]};{testData.DianxinVoltage[2]};{testData.DianxinVoltage[3]};{testData.DianxinVoltage[4]};{testData.DianxinVoltage[5]};{testData.DianxinVoltage[6]};{testData.DianxinVoltage[7]};");
                    AddLog($"电芯电压9-16:{testData.DianxinVoltage[8]};{testData.DianxinVoltage[9]};{testData.DianxinVoltage[10]};{testData.DianxinVoltage[11]};{testData.DianxinVoltage[12]};{testData.DianxinVoltage[13]};{testData.DianxinVoltage[14]};{testData.DianxinVoltage[15]};");
                    AddLog("单位:mV");

                }

                //interSuccess = InterTestMode(1);
                Thread.Sleep(200);
            } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

            if (!interSuccess)
            {
                AddLog("读取失败");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 写入出厂时间
        /// </summary>
        /// <returns></returns>
        private bool WriteProductDate()
        {
            AddLog("写入出厂时间");
            //写入参数
            bool interSuccess = false;
            int ERROR_COUNT = 0;
            //发 01 06 01 21 00 00 00 00 crc    //00 00 00 00出厂日期内容32位
            //收 01 03 06 19 08 07 0E 11 2A 2F 3F
            //获取当前时间，转换成字节格式
            DateTime now = DateTime.Now;
            byte[] head = new byte[] { 0x01, 0x06, 0x01, 0x21 };
            byte[] writeTime = ByteConverter.DateNumberToBytes(long.Parse(now.ToString("yyyy") + now.ToString("MM") + now.ToString("dd")));
            byte[] readTime = Tools.CommunicateTool.ConcatByteArrays(head, writeTime);
            byte[] crc16 = SerialCommunicationService2.getCRC16(readTime);
            readTime = CommunicateTool.ConcatByteArrays(readTime, crc16);
            do
            {
                ERROR_COUNT++;
                byte[] receive = SerialCommunicationService2.SendTestCommand2(readTime, 10);
                if (receive.Length == 0)
                {
                    AddLog("写入出厂时间：指令返回异常");
                }
                else if (receive.Length == 1 && receive[0] == 1)
                {
                    AddLog("写入出厂时间：返回校验码不合");
                }
                else if ((receive[0] == 2))
                {
                    AddLog("写入出厂时间：返回超时");
                }
                if (receive.Length == 11)
                {
                    interSuccess = true;
                }

                //interSuccess = InterTestMode(1);
                Thread.Sleep(200);
            } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

            if (!interSuccess)
            {
                AddLog("写入出厂时间失败");
                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取当前时间并转换成字节格式
        /// </summary>
        /// <returns></returns>
        public byte[] GetFormattedDateTime()
        {
            DateTime now = DateTime.Now;

            // 每个部分转换为字节（0-255范围，日期时间各部分均在此范围内）
            byte year = (byte)(now.Year % 100);  // 取年份后两位
            byte month = (byte)now.Month;
            byte day = (byte)now.Day;
            byte hour = (byte)now.Hour;
            byte minute = (byte)now.Minute;
            byte second = (byte)now.Second;

            // 组合为字节数组返回
            return new byte[] { year, month, day, hour, minute, second };
        }


        /// <summary>
        /// 读取电流
        /// </summary>
        /// <returns></returns>
        private ushort GetCurrentFormBMS()
        {
            //拼接指令
            byte[] command = new byte[]
            {
                0x01,0x03,0x00,0x17,0x00,0x01
            };
            byte[] sendPack = CommunicateTool.ConcatByteArrays(command, SerialCommunicationService2.getCRC16(command));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand(sendPack, 7);

            //解析
            ushort current = 0;
            if (receive.Length == 2)
            {
                //解析出读取的电流
                //current = ByteConverter.BytesToNumber(receive);
                ushort test = ByteConverter.BytesToNumber(receive);
                short value = CommunicateTool.BytesToShort(receive);
                if (value < 0)
                {
                    current = (ushort)(-value);
                }
                else
                    current = (ushort)value;

                return current;

            }
            else if (receive.Length == 1)
            {
                if (receive[0] == 0x01)
                {
                    ShowBubbles("CRC校验失败，读取电流失败", 2000);
                    AddLog("读取电流返回CRC校验失败");
                }
                else if (receive[0] == 0x02)
                {
                    AddLog("读取电流返回超时");
                    ShowBubbles("读取电流返回超时，读取电流失败", 2000);
                }
            }


            return current;

        }

        /// <summary>
        /// 读取充电电流校准系数
        /// </summary>
        /// <returns></returns>
        private ushort ReadChargeCurrentAdjustParameter()
        {
            //拼接指令
            byte[] command = new byte[]
            {
                0x01,0x03,0x01,0x11,0x00,0x01
            };
            byte[] sendPack = CommunicateTool.ConcatByteArrays(command, SerialCommunicationService2.getCRC16(command));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand(sendPack, 7);

            //解析出读取的电流
            ushort current = 0;
            if (receive.Length == 2)
            {
                //解析出读取的电流
                current = ByteConverter.BytesToNumber(receive);
            }
            else if (receive.Length == 1)
            {
                if (receive[0] == 0x01)
                {
                    ShowBubbles("读取电流失败", 2000);
                    AddLog("读取电流返回CRC校验失败");
                }
                else if (receive[0] == 0x02)
                {
                    AddLog("读取电流返回超时");
                    ShowBubbles("读取电流失败", 2000);
                }
            }
            return current;
        }

        /// <summary>
        /// 读取放电电流校准系数
        /// </summary>
        /// <returns></returns>
        private ushort ReadDischargeCurrentAdjustParameter()
        {
            //拼接指令
            byte[] command = new byte[]
            {
                0x01,0x03,0x01,0x12,0x00,0x01
            };
            byte[] sendPack = CommunicateTool.ConcatByteArrays(command, SerialCommunicationService2.getCRC16(command));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand(sendPack, 7);

            //解析出读取的电流
            ushort current = 0;
            if (receive.Length == 2)
            {
                //解析出读取的电流
                current = ByteConverter.BytesToNumber(receive);
            }
            else if (receive.Length == 1)
            {
                if (receive[0] == 0x01)
                {
                    ShowBubbles("读取电流失败", 2000);
                    AddLog("读取电流返回CRC校验失败");
                }
                else if (receive[0] == 0x02)
                {
                    AddLog("读取电流返回超时");
                    ShowBubbles("读取电流失败", 2000);
                }
            }
            return current;
        }

        /// <summary>
        /// 读取短路保护相关系数
        /// </summary>
        /// <returns></returns>
        private ushort ReadAdParameter()
        {
            //拼接指令
            byte[] command = new byte[]
            {
                0x01,0x03,0x00,0x18,0x00,0x01
            };
            byte[] sendPack = CommunicateTool.ConcatByteArrays(command, SerialCommunicationService2.getCRC16(command));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand(sendPack, 7);

            //解析出读取的系数
            ushort current = 0;
            if (receive.Length == 2)
            {
                //解析出读取的电流
                current = ByteConverter.BytesToNumber(receive);
            }
            else if (receive.Length == 1)
            {
                if (receive[0] == 0x01)
                {
                    ShowBubbles("读取短路保护失败", 2000);
                    AddLog("读取短路保护返回CRC校验失败");
                }
                else if (receive[0] == 0x02)
                {
                    AddLog("读取短路保护返回超时");
                    ShowBubbles("读取短路保护失败", 2000);
                }
            }
            return current;
        }

        /// <summary>
        /// 写入充电电流校准系数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool WriteChargeCurrentAdjustParameter(ushort value)
        {
            //拼接指令
            byte[] command = new byte[]
            {
                0x01,0x06,0x01,0x11
            };
            byte[] adj = CommunicateTool.ConcatByteArrays(command, ByteConverter.NumberToBytesNormal(value));
            byte[] sendPack = CommunicateTool.ConcatByteArrays(adj, SerialCommunicationService2.getCRC16(adj));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand(sendPack, 8);

            if (receive.Length == 2 && ByteConverter.BytesToNumberNormal(receive) == value)
            {
                return true;
            }
            else if (receive.Length == 1)
            {
                if (receive[0] == 0x01)
                {
                    AddLog("写入充电校准系数没通过");
                }
                else if (receive[0] == 0x02)
                {
                    AddLog("写入充电校准系数超时");
                }
            }

            return false;
        }

        /// <summary>
        /// 写入放电电流校准系数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool WriteDischargeCurrentAdjustParameter(ushort value)
        {
            //拼接指令
            byte[] command = new byte[]
            {
                0x01,0x06,0x01,0x12
            };
            byte[] adj = CommunicateTool.ConcatByteArrays(command, ByteConverter.NumberToBytesNormal(value));
            byte[] sendPack = CommunicateTool.ConcatByteArrays(adj, SerialCommunicationService2.getCRC16(adj));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand(sendPack, 8);

            if (receive.Length == 2 && ByteConverter.BytesToNumberNormal(receive) == value)
            {
                return true;
            }
            else if (receive.Length == 1)
            {
                if (receive[0] == 0x01)
                {
                    AddLog("写入充电校准系数没通过");
                }
                else if (receive[0] == 0x02)
                {
                    AddLog("写入充电校准系数超时");
                }
            }

            return false;
        }

        /// <summary>
        /// 开启限流开关
        /// </summary>
        /// <returns></returns>
        private bool OpenLimitedCurrent()
        {
            //拼接指令
            byte[] command = new byte[]
            {
                0x01,0x20,0x00,0x09,0x00,0x01
            };
            byte[] sendPack = CommunicateTool.ConcatByteArrays(command, SerialCommunicationService2.getCRC16(command));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand(sendPack, sendPack.Length);

            if (receive.Length == 2)
                return true;
            return true;
        }

        /// <summary>
        /// 关闭限流开关
        /// </summary>
        /// <returns></returns>
        private bool CloseLimitedCurrent()
        {
            //拼接指令
            byte[] command = new byte[]
            {
                0x01,0x20,0x00,0x09,0x00,0x00
            };
            byte[] sendPack = CommunicateTool.ConcatByteArrays(command, SerialCommunicationService2.getCRC16(command));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand(sendPack, sendPack.Length);

            if (receive.Length == 2)
                return true;
            return true;
        }

        /// <summary>
        /// 干节点测试
        /// </summary>
        /// <returns></returns>
        private bool RelayStatusTest()
        {
            //拼接指令
            byte[] command = new byte[]
            {
                0X44,0X52,0X59,0X43,0X4F,0X4E,0X50,0X55,0X4C,0X4C
            };
            byte[] sendPack = CommunicateTool.ConcatByteArrays(command);

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand(sendPack, sendPack.Length);

            if (receive.Length == sendPack.Length)
                return true;
            return false;
        }


        #endregion


        #endregion

        #region 单步测试按钮

        /// <summary>
        /// 直流源1开启
        /// </summary>
        public RelayCommand OpenCharge
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenDcSource1Switch();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("直流源1开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("直流源1开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 关闭直流源1
        /// </summary>
        public RelayCommand CloseCharge
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = CloseDcSource1SwitchTest();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("直流源1关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("直流源1关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 开启直流源2
        /// </summary>
        public RelayCommand OpenDcSource2SwitchCom
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenDcSource2Switch();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("直流源2开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("直流源2开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 关闭直流源2
        /// </summary>
        public RelayCommand CloseDcSource2SwitchTestCom
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = CloseDcSource2SwitchTest();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("直流源2关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("直流源2关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 打开拨码继电器
        /// </summary>
        public RelayCommand OpenDIPSwitchValue
        {
            get
            {
                return new RelayCommand(() =>
                {

                    bool isSuccess = OpenOrCloseDIPSwitchValueTest(1);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("拨码继电器开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("拨码继电器开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 关闭拨码继电器
        /// </summary>
        public RelayCommand CloseDIPSwitchValue
        {
            get
            {
                return new RelayCommand(() =>
                {

                    bool isSuccess = OpenOrCloseDIPSwitchValueTest(0);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("拨码继电器关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("拨码继电器关闭失败", 2000);
                    }
                });
            }
        }


        /// <summary>
        /// 打开复位继电器
        /// </summary>
        public RelayCommand OpenResetSwitchStatus
        {
            get
            {
                return new RelayCommand(() =>
                {

                    bool isSuccess = ResetSwitchStatusTest(1);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("复位继电器开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("复位继电器开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 打开复位继电器
        /// </summary>
        public RelayCommand CloseResetSwitchStatus
        {
            get
            {
                return new RelayCommand(() =>
                {

                    bool isSuccess = ResetSwitchStatusTest(0);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("复位继电器关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("复位继电器关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 电子负载模式开启
        /// </summary>
        public RelayCommand OpenElectronicLoadModeCom
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.ElectronicLoadCurrent = SetElectronicLoadCurrent;
                    bool isSuccess = OpenElectronicLoadMode();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("电子负载开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("电子负载开启失败", 2000);
                    }
                });
            }
        }

        //设置电子负载电流
        private ushort setElectronicLoadCurrent;

        public ushort SetElectronicLoadCurrent
        {
            get { return setElectronicLoadCurrent; }
            set
            {
                setElectronicLoadCurrent = value;
                this.RaiseProperChanged(nameof(SetElectronicLoadCurrent));
            }
        }


        /// <summary>
        /// 电子负载模式关闭
        /// </summary>
        public RelayCommand CloseElectronicLoadModeCom
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = CloseElectronicLoadMode();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("电子负载关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("电子负载关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 电子负载输出开启
        /// </summary>
        public RelayCommand OpenReserved1
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.ElectronicLoadCurrent = SetElectronicLoadCurrent;
                    parametersSending.Reserved1 = 1;

                    bool isSuccess = OpenParameter(parametersSending.Reserved1, "Reserved1", "电子负载输出");
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("电子负载输出开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("电子负载输出开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 电子负载输出关闭
        /// </summary>
        public RelayCommand CloseReserved1
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.Reserved1 = 0;
                    bool isSuccess = CloseParameter(parametersSending.Reserved1, "Reserved1", "电子负载输出");
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("电子负载输出", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("电子负载输出", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 预留二开启
        /// </summary>
        public RelayCommand OpenReserved2
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.Reserved2 = 1;
                    bool isSuccess = CloseParameter(parametersSending.Reserved2, "Reserved2", "拨码开关");
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("拨码开关开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("拨码开关开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 预留二关闭
        /// </summary>
        public RelayCommand CloseReserved2
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.Reserved2 = 0;
                    bool isSuccess = CloseParameter(parametersSending.Reserved2, "Reserved2", "拨码开关");
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("拨码开关关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("拨码开关关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 预留3开启
        /// </summary>
        public RelayCommand OpenReserved3
        {
            get
            {

                return new RelayCommand(() =>
                {
                    parametersSending.Reserved3 = 1;
                    bool isSuccess = CloseParameter(parametersSending.Reserved3, "Reserved3", "复位开关");

                    if (isSuccess)
                    {
                        ShowBubbleWithTime("复位开关开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("复位开关开启失败", 2000);
                    }
                });
            }
        }


        /// <summary>
        /// 预留三关闭
        /// </summary>
        public RelayCommand CloseReserved3
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.Reserved3 = 0;
                    bool isSuccess = CloseParameter(parametersSending.Reserved3, "Reserved3", "复位开关");
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("复位开关关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("复位开关关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 电阻棒MOS开启
        /// </summary>
        public RelayCommand OpenResistorBankMosfetStatus
        {
            get
            {

                return new RelayCommand(() =>
                {
                    parametersSending.ResistorBankMosfetStatus = 1;
                    bool isSuccess = OpenParameter(parametersSending.ResistorBankMosfetStatus, "ResistorBankMosfetStatus", "电阻棒MOS");

                    if (isSuccess)
                    {
                        ShowBubbleWithTime("电阻棒MOS开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("电阻棒MOS开启失败", 2000);
                    }
                });
            }
        }


        /// <summary>
        /// 电阻棒MOS关闭
        /// </summary>
        public RelayCommand CloseResistorBankMosfetStatus
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.ResistorBankMosfetStatus = 0;
                    bool isSuccess = CloseParameter(parametersSending.ResistorBankMosfetStatus, "ResistorBankMosfetStatus", "电阻棒MOS");
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("电阻棒MOS关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("电阻棒MOS关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 充电继电器开启
        /// </summary>
        public RelayCommand OpenChargeRelayStatus
        {
            get
            {

                return new RelayCommand(() =>
                {
                    parametersSending.ChargeRelayStatus = 1;
                    parametersSending.ElectronicLoadCurrent = SetElectronicLoadCurrent;
                    bool isSuccess = OpenParameter(parametersSending.ChargeRelayStatus, "ChargeRelayStatus", "充电继电器");

                    if (isSuccess)
                    {
                        ShowBubbleWithTime("充电继电器开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("充电继电器开启失败", 2000);
                    }
                });
            }
        }


        /// <summary>
        /// 充电继电器关闭
        /// </summary>
        public RelayCommand CloseChargeRelayStatus
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.ChargeRelayStatus = 0;
                    bool isSuccess = CloseParameter(parametersSending.ChargeRelayStatus, "ChargeRelayStatus", "充电继电器");
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("充电继电器关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("充电继电器关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 放电继电器电器开启
        /// </summary>
        public RelayCommand OpenDischargeRelayStatus
        {
            get
            {

                return new RelayCommand(() =>
                {
                    parametersSending.DischargeRelayStatus = 1;
                    bool isSuccess = OpenParameter(parametersSending.DischargeRelayStatus, "DischargeRelayStatus", "放电继电器");

                    if (isSuccess)
                    {
                        ShowBubbleWithTime("放电继电器开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("放电继电器开启失败", 2000);
                    }
                });
            }
        }


        /// <summary>
        /// 放电继电器电器关闭
        /// </summary>
        public RelayCommand CloseDischargeRelayStatus
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.DischargeRelayStatus = 0;
                    bool isSuccess = CloseParameter(parametersSending.DischargeRelayStatus, "DischargeRelayStatus", "充电继电器");
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("放电继电器关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("放电继电器关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 充电限流负极继电器开启
        /// </summary>
        public RelayCommand OpenChargeCurrentLimitNegativeRelayStatus
        {
            get
            {

                return new RelayCommand(() =>
                {
                    parametersSending.ChargeCurrentLimitNegativeRelayStatus = 1;
                    bool isSuccess = OpenParameter(parametersSending.ChargeCurrentLimitNegativeRelayStatus, "ChargeCurrentLimitNegativeRelayStatus", "充电限流负极继电器");

                    if (isSuccess)
                    {
                        ShowBubbleWithTime("放电继电器开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("放电继电器开启失败", 2000);
                    }
                });
            }
        }


        /// <summary>
        /// 充电限流负极继电器关闭
        /// </summary>
        public RelayCommand CloseChargeCurrentLimitNegativeRelayStatus
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.ChargeCurrentLimitNegativeRelayStatus = 0;
                    bool isSuccess = CloseParameter(parametersSending.ChargeCurrentLimitNegativeRelayStatus, "ChargeCurrentLimitNegativeRelayStatus", "充电限流负极继电器");
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("放电继电器关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("放电继电器关闭失败", 2000);
                    }
                });
            }
        }


        /// <summary>
        /// 读取电流
        /// </summary>
        public RelayCommand ReadCurrentCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    BMSCurrent = GetCurrentFormBMS();


                });
            }
        }

        /// <summary>
        /// BMS电流
        /// </summary>
        private ushort bmsCurrent;

        public ushort BMSCurrent
        {
            get { return bmsCurrent; }
            set
            {
                bmsCurrent = value;
                this.RaiseProperChanged(nameof(BMSCurrent));
            }
        }

        /// <summary>
        /// 充电校准系数
        /// </summary>
        private ushort chargeCurrentAdj;

        public ushort ChargeCurrentAdj
        {
            get { return chargeCurrentAdj; }
            set
            {
                chargeCurrentAdj = value;
                this.RaiseProperChanged(nameof(ChargeCurrentAdj));
            }
        }

        /// <summary>
        /// 设置充电校准系数
        /// </summary>
        private ushort setChargeCurrentAdj;

        public ushort SetChargeCurrentAdj
        {
            get { return setChargeCurrentAdj; }
            set
            {
                setChargeCurrentAdj = value;
                this.RaiseProperChanged(nameof(SetChargeCurrentAdj));
            }
        }

        /// <summary>
        /// 放电校准系数
        /// </summary>
        private ushort dischargeCurrentAdj;

        public ushort DischargeCurrentAdj
        {
            get { return dischargeCurrentAdj; }
            set
            {
                dischargeCurrentAdj = value;
                this.RaiseProperChanged(nameof(DischargeCurrentAdj));
            }
        }

        /// <summary>
        /// 设置放电校准系数
        /// </summary>
        private ushort setDischargeCurrentAdj;

        public ushort SetDischargeCurrentAdj
        {
            get { return setDischargeCurrentAdj; }
            set
            {
                setDischargeCurrentAdj = value;
                this.RaiseProperChanged(nameof(SetDischargeCurrentAdj));
            }
        }




        /// <summary>
        /// 读取充电电流校准指数
        /// </summary>
        public RelayCommand ReadChargeCurrentAdjustParameterCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ushort sa = ReadChargeCurrentAdjustParameter();
                    ChargeCurrentAdj = sa;
                    bool isSuccess = false;
                    AddLog($"充电校准指数{ChargeCurrentAdj}");

                });
            }
        }

        /// <summary>
        /// 读取放电电流校准指数
        /// </summary>
        public RelayCommand ReadDischargeCurrentAdjustParameterCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ushort sa = ReadDischargeCurrentAdjustParameter();
                    bool isSuccess = false;
                    DischargeCurrentAdj = sa;
                    AddLog($"放电校准指数{DischargeCurrentAdj}");

                });
            }
        }



        /// <summary>
        /// 写入充电电流校准指数
        /// </summary>
        public RelayCommand WriteChargeCurrentAdjustParameterCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = WriteChargeCurrentAdjustParameter(SetChargeCurrentAdj);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("写入充电校准系数成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("写入充电校准系数失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 写入放电电流校准指数
        /// </summary>
        public RelayCommand WriteDischargeCurrentAdjustParameterCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = WriteDischargeCurrentAdjustParameter(SetDischargeCurrentAdj);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("写入放电校准系数成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("写入放电校准系数失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 开启限流开关
        /// </summary>
        public RelayCommand OpenLimitedCurrentCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenLimitedCurrent();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("限流开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("限流开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 关闭限流开关
        /// </summary>
        public RelayCommand CloseLimitedCurrentCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = CloseLimitedCurrent();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("充放电MOS管2关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("充放电MOS管2关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 低功耗检测开启
        /// </summary>
        public RelayCommand LowPowerVoltageAndCurrentCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.LowPowerRelayStatus = 1;
                    bool isSuccess = OpenParameter(parametersSending.LowPowerRelayStatus, "LowPowerRelayStatus", "低功耗继电器");
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("低功耗继电器开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("低功耗继电器开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 低功耗检测关闭
        /// </summary>
        public RelayCommand CloseLowPowerVoltageAndCurrentCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    parametersSending.LowPowerRelayStatus = 0;
                    bool isSuccess = CloseParameter(parametersSending.LowPowerRelayStatus, "LowPowerRelayStatus", "低功耗继电器");
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("低功耗继电器关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("低功耗继电器关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 开启预留继电器2
        /// </summary>
        public RelayCommand Relay2ControlTestCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = Relay2ControlTest(1);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("预留继电器1开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("预留继电器1开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 关闭预留继电器2
        /// </summary>
        public RelayCommand CloseRelay2ControlTestCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = Relay2ControlTest(0);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("预留继电器1关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("预留继电器1关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 开启预留继电器3
        /// </summary>
        public RelayCommand Relay3ControlTestCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = Relay3ControlTest(1);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("预留继电器2开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("预留继电器2开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 关闭预留继电器3
        /// </summary>
        public RelayCommand CloseRelay3ControlTestCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = Relay3ControlTest(0);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("预留继电器2关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("预留继电器2关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 开启预留继电器4
        /// </summary>
        public RelayCommand Relay4ControlTestCmd
        {
            get
            {
                return new RelayCommand(() =>
                {

                    bool isSuccess = Relay4ControlTest(1);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("预留继电器3开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("预留继电器3开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 关闭预留继电器4
        /// </summary>
        public RelayCommand CloseRelay4ControlTestCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = Relay4ControlTest(0);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("预留继电器3关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("预留继电器3关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 开启DC电源
        /// </summary>
        public RelayCommand OpenDcSourceSwitchCmd
        {
            get
            {
                return new RelayCommand(() =>
                {

                    bool isSuccess = OpenDcSourceSwitch();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("DC电源开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("DC电源开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 关闭DC电源
        /// </summary>
        public RelayCommand CloseDcSourceSwitchTestCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = CloseDcSourceSwitchTest();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("DC电源关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("DC电源关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 设置DC源电流
        /// </summary>
        public RelayCommand DcSourceControlCurrentTestCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = DcSourceControlCurrentTest();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("DC电源电流设置成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("DC电源电流设置失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 设置DC源电压
        /// </summary>
        public RelayCommand DcSourceControlVoltageTestCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = DcSourceControlVoltageTest();
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("DC电源电压设置成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("DC电源电压设置失败", 2000);
                    }
                });
            }
        }





        #endregion

        #region 气泡弹窗

        /// <summary>
        /// 气泡控件
        /// </summary>
        private UserControl _bubbleControl;

        public UserControl Bubble
        {
            get { return _bubbleControl; }
            set
            {
                _bubbleControl = value;
                OnPropertyChanged();
            }
        }

        private string _BubbleMesg;

        public string BubbleMesg
        {
            get { return _BubbleMesg; }
            set
            {
                _BubbleMesg = value;
                OnPropertyChanged(nameof(BubbleMesg));
            }
        }

        /// <summary>
        /// 显示气泡消息(在线程安全的情况下)
        /// </summary>
        /// <param name="timeDelay">显示多长时间(不少于1秒)</param>
        /// <param name="message"></param>
        private void ShowBubbles(string message, int timeDelay)
        {
            Application.Current.Dispatcher.Invoke(() => ShowBubbleWithTime(message, timeDelay));
        }


        /// <summary>
        /// 显示气泡消息
        /// </summary>
        /// <param name="message"></param>
        private async void ShowBubble(string message)
        {


            var bubbleControl = new BubbleControl();
            Bubble = bubbleControl;


            BubbleMesg = message;

            //动画效果(由下而上)
            //位移 移动时间
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(0, 10, 0, -10), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 200));

            //透明度
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 0, 200));

            Storyboard.SetTarget(thicknessAnimation, bubbleControl);
            Storyboard.SetTarget(doubleAnimation, bubbleControl);

            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath("Margin"));
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity"));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(thicknessAnimation);
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();


            await Task.Delay(4600);

            // 位移
            ThicknessAnimation thicknessAnimation2 = new ThicknessAnimation(
                new Thickness(0, 0, 0, 0), new Thickness(0, -10, 0, 10),
                new TimeSpan(0, 0, 0, 0, 400));
            // 透明度
            DoubleAnimation doubleAnimation2 = new DoubleAnimation(1, 0, new TimeSpan(0, 0, 0, 0, 400));

            Storyboard.SetTarget(thicknessAnimation2, bubbleControl);
            Storyboard.SetTarget(doubleAnimation2, bubbleControl);
            Storyboard.SetTargetProperty(thicknessAnimation2, new PropertyPath("Margin"));
            Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath("Opacity"));

            Storyboard storyboard2 = new Storyboard();
            storyboard2.Children.Add(thicknessAnimation2);
            storyboard2.Children.Add(doubleAnimation2);

            //动画效果完了才关闭
            storyboard2.Completed += (se, ev) =>
            {
                bubbleControl.Visibility = Visibility.Collapsed;
            };
            storyboard2.Begin();

        }


        private async void ShowBubbleWithTime(string message, int timeDelay)
        {


            var bubbleControl = new BubbleControl();
            Bubble = bubbleControl;


            BubbleMesg = message;

            //动画效果(由下而上)
            //位移 移动时间
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(0, 10, 0, -10), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 200));

            //透明度
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 0, 200));

            Storyboard.SetTarget(thicknessAnimation, bubbleControl);
            Storyboard.SetTarget(doubleAnimation, bubbleControl);

            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath("Margin"));
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity"));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(thicknessAnimation);
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();


            await Task.Delay(timeDelay);

            // 位移
            ThicknessAnimation thicknessAnimation2 = new ThicknessAnimation(
                new Thickness(0, 0, 0, 0), new Thickness(0, -10, 0, 10),
                new TimeSpan(0, 0, 0, 0, 400));
            // 透明度
            DoubleAnimation doubleAnimation2 = new DoubleAnimation(1, 0, new TimeSpan(0, 0, 0, 0, 400));

            Storyboard.SetTarget(thicknessAnimation2, bubbleControl);
            Storyboard.SetTarget(doubleAnimation2, bubbleControl);
            Storyboard.SetTargetProperty(thicknessAnimation2, new PropertyPath("Margin"));
            Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath("Opacity"));

            Storyboard storyboard2 = new Storyboard();
            storyboard2.Children.Add(thicknessAnimation2);
            storyboard2.Children.Add(doubleAnimation2);

            //动画效果完了才关闭
            storyboard2.Completed += (se, ev) =>
            {
                bubbleControl.Visibility = Visibility.Collapsed;
            };
            storyboard2.Begin();

        }

        #endregion

        #region 消息框

        private readonly IMessageDialogService _messageService;

        public ICommand ShowMessageCommand { get; }

        private void OnShowMessage()
        {
            
            parametersSending = new BmsSystemParametersSending() { CommunicationVersion = 1001, LowPowerRelayStatus = 1, Reserved1RelayStatus = 1, Reserved2RelayStatus = 1, Reserved3RelayStatus = 1, DcSource1Current = 10, DcSource1Switch = 1, DcSource2Current = 100 }; //发送指令实体类初始化

            if (testMachine.BatVol == "24")
            {
                parametersSending.DcSource1Voltage = 2500;

            }
            else if (testMachine.BatVol == "48")
            {
                parametersSending.DcSource1Voltage = 5000;

            }
            BMS_Receive = SendPacked(parametersSending);
            if (BMS_Receive == null)
            {
                ShowBubbles("开机失败", 1000);
            }
            else
            {
                ShowBubbles("开机成功", 1000);
            }
        }

        /// <summary>
        /// 消息弹框
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="title">标题</param>
        /// <param name="messageIcon">图标</param>
        /// <returns></returns>
        private MessageResult LShowMessage(string message, string title, MessageIcon messageIcon)
        {
            MessageResult result = MessageResult.OK;

            if (Application.Current.Dispatcher.CheckAccess())
            {
                // 当前是UI线程直接调用
                result = _messageService.Show(
                message,
                title,
                messageIcon,
                fontSize: 50
                );
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    result = _messageService.Show(
                    message,
                    title,
                    messageIcon,
                    fontSize: 50
                    );
                }));
            }

            return result;

        }

        /// <summary>
        /// 文本输入消息框
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="title">标题</param>
        /// <param name="error">输入错误提示</param>
        /// <returns></returns>
        private string LShowTestMessage(string message, string title, string error)
        {
            string result = string.Empty;
            if (Application.Current.Dispatcher.CheckAccess())
            {
                // 当前是UI线程直接调用
                result = _messageService.ShowInputDialog(
                message,
                title,
                InputType.Text,
                "软件版本",
                validator: input => input.Length >= 4,
                validationMessage: error,
                fontSize: 50);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    result = _messageService.ShowInputDialog(
                message,
                title,
                InputType.Text,
                "软件版本",
                validator: input => input.Length >= 4,
                validationMessage: error,
                fontSize: 50);
                }));
            }
            return result;
        }

        /// <summary>
        /// 密码输入消息框
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="title">标题</param>
        /// <param name="error">输入错误提示</param>
        /// <returns></returns>
        private string MShowTestMessage(string message, string title, string error)
        {
            string result = string.Empty;
            if (Application.Current.Dispatcher.CheckAccess())
            {
                // 当前是UI线程直接调用
                result = _messageService.ShowInputDialog(
                message,
                title,
                InputType.Password,
                "管理员密码",

                validator: input => input == "Tqf147258",
                validationMessage: error,
                fontSize: 50);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    result = _messageService.ShowInputDialog(
                message,
                title,
                InputType.Password,
                "管理员密码",

                validator: input => input == "Tqf147258",
                validationMessage: error,
                fontSize: 50);
                }));
            }
            return result;
        }


        //是否打开
        private bool IsExpand1;

        public bool _IsExpand1
        {
            get { return IsExpand1; }
            set
            {
                if (value == _IsExpand1) return;

                // 只有当准备从收起 -> 展开时才需要确认
                if (value && !_IsExpand1)
                {
                    string result = MShowTestMessage("请输入管理员密码", "打开权限", "密码错误，请联系研发人员");
                    if (result != "Tqf147258")
                    {
                        return;
                    }
                }
                IsExpand1 = value;
                this.RaiseProperChanged(nameof(_IsExpand1));
            }
        }

        //是否打开
        private bool IsExpand2;

        public bool _IsExpand2
        {
            get { return IsExpand2; }
            set
            {
                if (value == _IsExpand2) return;

                // 只有当准备从收起 -> 展开时才需要确认
                if (value && !_IsExpand2)
                {
                    string result = MShowTestMessage("请输入管理员密码", "打开权限", "密码错误，请联系研发人员");
                    if (result != "Tqf147258")
                    {
                        return;
                    }
                }
                IsExpand2 = value;
                this.RaiseProperChanged(nameof(_IsExpand2));
            }
        }


        #endregion
    }

}
