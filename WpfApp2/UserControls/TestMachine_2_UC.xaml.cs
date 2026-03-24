using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// TestMachine_2_UC.xaml 的交互逻辑
    /// </summary>
    public partial class TestMachine_2_UC : UserControl
    {
        public TestMachine_2_UC()
        {
            InitializeComponent();
            Loaded += (s, e) => BlueText.Focus(); // 初始聚焦

            // 监听 ViewModel 属性变化
            this.DataContextChanged += (s, e) =>
            {
                if (e.OldValue is INotifyPropertyChanged oldVm)
                    oldVm.PropertyChanged -= ViewModel_PropertyChanged;
                if (e.NewValue is INotifyPropertyChanged newVm)
                    newVm.PropertyChanged += ViewModel_PropertyChanged;
            };
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowVM.ShouldFocusTextBox))
            {
                if (sender is MainWindowVM vm && vm.ShouldFocusTextBox)
                {
                    BlueText.Focus();
                    vm.ShouldFocusTextBox = false; // 重置信号
                }
            }
        }

        #region 日志自动滚动
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
                                var lastItem = e.NewItems[e.NewItems.Count - 1];
                                if (lastItem != null && !IsItemVisible(lastItem))
                                {
                                    LogListView.ScrollIntoView(lastItem);
                                }
                            }), System.Windows.Threading.DispatcherPriority.Render);
                        };
                    }

                    // 延迟滚动到最后一项，确保UI已更新
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (!IsItemVisible(e.NewItems[e.NewItems.Count - 1]))
                        {
                            LogListView.ScrollIntoView(e.NewItems[e.NewItems.Count - 1]);
                        }
                        //LogListView.ScrollIntoView(e.NewItems[e.NewItems.Count - 1]);
                    }), System.Windows.Threading.DispatcherPriority.Render);
                }
            };
        }

        //仅当最新项不在视图中时才触发滚动，避免干扰用户当前浏览位置
        private bool IsItemVisible(object? item)
        {
            if (item == null) return false;

            var container = LogListView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
            if (container == null) return false;

            var transform = container.TransformToAncestor(LogListView);
            var bounds = transform.TransformBounds(new Rect(0, 0, container.ActualWidth, container.ActualHeight));
            var viewport = new Rect(0, 0, LogListView.ActualWidth, LogListView.ActualHeight);

            return viewport.Contains(bounds.TopLeft) || viewport.Contains(bounds.BottomRight);
        }

        #endregion
    }
}
