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
            var dataService = new MongoDataService("mongodb://localhost:27017", "people", "people");
            ListViewModel = new ListViewModel(async () => await dataService.GetAll<Person>());
        }
    }
}
