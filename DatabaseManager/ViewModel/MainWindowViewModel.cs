using System.Linq;
using DatabaseManager.Models;
using DatabaseManager.Services;
using DatabaseManager.ViewModelInterfaces;

namespace DatabaseManager.ViewModel
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        public IListViewModel ListViewModel { get; }

        public MainWindowViewModel()
        {
            IDataService<Person> dataService =
                new MongoDataService<Person>("mongodb://localhost:27017", "people", "people");

            ListViewModel = new ListViewModel<Person>(
                dataService.GetAllAsync,
                dataService.InsertAsync,
                dataService.UpdateAsync,
                dataService.RemoveAsync);
        }
    }
}