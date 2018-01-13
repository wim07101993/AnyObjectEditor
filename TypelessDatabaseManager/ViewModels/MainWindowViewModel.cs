using Shared.Services;
using Shared.ViewModelInterfaces;
using TypelessDatabaseManager.Models;
using TypelessDatabaseManager.Services.DataService.Mongo;

namespace TypelessDatabaseManager.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel<Object>
    {
        public IListViewModel<Object> ListViewModel { get; }

        public IDataService<Object> DataService { get; }

        public MainWindowViewModel()
        {
            DataService = new MongoDataService("mongodb://localhost:27017", "people", "people",
                "617474726962757465730001", "attributes");

            ListViewModel = new ListViewModel(DataService);
        }
    }
}