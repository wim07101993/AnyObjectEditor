using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Shared.ViewModelInterfaces;

namespace TypelessDatabaseManager.Views.Controls
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

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            ((ICommand) DataContext
                    .GetType()
                    .GetProperties()
                    .SingleOrDefault(x => x.Name == nameof(IListViewModel<object>.SaveCommand) &&
                                          typeof(ICommand).IsAssignableFrom(x.PropertyType))
                    ?.GetValue(DataContext))
                ?.Execute(null);
        }
    }
}