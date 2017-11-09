using System;
using System.Globalization;
using System.Windows.Data;
using DatabaseManager.Extensions;
using DatabaseManager.Properties;

namespace DatabaseManager.Views.Converters
{
    public class TypeToENativeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Type))
                throw new ArgumentException(Resources.valueNotOfTypeType,nameof(value));

            try
            {
                return ((Type) value).Name.ConvertToENativeType();
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}