using System.Collections;
using System.ComponentModel;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IListViewModel : INotifyPropertyChanged
    {
        IEnumerable ItemsSource { get; }
    }
}
