using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Models;
using WpfApp2.ViewModels;

namespace WpfApp2.UserControls
{
    /// <summary>
    /// TestViewUC.xaml 的交互逻辑
    /// </summary>
    public partial class TestViewUC : UserControl
    {
        public TestViewUC()
        {
            InitializeComponent();
            
        }

        public void SetupScrolling()
        {
            var viewModel = (MainWindowVM)DataContext;
            //日志自动滚动到最新
            viewModel.LogEntries.CollectionChanged += (sender, e) =>
            {
                if (e.NewItems != null && e.NewItems.Count > 0)
                {
                    // 为新添加的项注册属性变更事件
                    foreach (LogEntry newItem in e.NewItems)
                    {
                        newItem.PropertyChanged += (s, args) =>
                        {
                            // 同样使用Dispatcher确保UI已更新
                            Dispatcher.BeginInvoke((Action)(() =>
                            {
                                LogListView.ScrollIntoView(s);
                            }), System.Windows.Threading.DispatcherPriority.Render);
                        };
                    }

                    // 延迟滚动到最后一项，确保UI已更新
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        LogListView.ScrollIntoView(e.NewItems[e.NewItems.Count - 1]);
                    }), System.Windows.Threading.DispatcherPriority.Render);
                }
            };
        }

        private void ScrollToSpecificRecord_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (MainWindowVM)DataContext;
            if (viewModel.LogEntries.Count > 0)
            {
                // 这里假设要滚动到第一条记录，你可以根据需求修改索引
                int index = 0;
                var targetLogEntry = viewModel.LogEntries[index];
                LogListView.ScrollIntoView(targetLogEntry);
            }
        }
    }
}
