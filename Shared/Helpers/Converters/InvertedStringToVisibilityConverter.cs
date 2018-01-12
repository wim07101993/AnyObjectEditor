using System;
using System.Globalization;
using System.Windows.Data;

namespace Shared.Helpers.Converters
{
    public class InvertedStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => string.IsNullOrEmpty(value as string);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => throw new NotImplementedException();
    }
}
