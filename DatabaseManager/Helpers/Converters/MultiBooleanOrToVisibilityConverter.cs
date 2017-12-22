using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using DatabaseManager.Helpers.Extensions;

namespace DatabaseManager.Helpers.Converters
{
    public class MultiBooleanOrToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (EnumerableExtensions.IsNullOrEmpty(values))
                return Visibility.Collapsed;

            return values.Any(x => x is bool b && b)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) 
            => throw new NotImplementedException();
    }
}