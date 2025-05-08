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
                    LogListView.ScrollIntoView(e.NewItems[e.NewItems.Count - 1]);
                }
            }; 
            
            //测试项自动滚动到已完成项目
            foreach (var item in viewModel.Items)
            {
                item.PropertyChanged += (sender, e) =>
                {
                    TestItemListView.ScrollIntoView(sender);
                };
            }
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
