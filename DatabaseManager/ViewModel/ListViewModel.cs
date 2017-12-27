using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DatabaseManager.Services;

namespace DatabaseManager.ViewModel
{
    public class ListViewModel : ListViewModel<object>
    {
        #region CONSTRUCTORS

        protected ListViewModel()
        {
        }

        public ListViewModel(Func<Task<IEnumerable>> getItemsFunc, Func<object, Task> insertItemFunc,
            Func<object, Task> updateItemFunc, Func<object, Task> removeItemFunc)
            : base(async () => (await getItemsFunc()).Cast<object>(), insertItemFunc, updateItemFunc, removeItemFunc)
        {
        }

        public ListViewModel(IDataService dataService)
        {
            Init();

            GetItemsFunc = async () => await dataService.GetAllAsync();
            Task.Factory.StartNew(async () => { ItemsSource = await GetItemsFunc(); });

            InsertItemFunc = dataService.InsertAsync;
            UpdateItemFunc = dataService.UpdateAsync;
            RemoveItemFunc = dataService.RemoveAsync;
        }

        #endregion CONSTRUCTORS


        #region METHODS

        protected override object CreateNewItem()
            => Activator.CreateInstance(ItemsSource.First().GetType());

        #endregion METHODS
    }
}