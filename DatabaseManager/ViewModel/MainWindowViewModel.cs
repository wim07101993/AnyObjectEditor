using DatabaseManager.Models;
using DatabaseManager.Services.DataService;
using DatabaseManager.Services.DataService.Mongo;
using DatabaseManager.ViewModelInterfaces;

namespace DatabaseManager.ViewModel
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        public IListViewModel<Person> ListViewModel { get; }
        public TypelessListViewModel TypelessListViewModel { get; set; }

        public IDataService<Person> PersonDataService { get; }
        public IDataService TypeLessDataService { get; }

        public MainWindowViewModel()
        {
            PersonDataService = new MongoDataService<Person>("mongodb://localhost:27017", "people", "people");

            ListViewModel = new ListViewModel<Person>(
                PersonDataService.GetAllAsync,
                PersonDataService.InsertAsync,
                PersonDataService.UpdateAsync,
                PersonDataService.RemoveAsync);

            TypeLessDataService = new MongoDataService("mongodb://localhost:27017", "people", "people", "attributes", "617474726962757465730001");

            TypelessListViewModel = new TypelessListViewModel(
                TypeLessDataService.GetAllAsync,
                TypeLessDataService.InsertAsync,
                TypeLessDataService.UpdateAsync,
                TypeLessDataService.RemoveAsync,
                TypeLessDataService.GetAttributesDictionary);
        }
    }
}