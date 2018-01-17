using System;
using System.Globalization;
using System.Windows.Data;

namespace Shared.Helpers.Converters.NumericConverters
{
    public class UIntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is uint
                ? System.Convert.ToDouble(value)
                : value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is double
                ? System.Convert.ToUInt32(value)
                : value;
    }
}
