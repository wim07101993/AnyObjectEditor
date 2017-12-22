using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace DatabaseManager.Helpers.Converters
{
    public class MultiBooleanAndToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            => values.Any(value => !(value is bool) || !(bool) value)
                ? Visibility.Collapsed
                : Visibility.Visible;

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}