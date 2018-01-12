using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Shared.Helpers.Converters
{
    public class NumberEqualityToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var p = System.Convert.ToDouble(parameter);
                var v = System.Convert.ToDouble(value);
                return Math.Abs(p - v) < 0.000001
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            catch (Exception)
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}