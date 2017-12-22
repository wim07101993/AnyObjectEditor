using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IListViewModel<T> : INotifyPropertyChanged
    {
        IEnumerable<T> ItemsSource { get; set; }
        IEnumerable<IObjectEditorViewModel<T>> ConvertedItemsSource { get; }

        IObjectEditorViewModel<T> SelectedItem { get; set; }

        IObjectEditorViewModel<T> EmptyElement { get; set; }

        bool IsListEditable { get; }

        ICommand SaveCommand { get; }
        ICommand DeleteCommand { get; }
    }
}
