using System.Windows;

namespace DatabaseManager.Views.Controls
{
    public class NumericTextBoxChangedRoutedEventArgs : RoutedEventArgs
    {
        public double Interval { get; set; }

        public NumericTextBoxChangedRoutedEventArgs(RoutedEvent routedEvent, double interval) : base(routedEvent)
        {
            Interval = interval;
        }
    }
}
