using System.Windows;
using System.Windows.Controls;

namespace DatabaseManager.Views.Controls
{
    public class EditableTextBlock : UserControl
    {
        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(EditableTextBlock),
            new PropertyMetadata(default(string)));
        
        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register(
            nameof(WaterMark),
            typeof(string),
            typeof(EditableTextBlock),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IsInEditingModeProperty = DependencyProperty.Register(
            nameof(IsInEditingMode),
            typeof(bool),
            typeof(EditableTextBlock),
            new PropertyMetadata(default(bool)));
        
        #endregion DEPENDENCY PROPERTIES

        #region PROPERTIES

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string WaterMark
        {
            get => (string) GetValue(WaterMarkProperty);
            set => SetValue(WaterMarkProperty, value);
        }

        public bool IsInEditingMode
        {
            get => (bool) GetValue(IsInEditingModeProperty);
            set => SetValue(IsInEditingModeProperty, value);
        }
        
        #endregion PROPERTIES

        #region CONSTRUCTOR

        static EditableTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableTextBlock),
                new FrameworkPropertyMetadata(typeof(EditableTextBlock)));
        }

        public EditableTextBlock()
        {
           MouseDown += (sender, args) => IsInEditingMode = true;
           LostFocus += (sender, args) => IsInEditingMode = false;
        }

        #endregion CONSTRUCTOR

        #region METHODS
        
        #endregion METHODS
    }
}