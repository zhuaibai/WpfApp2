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
}
