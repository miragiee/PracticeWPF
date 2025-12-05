using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp1
{
    public class BoolToDeliveryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isDelivery)
            {
                return isDelivery ? "С доставкой" : "Самовывоз";
            }
            return "Не определено";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}