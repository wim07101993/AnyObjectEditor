using System;
using System.Globalization;
using System.Windows.Data;

namespace Shared.Helpers.Converters
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
}
