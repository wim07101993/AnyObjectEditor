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
        #region CONSTRUCTOR

        public EditableTextBlock()
        {
            InitializeComponent();
        }

        #endregion CONSTRUCTOR

        #region METHODS

        private void OnTextBlockMouseUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock.Visibility = Visibility.Collapsed;
            TextBox.Visibility = Visibility.Visible;
            TextBox.Focus();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBlock.Text = TextBox.Text;
        }

        private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox.Visibility = Visibility.Collapsed;
            TextBlock.Visibility = Visibility.Visible;
        }

        #endregion METHODS
    }
}