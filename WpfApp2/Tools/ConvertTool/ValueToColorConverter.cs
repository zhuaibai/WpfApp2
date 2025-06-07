using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp2.Tools.ConvertTool
{
    public class ValueToColorConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                // 值为1时返回深色，为0时返回浅色
                return intValue == 1
                    ? new SolidColorBrush(Colors.Black)       // 深色
                    : new SolidColorBrush(Colors.LightGray);  // 浅色
            }
            return Brushes.Black; // 默认颜色
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
