using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp2.Tools.ConvertTool
{
    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ushort intValue)
            {
                // 值为1时返回深色，为0时返回浅色
                return intValue == 1
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff2bedf1"))       // 深色
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#aaa"));  // 浅色
            }
            return Brushes.Black; // 默认颜色
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ValueToColorConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ushort intValue)
            {
                // 值为1时返回深色，为0时返回浅色
                return intValue == 1
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#aaa"))       // 深色
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff2bedf1"));  // 浅色
            }
            return Brushes.Black; // 默认颜色
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 如果绑定值为整数
            if (value is int intValue)
            {
                // 值为0时返回红色
                if (intValue == 0) return Brushes.Red;

                // 值为1时返回指定颜色 #ff2bedf1
                if (intValue == 1) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff2bedf1"));
            }

            // 默认返回黑色
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("单向转换不支持逆向转换");
        }
    }
}
