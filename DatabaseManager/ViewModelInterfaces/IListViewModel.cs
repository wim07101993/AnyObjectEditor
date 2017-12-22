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

        IObjectEditorViewModel SelectedItem { get; set; }

        IObjectEditorViewModel EmptyElement { get; set; }

        bool IsListEditable { get; }

        ICommand SaveCommand { get; }
        ICommand DeleteCommand { get; }
    }
}
