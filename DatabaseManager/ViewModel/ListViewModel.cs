using System;
using System.Collections;
using System.Threading.Tasks;
using DatabaseManager.ViewModelInterfaces;
using Prism.Mvvm;

namespace DatabaseManager.ViewModel
{
    public class ListViewModel : BindableBase, IListViewModel
    {
        private IEnumerable _itemsSource;

        public IEnumerable ItemsSource
        {
            get => _itemsSource;
            private set => SetProperty(ref _itemsSource, value);
        }

        public ListViewModel()
        {
        }

        public ListViewModel(Func<Task<IEnumerable>> getItemsFunc)
        {
            Task.Factory.StartNew(async () =>
            {
                return ItemsSource = await getItemsFunc();
            });
        }
    }
}