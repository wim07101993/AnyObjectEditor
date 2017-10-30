using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DatabaseManager.Views.Controls
{
    /// <inheritdoc cref="System.Windows.Controls.UserControl" />
    /// <summary>
    /// Interaction logic for EditableTextBlock.xaml
    /// </summary>
    public partial class EditableTextBlock
    {
        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty IsInEditingModeProperty = DependencyProperty.Register(
            nameof(IsInEditingMode),
            typeof(bool),
            typeof(EditableTextBlock),
            new PropertyMetadata(default(bool), OnEditingModeChanged));
        
        #endregion DEPENDENCY PROPERTIES

        #region PROPERTIES

        public bool IsInEditingMode
        {
            get => (bool)GetValue(IsInEditingModeProperty);
            set => SetValue(IsInEditingModeProperty, value);
        }

        #endregion PROPERTIES

        #region CONSTRUCTOR

        public EditableTextBlock()
        {
            InitializeComponent();
        }

        #endregion CONSTRUCTOR

        #region METHODS

        private void OnTextBlockMouseUp(object sender, MouseButtonEventArgs e)
        {
            IsInEditingMode = true;
            TextBox.Focus();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBlock.Text = TextBox.Text;

            if (!IsInEditingMode)
                IsInEditingMode = string.IsNullOrWhiteSpace(TextBox.Text);
        }

        private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            IsInEditingMode = false;
            SetEditingMode(false);
        }


        private static void OnEditingModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((EditableTextBlock)obj).SetEditingMode((bool)args.NewValue);
        }

        private void SetEditingMode(bool value)
        {
            if (value || string.IsNullOrWhiteSpace(TextBox.Text))
            {
                TextBlock.Visibility = Visibility.Collapsed;
                TextBox.Visibility = Visibility.Visible;
            }
            else
            {
                TextBox.Visibility = Visibility.Collapsed;
                TextBlock.Visibility = Visibility.Visible;
            }
        }

        #endregion METHODS
    }
}