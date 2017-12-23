using System.Windows;
using System.Windows.Controls;

namespace DatabaseManager.Views.Controls
{
    public partial class List
    {
        public List()
        {
            InitializeComponent();
        }

        private void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Transitioner.SelectedIndex == 0 && ListBox.SelectedItem != null)
                Transitioner.SelectedIndex = 1;
        }

        private void OnTransitionerSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Transitioner.SelectedIndex != 0)
                return;

            ListBox.SelectedItem = null;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Transitioner.SelectedIndex = 0;
        }
    }
}