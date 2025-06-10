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
using System.Windows.Media.Animation;
using WpfApp2.Command;
using WpfApp2.Models;
using WpfApp2.Models.Service;
using WpfApp2.Tools;
using WpfApp2.UserControls;

namespace WpfApp2.ViewModels
{
    public class MainWindowVM : BaseViewModel
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

            //数据库路径
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
            TestUC.SetupScrolling();    //日志自动滚动

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



        //测试机器
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
                if (LogEntries.Count > 100) LogEntries.RemoveAt(0);
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
        private BmsSystemParametersSending parametersSending = new BmsSystemParametersSending() { CommunicationVersion = 1001, ChargeDischargeMosfet2Control = 1, LowerRelay1Control = 1, Relay2Control = 1, Relay3Control = 1 }; //发送指令实体类初始化，默认MOS管2开启


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
            SwitchButtonVisible(true);
        }

        /// <summary>
        /// 后台工作线程主循环
        /// </summary>
        private async Task BackgroundWorker(CancellationToken token)
        {
            //把所有项置为待测试
            ReSetTestItems();

            int i = 0;//计数
            bool flag = false;//测试成功与否
            try
            {

                while (!token.IsCancellationRequested)
                {

                    for (i = 0; i < TestItems.Count;)
                    {
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
                            MessageBoxResult boxResult = MessageBox.Show("是否继续？", "测试暂停", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (boxResult == MessageBoxResult.No)
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
                    Application.Current.Dispatcher.Invoke(() => ShowBubble("测试结束"));
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                AddLog(ex.ToString());
            }
            finally
            {
                IsRunning = false;
                AddLog("已停止");
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
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = InterTestMode(1);
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            interSuccess = Bms232CommunicationTset();//BMS232通讯
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("测试异常过多");
                        }
                        return true;
                    }
                    else
                    {
                        //进入测试模式失败
                        AddLog("进入测试模式异常");
                        return false;
                    }
                case "CAN通讯":
                    return true;
                    //进入测试模式1
                    interSuccess = true;
                    //ERROR_COUNT = 0;
                    //do
                    //{
                    //    ERROR_COUNT++;
                    //    interSuccess = InterTestMode(1);
                    //} while (!interSuccess && ERROR_COUNT < 10);//最多发十次

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            interSuccess = CanCommunicationTset();//CAN通讯
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("测试异常过多");
                        }
                        return true;
                    }
                    else
                    {
                        //进入测试模式失败
                        AddLog("进入测试模式异常");
                        return false;
                    }
                case "BMS逆变器通讯":
                    //进入测试模式1
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = InterTestMode(1);
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            ERROR_COUNT++;
                            interSuccess = BmsInverterCommunicationTset();//BMS逆变器通讯
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("测试异常过多");
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
                    MessageBoxResult boxResult = MessageBox.Show("准备测试BMS并机通讯，请把最左边的拨码打开，确保拨码值为1！", "测试暂停", MessageBoxButton.OK, MessageBoxImage.Warning);

                    //进入测试模式1
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = InterTestMode(1);
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            ERROR_COUNT++;
                            interSuccess = BmsParallelCommunicationTset();//BMS并机通讯
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("测试异常过多");
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
                    boxResult = MessageBox.Show("准备测试复位开关，请确保复位开关按下！", "测试暂停", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //进入测试模式2
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = InterTestMode(2);
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            ERROR_COUNT++;
                            interSuccess = ResetSwitchStatusTest();//复位开关
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("测试异常过多");
                        }
                        return true;
                    }
                    else
                    {
                        //进入测试模式失败
                        AddLog("进入测试模式异常");
                        return false;
                    }
                case "拨码开关":
                    boxResult = MessageBox.Show("准备测试BMS并机通讯，确保四个拨码位全部打开！", "测试暂停", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //进入测试模式3
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = InterTestMode(3);
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            ERROR_COUNT++;
                            interSuccess = DIPSwitchValueTest();//拨码开关
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("测试异常过多");
                        }
                        return true;
                    }
                    else
                    {
                        ERROR_COUNT++;
                        //进入测试模式失败
                        AddLog("进入测试模式异常");
                        return false;
                    }
                case "干节点功能":
                    //进入测试模式3
                    interSuccess = false;
                    ERROR_COUNT = 0;
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = InterTestMode(4);
                    } while (!interSuccess && ERROR_COUNT < 10);//最多发十次

                    if (interSuccess)
                    {
                        ERROR_COUNT = 0;
                        //已进入测试模式
                        do
                        {
                            ERROR_COUNT++;
                            interSuccess = RelayStatus();//测试干节点开关
                        } while (!interSuccess && ERROR_COUNT < 10);
                        if (!interSuccess)
                        {
                            AddLog("测试异常");
                            return false;
                        }
                        //干节点测试成功，退出测试模式
                        AddLog("干节点测试合格，即将退出测试模式");
                        bool exitTest = ExitTestMode();
                        return exitTest;
                    }
                    else
                    {
                        //进入测试模式失败
                        AddLog("进入测试模式异常");
                        return false;
                    }
                case "低功耗检测":
                    boxResult = MessageBox.Show("准备测试低功耗检测，请按下低功耗开关！", "测试暂停", MessageBoxButton.OK, MessageBoxImage.Warning);
                    ERROR_COUNT = 0;

                    //低功耗检测
                    do
                    {
                        ERROR_COUNT++;
                        interSuccess = LowPowerVoltageAndCurrent(1);//低功耗检测
                    } while (!interSuccess && ERROR_COUNT < 10);
                    if (!interSuccess)
                    {
                        AddLog("测试异常过多");
                        return false;
                    }
                    return true;

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
        }

        #endregion

        #region 测试项目实际操作方法

        #region 串口一(上位机串口)

        byte[] Head = new byte[] { 0x01, 0x03, 0x1E }; //帧头

        /// <summary>
        /// 进入测试模式
        /// </summary>
        /// <param name="testNum">测试模式(0-4)</param>
        /// <returns>进入是否成功</returns>
        private bool InterTestMode(ushort testNum)
        {
            //测试模式置           
            parametersSending.TestMode = testNum;

            //拼接报文                                                               
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());                  //帧头 + 数据
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack)); //帧头 + 数据 + CRC校验码 

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 65);

            //解析成帧对象
            BMS_Receive = AnalyseBmsReceive(result);

            //判断
            if (BMS_Receive == null)
            {
                return false;
            }
            else
            {
                if (BMS_Receive.TestMode == testNum)
                {
                    return true;
                }
            }
            return false;

        }

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

            //拼接报文                                                               
            byte[] sengdingPack = CommunicateTool.ConcatByteArrays(Head, parametersSending.ToByteArray());                  //帧头 + 数据
            sengdingPack = CommunicateTool.ConcatByteArrays(sengdingPack, SerialCommunicationService.getCRC16(sengdingPack)); //帧头 + 数据 + CRC校验码 

            //发送字符串
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 65);

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
        private bool ResetSwitchStatusTest()
        {
            //测试模式置2
            parametersSending.TestMode = 2;

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
                //判断复位开关是否正常
                if (BMS_Receive.ResetSwitchStatus == 1)
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
        private bool DIPSwitchValueTest()
        {
            //测试模式置3
            parametersSending.TestMode = 3;

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
                //判断拨码开关
                if (BMS_Receive.DIPSwitchValue == 15)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 干节点开关
        /// </summary>
        /// <returns></returns>
        private bool RelayStatus()
        {
            //测试模式置4
            parametersSending.TestMode = 4;

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
                //判断干节点开关是否正常
                if (BMS_Receive.Relay1Status == 1 && BMS_Receive.Relay2Status == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 退出测试模式
        /// </summary>
        /// <returns></returns>
        private bool ExitTestMode()
        {
            //测试模式置4
            parametersSending.TestMode = 5;

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
                //判断是否退出测试模式
                if (BMS_Receive.TestMode == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 低功耗检测
        /// </summary>
        /// <returns></returns>
        private bool LowPowerVoltageAndCurrent(ushort open)
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //低功耗继电器控制打开
            parametersSending.LowerRelay1Control = open;

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
                    if (BMS_Receive.LowerRelay1Control == open)
                    {
                        AddLog("低功耗继电器操作成功");
                        return true;
                    }
                    //if (bms.LowPowerVoltage <= 300 & bms.LowPowerCurrent <= 300)
                    //    return true;
                    //else
                    //{
                    //    AddLog($"不合格 低功耗:电流 {bms.LowPowerCurrent}\r 电压 {bms.LowPowerVoltage}");
                    //}
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
        private bool Relay2ControlTest(ushort open)
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //低功耗继电器控制打开
            parametersSending.LowerRelay1Control = open;

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
                if (BMS_Receive.TestMode == open)
                {
                    if (BMS_Receive.LowerRelay1Control != open)
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
        private bool Relay3ControlTest(ushort open)
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //低功耗继电器控制打开
            parametersSending.Relay3Control = open;

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
                    if (BMS_Receive.Relay3Control != open)
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

        /// <summary>
        /// 预留继电器4
        /// </summary>
        /// <returns></returns>
        private bool Relay4ControlTest(ushort open)
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //低功耗继电器控制打开
            parametersSending.Relay4Control = open;

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
                    if (BMS_Receive.Relay4Control != open)
                    {
                        AddLog("预留继电器4操作失败");
                    }
                    else
                    {
                        AddLog($"预留继电器4操作成功");
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
        /// 打开DC源开关
        /// </summary>
        /// <returns></returns>
        private bool OpenDcSourceSwitch()
        {
            //测试模式置0(即不在测试模式)
            parametersSending.TestMode = 0;
            //DC控制开关控制打开
            parametersSending.DcSourceSwitch = 1;

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
                    if (BMS_Receive .DcSourceSwitch != 1)
                    {
                        AddLog("DC控制开关打开失败");
                    }
                    else
                    {
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
            parametersSending.DcSourceSwitch = 0;

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
                    if (BMS_Receive.DcSourceSwitch != 0)
                    {
                        AddLog("DC控制开关关闭失败");
                    }
                    else
                    {
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
        private int _DcSourceControlVoltage;

        public int DcSourceControlVoltage
        {
            get { return _DcSourceControlVoltage; }
            set
            {
                _DcSourceControlVoltage = value;
                this.RaiseProperChanged(nameof(_DcSourceControlVoltage));
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
        private int _DcSourceControlCurrent;

        public int DcSourceControlCurrent
        {
            get { return _DcSourceControlCurrent; }
            set
            {
                _DcSourceControlCurrent = value;
                this.RaiseProperChanged(nameof(DcSourceControlCurrent));
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


        /// <summary>
        /// 充电电流测试步骤一(开继电器4关闭继电器5)
        /// </summary>
        /// <returns></returns>
        private bool ChargeCurrentTest1()
        {
            int failCount = 0;
            bool IsSuccess = false;
            do
            {
                //打开继电器4
                IsSuccess = OpenOrCloseChargeDischargeRelayControl(4, true);
                failCount++;
            } while (!IsSuccess && failCount <= 10);
            if (!IsSuccess)
            {
                AddLog("打开继电器4失败");
                return false;
            }

            //关闭继电器5
            failCount = 0;
            do
            {
                IsSuccess = OpenOrCloseChargeDischargeRelayControl(5, false);
                failCount += 1;
            } while (!IsSuccess && failCount <= 10);
            if (!IsSuccess)
            {
                AddLog("关闭继电器5失败");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 开启电流档位
        /// </summary>
        /// <param name="level">电流档位</param>
        /// <returns></returns>
        private bool OpenOrCloseCurrentLevel(int level, bool open)
        {
            int failCount = 0;
            bool IsSuccess = false;
            if (level == 1)
            {
                //开启电流档位1
                do
                {
                    IsSuccess = OpenOrCloseChargeDischargeRelayControl(3, open);
                    failCount++;
                } while (!IsSuccess && failCount <= 10);
            }
            else if (level == 2)
            {
                do
                {
                    IsSuccess = OpenOrCloseChargeDischargeRelayControl(2, open);
                    failCount++;
                } while (!IsSuccess && failCount <= 10);
            }
            else if (level == 3)
            {
                do
                {
                    IsSuccess = OpenOrCloseChargeDischargeRelayControl(1, open);
                    failCount++;
                } while (!IsSuccess && failCount <= 10);
            }

            if (!IsSuccess)
            {
                string msg = open ? "开启" : "关闭";
                AddLog($"{msg}{level}档电流失败");
                return false;
            }
            else
            {
                string msg = open ? "开启" : "关闭";
                AddLog($"{msg}{level}档电流成功");
                return true;
            }

        }

        /// <summary>
        /// 打开充放电电流MOS管1，关闭MOS管2
        /// </summary>
        /// <returns></returns>
        private bool ChargeCurrentTest2()
        {
            int failCount = 0;
            bool IsSuccess = false;
            do
            {
                IsSuccess = OpenOrCloseChargeDischargeMosfetControl(1, true);
                failCount++;
            } while (!IsSuccess && failCount <= 10);
            if (!IsSuccess)
            {
                AddLog("打开MOS管1失败");
                return false;
            }
            else
            {
                AddLog("打开MOS管1成功");
            };

            do
            {
                IsSuccess = OpenOrCloseChargeDischargeMosfetControl(2, false);
                failCount++;
            } while (!IsSuccess && failCount <= 10);
            if (!IsSuccess)
            {
                AddLog("关闭MOS管2失败");
                return false;
            }
            else
            {
                AddLog("打开MOS管成功");
                return true;
            }
        }

        /// <summary>
        /// 放电电流测试步骤一(开继电器5关闭继电器4)
        /// </summary>
        /// <returns></returns>
        private bool DisChargeCurrentTset1()
        {
            int failCount = 0;
            bool IsSuccess = false;
            do
            {
                //打开继电器5
                IsSuccess = OpenOrCloseChargeDischargeRelayControl(5, true);
                failCount++;
            } while (!IsSuccess && failCount <= 10);
            if (!IsSuccess)
            {
                AddLog("打开继电器5失败");
                return false;
            }

            //关闭继电器4
            failCount = 0;
            do
            {
                IsSuccess = OpenOrCloseChargeDischargeRelayControl(4, false);
                failCount += 1;
            } while (!IsSuccess && failCount <= 10);
            if (!IsSuccess)
            {
                AddLog("关闭继电器4失败");
                return false;
            }
            return true;
        }


        /// <summary>
        /// 打开或关闭第n个充放电继电器
        /// </summary>
        /// <param name="num">第几个继电器</param>
        /// <param name="open">打开还是关闭</param>
        /// <returns></returns>
        private bool OpenOrCloseChargeDischargeRelayControl(int num, bool open)
        {
            //设置充放电继电器状态
            switch (num)
            {
                case 1:
                    //parametersSending.ReSetChargeDischargeRelayControlToZero();
                    parametersSending.ChargeDischargeRelay1Control = (ushort)(open == true ? 1 : 0);
                    BMS_Receive = SendPacked(parametersSending);
                    if (BMS_Receive != null)
                    {
                        return BMS_Receive.ChargeDischargeRelay1Control == (ushort)(open ? 1 : 0);
                    }
                    else
                        return false;
                case 2:
                    //parametersSending.ReSetChargeDischargeRelayControlToZero();
                    parametersSending.ChargeDischargeRelay2Control = (ushort)(open == true ? 1 : 0);
                    BMS_Receive = SendPacked(parametersSending);
                    if (BMS_Receive != null)
                    {
                        return BMS_Receive.ChargeDischargeRelay2Control == (ushort)(open ? 1 : 0);
                    }
                    else
                        return false;
                case 3:
                    //parametersSending.ReSetChargeDischargeRelayControlToZero();
                    parametersSending.ChargeDischargeRelay3Control = (ushort)(open == true ? 1 : 0);
                    BMS_Receive = SendPacked(parametersSending);
                    if (BMS_Receive != null)
                    {
                        return BMS_Receive.ChargeDischargeRelay3Control == (ushort)(open ? 1 : 0);
                    }
                    else
                        return false;
                case 4:
                    //parametersSending.ReSetChargeDischargeRelayControlToZero();
                    parametersSending.ChargeDischargeRelay4Control = (ushort)(open == true ? 1 : 0);
                    BMS_Receive = SendPacked(parametersSending);
                    if (BMS_Receive != null)
                    {
                        return BMS_Receive.ChargeDischargeRelay4Control == (ushort)(open ? 1 : 0);
                    }
                    else
                        return false;
                case 5:
                    //parametersSending.ReSetChargeDischargeRelayControlToZero();
                    parametersSending.ChargeDischargeRelay5Control = (ushort)(open == true ? 1 : 0);
                    BMS_Receive = SendPacked(parametersSending);
                    if (BMS_Receive != null)
                    {
                        return BMS_Receive.ChargeDischargeRelay5Control == (ushort)(open ? 1 : 0);
                    }
                    else
                        return false;
                default:
                    return false;
            }


        }


        /// <summary>
        /// 打开或关闭充放电MOS管(n)
        /// </summary>
        /// <param name="num">第几个MOS管</param>
        /// <param name="open">打开或者关闭</param>
        /// <returns></returns>
        private bool OpenOrCloseChargeDischargeMosfetControl(int num, bool open)
        {
            //设置充放电继电器状态
            switch (num)
            {
                case 1:
                    parametersSending.ReSetChargeDischargeMosfetControl();
                    parametersSending.ChargeDischargeMosfet1Control = (ushort)(open == true ? 1 : 0);
                    BMS_Receive = SendPacked(parametersSending);
                    if (BMS_Receive != null)
                    {
                        return BMS_Receive.ChargeDischargeMosfet1Control == (ushort)(open ? 1 : 0);
                    }
                    else
                        return false;
                case 2:
                    parametersSending.ReSetChargeDischargeMosfetControl();
                    parametersSending.ChargeDischargeMosfet2Control = (ushort)(open == true ? 1 : 0);
                    BMS_Receive = SendPacked(parametersSending);
                    if (BMS_Receive != null)
                    {
                        return BMS_Receive.ChargeDischargeRelay2Control == (ushort)(open ? 1 : 0);
                    }
                    else
                        return false;
                default: return false;

            }


        }


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
            byte[] result = SerialCommunicationService.SendTestCommand(sengdingPack, 65);

            //解析
            BmsSystemparametersReceive bmsReceive = AnalyseBmsReceive(result);

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
            else if (result.Length == 60)
            {
                BmsSystemparametersReceive bms = BmsSystemparametersReceive.FromByteArray(result);
                return bms;
            }
            return null;
        }

        #endregion

        #region 串口二(bms板)

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
                current = ByteConverter.BytesToNumberNormal(receive);
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
            byte[] adj = CommunicateTool.ConcatByteArrays(command, ByteConverter.NumberToBytes(value));
            byte[] sendPack = CommunicateTool.ConcatByteArrays(adj, SerialCommunicationService2.getCRC16(adj));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand2(sendPack, 8);

            if (receive.Equals(sendPack))
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
            byte[] adj = CommunicateTool.ConcatByteArrays(command, ByteConverter.NumberToBytes(value));
            byte[] sendPack = CommunicateTool.ConcatByteArrays(adj, SerialCommunicationService2.getCRC16(adj));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand2(sendPack, 8);

            if (receive.Equals(adj))
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
                0x01,0x06,0x00,0xC8,0x00,0x01
            };
            byte[] sendPack = CommunicateTool.ConcatByteArrays(command, SerialCommunicationService2.getCRC16(command));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand(sendPack, sendPack.Length);

            return false;
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
                0x01,0x06,0x00,0xC8,0x00,0x00
            };
            byte[] sendPack = CommunicateTool.ConcatByteArrays(command, SerialCommunicationService2.getCRC16(command));

            //发送指令
            byte[] receive = SerialCommunicationService2.SendTestCommand(sendPack, sendPack.Length);

            return false;
        }


        #endregion


        #endregion

        #region 单步测试按钮


        /// <summary>
        /// 充电开启
        /// </summary>
        public RelayCommand OpenCharge
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeRelayControl(4, true);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("充电开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("充电开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 关闭充电
        /// </summary>
        public RelayCommand CloseCharge
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeRelayControl(4, false);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("充电关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("充电关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 开启放电
        /// </summary>
        public RelayCommand OpenDisCharge
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeRelayControl(5, true);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("放电开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("放电开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 关闭放电
        /// </summary>
        public RelayCommand CloseDisCharge
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeRelayControl(5, false);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("放电关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("放电关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 一档电流开启
        /// </summary>
        public RelayCommand OpenLevel1Current
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeRelayControl(3, true);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("一档电流开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("一档电流开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 一档电流关闭
        /// </summary>
        public RelayCommand CloseLevel1Current
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeRelayControl(3, false);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("一档电流关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("一档电流关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 二档电流开启
        /// </summary>
        public RelayCommand OpenLevel2Current
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeRelayControl(2, true);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("二档电流开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("二档电流开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 二挡电流关闭
        /// </summary>
        public RelayCommand CloseLevel2Current
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeRelayControl(2, false);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("二档电流关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("二档电流关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 三挡电流开启
        /// </summary>
        public RelayCommand OpenLevel3Current
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeRelayControl(1, true);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("三档电流开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("三档电流开启失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 三挡电流关闭
        /// </summary>
        public RelayCommand CloseLevel3Current
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeRelayControl(1, false);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("三档电流关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("三档电流关闭失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 充电MOS管1打开
        /// </summary>
        public RelayCommand OpenChargeMOS1
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeMosfetControl(1, true);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("充放电MOS管1打开成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("充放电MOS管1打开失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 充电MOS管1关闭
        /// </summary>
        public RelayCommand CloseChargeMOS1
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeMosfetControl(1, false);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("充放电MOS管1关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("充放电MOS管1关闭失败", 2000);
                    }
                });
            }
        }


        /// <summary>
        /// 充电MOS管2打开
        /// </summary>
        public RelayCommand OpenChargeMOS2
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeMosfetControl(2, true);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("充放电MOS管2打开成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("充放电MOS管2打开失败", 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 充电MOS管2关闭
        /// </summary>
        public RelayCommand CloseChargeMOS2
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = OpenOrCloseChargeDischargeMosfetControl(2, false);
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
        /// 读取电流
        /// </summary>
        public RelayCommand ReadCurrentCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ushort sa = GetCurrentFormBMS();
                    bool isSuccess = false;
                    if (isSuccess)
                    {
                        ShowBubbleWithTime(sa.ToString(), 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime(sa.ToString(), 2000);
                    }
                });
            }
        }

        /// <summary>
        /// 充电指数
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
        /// 放电指数
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
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("充放电MOS管2关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime(sa.ToString(), 2000);
                    }
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
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("充放电MOS管2关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime(sa.ToString(), 2000);
                    }
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
                    bool isSuccess = WriteChargeCurrentAdjustParameter(ChargeCurrentAdj);
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
        /// 写入放电电流校准指数
        /// </summary>
        public RelayCommand WriteDischargeCurrentAdjustParameterCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool isSuccess = WriteDischargeCurrentAdjustParameter(DischargeCurrentAdj);
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
                    bool isSuccess = LowPowerVoltageAndCurrent(1);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("低功耗继电器1开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("低功耗继电器1开启失败", 2000);
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
                    bool isSuccess = LowPowerVoltageAndCurrent(0);
                    if (isSuccess)
                    {
                        ShowBubbleWithTime("低功耗继电器1关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("低功耗继电器1关闭失败", 2000);
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
                        ShowBubbleWithTime("低功耗继电器2开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("低功耗继电器2开启失败", 2000);
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
                        ShowBubbleWithTime("低功耗继电器2关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("低功耗继电器2关闭失败", 2000);
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
                        ShowBubbleWithTime("低功耗继电器3开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("低功耗继电器3开启失败", 2000);
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
                        ShowBubbleWithTime("低功耗继电器3关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("低功耗继电器3关闭失败", 2000);
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
                        ShowBubbleWithTime("低功耗继电器4开启成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("低功耗继电器4开启失败", 2000);
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
                        ShowBubbleWithTime("低功耗继电器4关闭成功", 2000);
                    }
                    else
                    {
                        ShowBubbleWithTime("低功耗继电器4关闭失败", 2000);
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

    }

}
