using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DatabaseManager.Helpers.Extensions;
using DatabaseManager.Services;
using DatabaseManager.ViewModelInterfaces;
using MaterialDesignThemes.Wpf.Transitions;
using Prism.Commands;
using Prism.Mvvm;

namespace DatabaseManager.ViewModel
{
    public class ListViewModel<T> : BindableBase, IListViewModel<T>, IListViewModel
    {
        #region FIELDS

        private readonly Func<Task<IEnumerable<T>>> _getItemsFunc;
        private readonly Func<T, Task> _insertItemFunc;
        private readonly Func<T, Task> _updateItemFunc;
        private readonly Func<T, Task> _removeItemFunc;

        private IEnumerable<T> _itemsSource;
        private IEnumerable<IObjectEditorViewModel<T>> _convertedItemsSource;
        private IObjectEditorViewModel<T> _emptyElement;
        private IObjectEditorViewModel<T> _selectedItem;

        #endregion FIELDS


        #region PROPERTIES

        public IEnumerable<T> ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (!SetProperty(ref _itemsSource, value))
                    return;

                ConvertedItemsSource = _itemsSource
                    .Select(x => new ObjectEditorViewModel<T>(x));

                RaisePropertyChanged(nameof(IsListEditable));

                if (!EnumerableExtensions.IsNullOrEmpty(ItemsSource))
                    EmptyElement = new ObjectEditorViewModel<T>(CreateNewItem());
            }
        }

        IEnumerable IListViewModel.ItemsSource
        {
            get => ItemsSource;
            set => ItemsSource = value.Cast<T>();
        }

        public IEnumerable<IObjectEditorViewModel<T>> ConvertedItemsSource
        {
            get => _convertedItemsSource;
            private set => SetProperty(ref _convertedItemsSource, value);
        }

        IEnumerable<IObjectEditorViewModel> IListViewModel.ConvertedItemsSource
            => ConvertedItemsSource.Cast<IObjectEditorViewModel>();

        public IObjectEditorViewModel<T> SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        IObjectEditorViewModel IListViewModel.SelectedItem
        {
            get => SelectedItem as IObjectEditorViewModel;
            set => SelectedItem = (IObjectEditorViewModel<T>) value;
        }

        public IObjectEditorViewModel<T> EmptyElement
        {
            get => _emptyElement;
            set => SetProperty(ref _emptyElement, value);
        }

        IObjectEditorViewModel IListViewModel.EmptyElement
        {
            get => EmptyElement as IObjectEditorViewModel;
            set => EmptyElement = (IObjectEditorViewModel<T>)value;
        }

        public bool IsListEditable
            => ItemsSource is IList;

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        #endregion PROPERTIES


        #region CONSTRUCTORS

        protected ListViewModel()
        {
            Init();
        }

        public ListViewModel(Func<Task<IEnumerable<T>>> getItemsFunc, Func<T, Task> insertItemFunc,
            Func<T, Task> updateItemFunc, Func<T, Task> removeItemFunc)
        {
            Init();

            _getItemsFunc = getItemsFunc;
            Task.Factory.StartNew(async () => { return ItemsSource = await _getItemsFunc(); });

            _insertItemFunc = insertItemFunc;
            _updateItemFunc = updateItemFunc;
            _removeItemFunc = removeItemFunc;
        }

        public ListViewModel(IDataService<T> dataService)
        {
            Init();
            _getItemsFunc = async () => await dataService.GetAllAsync();
            Task.Factory.StartNew(async () => { return ItemsSource = await _getItemsFunc(); });

            _insertItemFunc = dataService.InsertAsync;
            _updateItemFunc = dataService.UpdateAsync;
            _removeItemFunc = dataService.RemoveAsync;
        }

        private void Init()
        {
            SaveCommand = new DelegateCommand(Save);
            DeleteCommand = new DelegateCommand(Delete);
        }

        #endregion CONSTRUCTORS


        #region METHODS

        private T CreateNewItem()
            => Activator.CreateInstance<T>();

        public void Delete()
        {
        }

        public async void Save()
        {
            if (SelectedItem != null)
                await _updateItemFunc(SelectedItem.Value);
            else
            {
                var item = CreateNewItem();
                EmptyElement.Title?.PropertyInfo.SetValue(item, EmptyElement.Title.Value);
                EmptyElement.Subtitle?.PropertyInfo.SetValue(item, EmptyElement.Subtitle.Value);
                EmptyElement.Description?.PropertyInfo.SetValue(item, EmptyElement.Description.Value);
                EmptyElement.Picture?.PropertyInfo.SetValue(item, EmptyElement.Picture.Value);

                if (EnumerableExtensions.IsNullOrEmpty(EmptyElement.NativeProperties))
                    foreach (var property in EmptyElement.NativeProperties)
                        property.PropertyInfo.SetValue(item, property.Value);

                await _insertItemFunc(item);
            }

            ItemsSource = await _getItemsFunc();
            Transitioner.MoveFirstCommand?.Execute(null, null);
        }

        #endregion METHODS
    }
}