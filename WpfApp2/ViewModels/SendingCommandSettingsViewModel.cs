using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows;
using System.Xml.Serialization;
using WpfApp2.Command;
using WpfApp2.Tools;
using WpfApp2.UserControls;
using WpfApp2.Models;
using System.Security.Cryptography;

namespace WpfApp2.ViewModels
{
    public class SendingCommandSettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SendingCommandSettingsViewModel()
        {
            SendingCommands = LoadSettings();
            SaveCommand = new DelegateCommand(SaveSettings);
            SaveCommandToFile = new DelegateCommand(SaveSettingsToAtherFile);
            LoadFromFile = new DelegateCommand((object ds) => { SendingCommands = LoadSettingsFromFile(); });
            // 初始化命令，传入异步操作和按钮状态判断
            WriteCommand = new AsyncRelayCommand(ExecuteWriteAsync, () => IsButtonEnabled);
            //点击开启配置窗口
            //DoubleClickCommand = new RelayCommand(OnDoubleClick);
            //初始化串口
            SerialPort1 = new SerialPortSettingViewModel();
        }

        #region 功能方法
        //配置文件信息
        // 集合属性
        private ObservableCollection<SendingCommand> _sendingCommands;
        public ObservableCollection<SendingCommand> SendingCommands
        {
            get => _sendingCommands;
            set
            {
                if (_sendingCommands != value)
                {
                    _sendingCommands = value;
                    OnPropertyChanged(nameof(SendingCommands)); // 触发属性变更通知
                }
            }
        }
        //保存
        public ICommand SaveCommand { get; }
        //导出
        public ICommand SaveCommandToFile { get; set; }
        //导入
        public ICommand LoadFromFile { get; set; }
        //保存路径
        private string _filePathToSave;

        public string filePathToSave
        {
            get { return _filePathToSave; }
            set
            {
                _filePathToSave = value;
                this.OnPropertyChanged(nameof(filePathToSave));
            }
        }

