using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WpfApp2.CustomMessageBox.Service;
using WpfApp2.Tools;
using WpfApp2.ViewModels;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            EnterFullScreen();
            //FullScreenHelper.EnterFullScreen(this); // 启动全屏
            this.DataContext = new MainWindowVM(new MessageDialogService());
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DataContext is MainWindowVM mainVM)
            {
                if (mainVM.IsRunning) {
                    //退出程序时，若还在进行测试，则关闭所有电子元件
                    mainVM.ExitTestMode();
                }
            }
        }

        /// <summary>
        /// 拖拽窗口
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMin(object sender, RoutedEventArgs e)
        {
            //最小化
            this.WindowState = WindowState.Minimized;

            //this.WindowState = WindowState.Maximized;//最大化
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose(object sender, RoutedEventArgs e)
        {
            //this.Close();
            Environment.Exit(0);
        }


        private bool isFullScreen = true; // 记录当前状态
        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMaximize(object sender, RoutedEventArgs e)
        {
            //this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            

            if (isFullScreen)
            {
                //FullScreenHelper.ExitFullScreen(this, 800, 600);
                ExitFullScreen();
            }
            else
            {
                //FullScreenHelper.EnterFullScreen(this);
                EnterFullScreen();
            }
           
        }

        // 进入全屏（不遮挡任务栏）
        private void EnterFullScreen()
        {
            var workArea = SystemParameters.WorkArea;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            // 必须先恢复到 Normal 模式，否则某些情况下窗口尺寸不刷新
            this.WindowState = WindowState.Normal;

            this.Left = workArea.Left;
            this.Top = workArea.Top;
            this.Width = workArea.Width;
            this.Height = workArea.Height;
            isFullScreen = true;
        }

        // 退出全屏（保留无边框）
        private void ExitFullScreen()
        {
            this.WindowStyle = WindowStyle.None;
            // 必须先恢复到 Normal 模式，否则某些情况下窗口尺寸不刷新
            this.WindowState = WindowState.Normal;
            this.ResizeMode = ResizeMode.CanResize;

            // 设置成一个非全屏的固定大小（你可以自己调整）
            this.Width = 1294;
            this.Height = 800;

            var workArea = SystemParameters.WorkArea;
            // 居中显示
            this.Left = (workArea.Width - this.Width) / 2;
            this.Top = (workArea.Height - this.Height) / 2;

            isFullScreen = false;
        }
    }
}