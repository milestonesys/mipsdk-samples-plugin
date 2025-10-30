using DemoServerApplication.Configuration;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DemoServerApplication.UI.Converters
{
    internal class CoordinatesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((double)value == 0) 
                return string.Empty;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
