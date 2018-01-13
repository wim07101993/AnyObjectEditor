using DatabaseManager.Models;
using DatabaseManager.Services.DataService.Mongo;
using Shared.Services;
using Shared.ViewModelInterfaces;

namespace DatabaseManager.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel<Person>
    {
        public IListViewModel<Person> ListViewModel { get; }

        public IDataService<Person> PersonDataService { get; }

        public MainWindowViewModel()
        {
            PersonDataService = new MongoDataService<Person>("mongodb://localhost:27017", "people", "people");

            ListViewModel = new ListViewModel<Person>(PersonDataService);
        }
    }
}