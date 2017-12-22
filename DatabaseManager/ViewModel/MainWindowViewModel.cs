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
                async () => (await dataService.GetAllAsync()).OrderBy(x => x.FirstName),
                dataService.InsertAsync,
                dataService.UpdateAsync,
                dataService.RemoveAsync);
        }
    }
}