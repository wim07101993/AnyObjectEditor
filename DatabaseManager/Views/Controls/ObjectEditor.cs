using System.Windows;
using System.Windows.Controls;

namespace DatabaseManager.Views.Controls
{
    public class ObjectEditor : Control
    {
        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(object),
            typeof(ObjectEditor),
            new PropertyMetadata(default(object)));

        #endregion DEPENDENCY PROPERTIES

        #region PROPERTIES

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        static ObjectEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ObjectEditor),
                new FrameworkPropertyMetadata(typeof(ObjectEditor)));
        }

        #endregion CONSTRUCTORS

        #region METHODS

        #endregion METHODS
    }
}