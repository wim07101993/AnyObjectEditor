using System;
using System.Globalization;
using System.Windows.Data;

namespace Shared.Helpers.Converters.NumericConverters
{
    public class IntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is int
                ? System.Convert.ToDouble(value)
                : value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is double
                ? System.Convert.ToInt32(value)
                : value;
    }
}
