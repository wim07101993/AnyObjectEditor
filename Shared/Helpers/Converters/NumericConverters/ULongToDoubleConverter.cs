using System;
using System.Globalization;
using System.Windows.Data;

namespace Shared.Helpers.Converters.NumericConverters
{
    public class ULongToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is ulong
                ? System.Convert.ToDouble(value)
                : value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is double
                ? System.Convert.ToUInt64(value)
                : value;
    }
}
