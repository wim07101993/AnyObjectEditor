using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Shared.ViewModelInterfaces
{
    public interface IListViewModel<T> : INotifyPropertyChanged
    {
        IEnumerable<T> ItemsSource { get; set; }
        IEnumerable<IObjectEditorViewModel<T>> ConvertedItemsSource { get; }
        IEnumerable<IObjectEditorViewModel<T>> FilteredItemsSource { get; }

        IObjectEditorViewModel<T> SelectedItem { get; set; }

        IObjectEditorViewModel<T> EmptyElement { get; set; }

        bool IsListEditable { get; }
        string SearchString { get; set; }

        ICommand SaveCommand { get; }
        ICommand DeleteCommand { get; }

        int CurrentPage { get; set; }
    }
}
