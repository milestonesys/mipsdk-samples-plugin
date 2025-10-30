using DemoServerApplication.Configuration;
using System;
using System.Windows.Data;

namespace DemoServerApplication.UI.Converters
{
    public class DoorControllerNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            foreach (var doorController in ConfigurationManager.Instance.DoorControllers)
            {
                if (doorController.Id == (Guid)value)
                    return doorController.Name;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
