using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IListViewModel : INotifyPropertyChanged
    {
        IEnumerable ItemsSource { get; set; }
        IEnumerable<IObjectEditorViewModel> ConvertedItemsSource { get; }

        ICommand AddCommand { get; }
    }
}
