using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DatabaseManager.Helpers.Extensions;
using DatabaseManager.ViewModelInterfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace DatabaseManager.ViewModel
{
    public class ListViewModel : BindableBase, IListViewModel
    {
        private IEnumerable _itemsSource;
        private IEnumerable<IObjectEditorViewModel> _convertedItemsSource;
        private IObjectEditorViewModel _emptyElement;
        private IObjectEditorViewModel _selectedItem;

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

                if (!EnumerableExtensions.IsNullOrEmpty(ItemsSource))
                    EmptyElement = ObjectEditorViewModel.CreateEmpty(ItemsSource.Cast<object>().First());
            }
        }

        public IEnumerable<IObjectEditorViewModel> ConvertedItemsSource
        {
            get => _convertedItemsSource;
            private set => SetProperty(ref _convertedItemsSource, value);
        }

        public IObjectEditorViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public IObjectEditorViewModel EmptyElement
        {
            get => _emptyElement;
            set => SetProperty(ref _emptyElement, value);
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }


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
            SaveCommand = new DelegateCommand<IObjectEditorViewModel>(Save);
            DeleteCommand = new DelegateCommand<IObjectEditorViewModel>(Delete);
        }

        public void Delete(IObjectEditorViewModel element)
        {

        }

        public void Save(IObjectEditorViewModel element)
        {
        }
    }
}