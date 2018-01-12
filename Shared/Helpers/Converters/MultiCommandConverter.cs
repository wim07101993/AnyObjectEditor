using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Commands;

namespace Shared.Helpers.Converters
{
    public class MultiCommandConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var command = new DelegateCommand<object>(x =>
            {
                foreach (var value in values)
                    if (value is ICommand c && c.CanExecute(x))
                        c.Execute(x);
            });

            return command;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) 
            => throw new NotImplementedException();
    }
}
