using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace DatabaseManager.Views.Controls
{
    public class ListEditor : UserControl
    {
        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(IEnumerable),
            typeof(ListEditor),
            new PropertyMetadata(default(object)));

        #endregion DEPENDENCY PROPERTIES

        #region PROPERTIES

        public IEnumerable Value
        {
            get => (IEnumerable) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        #endregion PROPERTIES

        #region CONSTRUCTOR

        public ListEditor()
        {
            AddChild(new TextBlock {Text = "This is a listeditor."});
        }

        public ListEditor(IEnumerable value) : this()
        {
        }

        #endregion CONSTRUCTOR
    }
}