        //导入的文件名
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                this.OnPropertyChanged(nameof(FileName));
            }
        }

        /// <summary>
        /// 加载默认配置文件
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<SendingCommand> LoadSettings()
        {
            try
            {
                string path =  "default.xml";
                filePathToSave = path;
                if (File.Exists(path))
                {
                    // 1. 读取文件内容
                    using var fileStream = new FileStream(path, FileMode.Open);
                    var wrapperSerializer = new XmlSerializer(typeof(ConfigWrapper));
                    var wrapper = (ConfigWrapper)wrapperSerializer.Deserialize(fileStream);

                    // 2. 计算读取数据的哈希（直接对字节数组计算）
                    using var sha256 = SHA256.Create();
                    string computedHashString = Convert.ToBase64String(sha256.ComputeHash(wrapper.DataBytes));

                    // 3. 验证哈希
                    if (computedHashString != wrapper.Hash)
                    {
                        MessageBox.Show("配置文件已损坏或被篡改！", "错误",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return new ObservableCollection<SendingCommand>();
                    }

                    // 4. 反序列化原始数据
                    using var dataStream = new MemoryStream(wrapper.DataBytes);
                    var serializer = new XmlSerializer(typeof(ObservableCollection<SendingCommand>));
                    return (ObservableCollection<SendingCommand>)serializer.Deserialize(dataStream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置文件已损坏或被篡改！", "错误",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"加载设置时出错: {ex.Message}");
            }
            return new ObservableCollection<SendingCommand>();
        }

        /// <summary>
        /// 导入配置文件
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<SendingCommand> LoadSettingsFromFile()
        {
            try
            {
                // 创建文件打开对话框
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "XML文件 (*.xml)|*.xml",
                    Title = "选择配置文件",
                    CheckFileExists = true,
                    Multiselect = false
                };

                // 显示对话框并等待用户选择
                var dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == true)
                {
                    // 获取用户选择的文件路径
                    string filePath = openFileDialog.FileName;
                    //存一下保存路径
                    filePathToSave = filePath;
                    //显示导入文件名
                    FileName = Path.GetFileName(filePath);
                    //if (File.Exists(filePath))
                    //{
                    //    var serializer = new XmlSerializer(typeof(ObservableCollection<SendingCommand>));
                    //    using (var reader = new StreamReader(filePath))
                    //    {
                    //        var result = (ObservableCollection<SendingCommand>)serializer.Deserialize(reader);
                    //        return result;
                    //    } 
                    //}
                    // 1. 读取文件内容
                    using var fileStream = new FileStream(filePath, FileMode.Open);
                    var wrapperSerializer = new XmlSerializer(typeof(ConfigWrapper));
                    var wrapper = (ConfigWrapper)wrapperSerializer.Deserialize(fileStream);

                    // 2. 计算读取数据的哈希（直接对字节数组计算）
                    using var sha256 = SHA256.Create();
                    string computedHashString = Convert.ToBase64String(sha256.ComputeHash(wrapper.DataBytes));

                    // 3. 验证哈希
                    if (computedHashString != wrapper.Hash)
                    {
                        MessageBox.Show("哈希校验不通过，配置文件已损坏或被篡改！", "错误",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return new ObservableCollection<SendingCommand>();
                    }

                    // 4. 反序列化原始数据
                    using var dataStream = new MemoryStream(wrapper.DataBytes);
                    var serializer = new XmlSerializer(typeof(ObservableCollection<SendingCommand>));

                    // 5. 修改一下保存路径和显示
                    //filePathToSave = wrapper.machineName + ".xml";
                    SelectedMachineItem = wrapper.machineName;

                    // 6. 直接保存
                    //SaveSettings(this);

                    return (ObservableCollection<SendingCommand>)serializer.Deserialize(dataStream);
                }
                //取消选择则显示原来的配置文件
                return SendingCommands;
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件加载失败，配置文件已损坏或被篡改！", "错误",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"加载设置时出错: {ex.Message}");
                return new ObservableCollection<SendingCommand>();
            }

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="parameter"></param>
        private void SaveSettings(object parameter)
        {
            try
            {
                string path = filePathToSave ?? "default.xml";
                //var serializer = new XmlSerializer(typeof(ObservableCollection<SendingCommand>));
                //using (var writer = new StreamWriter(path))
                //{
                //    serializer.Serialize(writer, SendingCommands);
                //}

                // 1. 序列化原始数据到内存流
                var serializer = new XmlSerializer(typeof(ObservableCollection<SendingCommand>));
                byte[] dataBytes;

                using (var ms = new MemoryStream())
                {
                    serializer.Serialize(ms, SendingCommands);
                    dataBytes = ms.ToArray(); // 直接获取字节数组
                }

                // 2. 计算哈希值（直接对字节数组计算）
                using var sha256 = SHA256.Create();
                string hashString = Convert.ToBase64String(sha256.ComputeHash(dataBytes));

                // 3. 创建包含哈希的包装对象
                var wrapper = new ConfigWrapper
                {
                    machineName = SelectedMachineItem,
                    Hash = hashString,
                    DataBytes = dataBytes
                };

                // 4. 保存到文件
                using var fileStream = new FileStream(path, FileMode.Create);
                var wrapperSerializer = new XmlSerializer(typeof(ConfigWrapper));
                wrapperSerializer.Serialize(fileStream, wrapper);
                MessageBox.Show($"已保存");
                Console.WriteLine("设置已保存");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存设置时出错: {ex.Message}");
                MessageBox.Show($"保存设置时出错:{ex.Message}");
            }
        }

        /// <summary>
        /// 导出配置文件
        /// </summary>
        /// <param name="parameter"></param>
        private void SaveSettingsToAtherFile(object parameter)
        {
            try
            {
                // 创建文件保存对话框
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "XML文件 (*.xml)|*.xml",
                    Title = "保存配置文件",
                    DefaultExt = "xml",
                    AddExtension = true
                };

                // 显示对话框并等待用户选择
                var dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult == true)
                {
                    // 获取用户选择的文件路径
                    string filePath = saveFileDialog.FileName;

                    //var serializer = new XmlSerializer(typeof(ObservableCollection<SendingCommand>));
                    //using (var writer = new StreamWriter(filePath))
                    //{
                    //    serializer.Serialize(writer, SendingCommands);
                    //}
                    //MessageBox.Show($"保存设置成功");
                    //Console.WriteLine("设置已保存");

                    // 1. 序列化原始数据到内存流
                    var serializer = new XmlSerializer(typeof(ObservableCollection<SendingCommand>));
                    byte[] dataBytes;

                    using (var ms = new MemoryStream())
                    {
                        serializer.Serialize(ms, SendingCommands);
                        dataBytes = ms.ToArray(); // 直接获取字节数组
                    }

                    // 2. 计算哈希值（直接对字节数组计算）
                    using var sha256 = SHA256.Create();
                    string hashString = Convert.ToBase64String(sha256.ComputeHash(dataBytes));

                    // 3. 创建包含哈希的包装对象
                    var wrapper = new ConfigWrapper
                    {
                        machineName = SelectedMachineItem,
                        Hash = hashString,
                        DataBytes = dataBytes
                    };

                    // 4. 保存到文件
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    var wrapperSerializer = new XmlSerializer(typeof(ConfigWrapper));
                    wrapperSerializer.Serialize(fileStream, wrapper);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置时出错: {ex.Message}", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region 写入

        // 按钮文本属性
        private string _buttonText = "点击写入";
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        // 按钮可用状态
        private bool _isButtonEnabled = true;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set => OnPropertyChanged(nameof(IsButtonEnabled));
        }

        public Action<string, int> ShowBoubleWithTime;
        // 命令
        public AsyncRelayCommand WriteCommand { get; }
        // 异步执行写入操作
        private async Task ExecuteWriteAsync()
        {
            try
            {
                // 更新按钮状态为"写入中"且不可点击
                ButtonText = "写入中...";
                IsButtonEnabled = false;
                //Debug.WriteLine($"ButtonText 设置为: {ButtonText}");

                // 通知命令状态已更改
                WriteCommand.NotifyCanExecuteChanged();

                //组装报文
                byte[] sendBuffer = AnalyseSendCommand.GetSendBytes(SelectedMachineItem, SendingCommands);
                string receive = "";
                // 模拟耗时操作（实际应用中替换为真实的写入逻辑）
                await Task.Run(() =>
                {
                    // 模拟耗时操作，例如写入文件或网络请求
                    System.Threading.Thread.Sleep(2000);
                    receive = SerialCommunicationService2.SendCommand(sendBuffer, 191);

                });

                ButtonText = receive=="ACK" ? "成功" : "失败";
                ShowBoubleWithTime($"{ButtonText}", 1500);

                await Task.Delay(500); // 短暂显示"完成"状态
                ButtonText = "点击再次写入";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                ButtonText = "点击再次写入";
            }
            finally
            {
                // 恢复按钮可点击状态
                IsButtonEnabled = true;
                WriteCommand.NotifyCanExecuteChanged();
                //关闭串口
                //SerialCommunicationService.CloseCom();
            }
        }

        private bool AnalyseReceuve(string receive)
        {
            if (receive.Length >= 7)
            {
                if (receive.Substring(0, 4) == "(NAK")
                {
                    return false;
                }
                else if (receive.Substring(0, 4) == "(ACK")
                {
                    // 操作完成后更新按钮状态
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 配置

        public SerialPortSettingViewModel SerialPort1 { get; set; }

        //密码
        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                this.OnPropertyChanged(nameof(Password));
            }
        }


        // 双击命令
        public ICommand DoubleClickCommand { get; }

        // 处理双击事件
        //private void OnDoubleClick()
        //{
        //    // 显示密码输入对话框

        //    var dialogResult = ShowDialog(this);

        //    if (dialogResult == true)
        //    {
        //        if (Password == "Tqf147258")
        //        {
        //            // 密码验证通过，打开新窗口
        //            OpenNewWindow(this);
        //        }
        //        else
        //        {
        //            MessageBox.Show("密码错误！", "验证失败",
        //           MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    else
        //    {

        //    }
        //}

        //// 显示对话框（可通过依赖注入替换为实际实现）
        //private bool? ShowDialog(SendingCommandSettingsViewModel viewModel)
        //{
        //    var dialog = new PasswordDialogWindow { DataContext = viewModel };
        //    return dialog.ShowDialog();
        //}

        //// 打开新窗口
        //private void OpenNewWindow(SendingCommandSettingsViewModel viewModel)
        //{
        //    var newWindow = new PropertyWindow { DataContext = viewModel };
        //    newWindow.Show();
        //}

        #endregion

        #region 下拉选择机型

        private string? _selectedItem = "UPSLB600";
        public string? SelectedMachineItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> MachineItems { get; } = new(){
        "UPSLB600",
        "VQ3024",
        "VDF"
         };

        ////切换指令
        //public ICommand SelectionChangedCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(OnSelectionChanged);
        //    }
        //}

        //选中项改变
        private void OnSelectionChanged()
        {


            // 如果需要，可以在这里添加业务逻辑
            if (SelectedMachineItem != null)
            {
                // 执行你的命令逻辑
                SwitchViewToVQorGB(SelectedMachineItem);
            }
        }

        /// <summary>
        ///根据机型导入配置文件
        /// </summary>
        public void SwitchViewToVQorGB(string view)
        {

            if (view == "VQ3024")
            {
                filePathToSave = "VQ3024.xml";
                SendingCommands = LoadSettings();
                SelectedMachineItem = "VQ3024";
            }
            else if (view == "UPSLB600")
            {
                filePathToSave = "UPSLB600.xml";
                //重新加载配置文件
                SendingCommands = LoadSettings();
                //下拉框显示
                SelectedMachineItem = "UPSLB600";
            }
            else
            {
                filePathToSave = "VDF.xml";
                SendingCommands = LoadSettings();
                SelectedMachineItem = "VDF";
            }
            FileName = filePathToSave;
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


        #endregion
    }
}

