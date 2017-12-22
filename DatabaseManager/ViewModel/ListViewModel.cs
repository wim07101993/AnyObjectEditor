using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DatabaseManager.ViewModelInterfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace DatabaseManager.ViewModel
{
    public class ListViewModel : BindableBase, IListViewModel
    {
        private IEnumerable _itemsSource;
        private IEnumerable<IObjectEditorViewModel> _convertedItemsSource;

        public IEnumerable ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (!SetProperty(ref _itemsSource, value))
                    return;

                ConvertedItemsSource = _itemsSource
                    .Cast<object>()
                    .Select(x => new ObjectEditorViewModel(x));
            }
        }

        public IEnumerable<IObjectEditorViewModel> ConvertedItemsSource
        {
            get => _convertedItemsSource;
            private set => SetProperty(ref _convertedItemsSource, value);
        }

        public ICommand AddCommand { get; private set; }


        public ListViewModel()
        {
            Init();
        }

        public ListViewModel(Func<Task<IEnumerable>> getItemsFunc)
        {
            Init();
            Task.Factory.StartNew(async () => { return ItemsSource = await getItemsFunc(); });
        }

        private void Init()
        {
            AddCommand = new DelegateCommand(Add);
        }

        public void Add()
        {
            
        }
    }
}