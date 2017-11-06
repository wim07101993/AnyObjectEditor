using System;
using System.Globalization;
using System.Windows.Data;

namespace DatabaseManager.Views.Converters
{
    public class ObjectToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ToDouble(value);
            }
            catch (FormatException)
            {
            }
            catch (InvalidCastException)
            {
            }
            catch (OverflowException)
            {
            }
            return default(double);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => value;
    }

    public class ObjectToCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value?.ToString();
            return s?.Length == 1
                ? s[0]
                : default(char);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => value;
    }
}