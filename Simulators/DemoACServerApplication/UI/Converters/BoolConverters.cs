using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DemoServerApplication.UI.Converters
{
    internal class BoolConverter<T> : IValueConverter
    {
        public BoolConverter(T trueValue, T falseValue)
        {
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        internal T TrueValue { get; set; }
        internal T FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val = System.Convert.ToBoolean(value, System.Globalization.CultureInfo.InvariantCulture);
            return val ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return TrueValue.Equals(value) ? true : false;
        }
    }

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class BoolConverterExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            System.Diagnostics.Debug.Assert(TrueValue != null);
            System.Diagnostics.Debug.Assert(FalseValue != null);
            return new BoolConverter<object>(TrueValue, FalseValue);
        }

        public object TrueValue { get; set; }
        public object FalseValue { get; set; }
    }

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    internal class BoolToVisibilityConverter : MarkupExtension
    {
        public BoolToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new BoolConverter<Visibility>(TrueValue, FalseValue);
        }

        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }
    }
}