using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfApp2.Tools.ConvertTool
{
    public class BooleanToTextConverter : IMultiValueConverter
    {
        // 可自定义显示文本
        public string TrueText { get; set; } = "合格";
        public string FalseText { get; set; } = "不合格";
        public string PendingText { get; set; } = "待测试";
        public string LoadingText { get; set; } = "测试中";


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 检查输入值
            if (values == null || values.Length < 2)
                return DependencyProperty.UnsetValue;

            // 检查 Flag 值
            if (values[1] is int flag && flag == 0)
            {
                if (values[0] is bool isTesting)
                return isTesting ? LoadingText : PendingText;
            }
               

            // 根据 IsImportant 返回对应文本
            if (values[0] is bool isImportant)
                return isImportant ? TrueText : FalseText;
            
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StringSubstringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string timeString && timeString.Length > 10)
            {
                return timeString.Substring(11, 8); // 从第11个字符开始，截取8个字符（时分秒）
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
