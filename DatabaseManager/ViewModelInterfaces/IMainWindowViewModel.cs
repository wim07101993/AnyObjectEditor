using DatabaseManager.Models;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IMainWindowViewModel
    {
        IListViewModel<Person> ListViewModel { get; }
    }
}