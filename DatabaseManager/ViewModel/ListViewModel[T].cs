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
        private IEnumerable<IObjectEditorViewModel<T>> _filteredItemsSource;

        private IObjectEditorViewModel<T> _emptyElement;
        private IObjectEditorViewModel<T> _selectedItem;
        private string _searchString;

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
                    .Select(x => new ObjectEditorViewModel<T>(x))
                    .OrderBy(x => x.Title?.Value)
                    .ThenBy(x => x.Subtitle?.Value)
                    .ThenBy(x => x.Description?.Value);

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
            private set
            {
                if (!SetProperty(ref _convertedItemsSource, value))
                    return;
                FilterItemsSource();
            }
        }

        IEnumerable<IObjectEditorViewModel> IListViewModel.ConvertedItemsSource
            => ConvertedItemsSource.Cast<IObjectEditorViewModel>();

        public IEnumerable<IObjectEditorViewModel<T>> FilteredItemsSource
        {
            get => _filteredItemsSource;
            private set
            {
                if (!SetProperty(ref _filteredItemsSource, value))
                    return;
                RaisePropertyChanged(nameof(FilteredItemsSource));
            }
        }

        IEnumerable<IObjectEditorViewModel> IListViewModel.FilteredItemsSource
            => FilteredItemsSource.Cast<IObjectEditorViewModel>();

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
            set => EmptyElement = (IObjectEditorViewModel<T>) value;
        }

        public bool IsListEditable
            => ItemsSource is IList;

        public string SearchString
        {
            get => _searchString;
            set
            {
                if (!SetProperty(ref _searchString, value))
                    return;
                FilterItemsSource();
            }
        }

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
            DeleteCommand = new DelegateCommand<T>(Delete);
        }

        #endregion CONSTRUCTORS


        #region METHODS

        private void FilterItemsSource()
        {
            if (string.IsNullOrWhiteSpace(SearchString))
            {
                FilteredItemsSource = ConvertedItemsSource;
                return;
            }

            var split = SearchString.Split(':');
            if (split.Length > 1)
                switch (split[0])
                {
                    case nameof(ObjectEditorViewModel.Title):
                        FilteredItemsSource = ConvertedItemsSource.Where(x => FilterOnTitle(x, split[1]));
                        return;
                    case nameof(ObjectEditorViewModel.Subtitle):
                        FilteredItemsSource = ConvertedItemsSource.Where(x => FilterOnSubtitle(x, split[1]));
                        return;
                    case nameof(ObjectEditorViewModel.Description):
                        FilteredItemsSource = ConvertedItemsSource.Where(x => FilterOnDescription(x, split[1]));
                        return;
                    case nameof(ObjectEditorViewModel.NativeProperties):
                        FilteredItemsSource = ConvertedItemsSource.Where(x => FilterOnNativeProperties(x, split[1]));
                        return;
                }
            
            var titleFiltered = ConvertedItemsSource.Where(x => FilterOnTitle(x, SearchString));
            var subtitleFiltered = ConvertedItemsSource.Where(x => FilterOnSubtitle(x, SearchString));
            var descriptionFiltered = ConvertedItemsSource.Where(x => FilterOnDescription(x, SearchString));
            var nativePropertiesFiltered = ConvertedItemsSource.Where(x => FilterOnNativeProperties(x, SearchString));

            var filteredItems = titleFiltered.ToList();
            var allOtherItems = subtitleFiltered.ToList();
            allOtherItems.AddRange(descriptionFiltered);
            allOtherItems.AddRange(nativePropertiesFiltered);

            foreach (var item in allOtherItems)
                if (filteredItems.Any(x => x.Value.Equals(item.Value)))
                    filteredItems.Add(item);

            FilteredItemsSource = filteredItems;
        }

        private static bool FilterOnTitle(IObjectEditorViewModel<T> objectEditorViewModel, string filter)
            => objectEditorViewModel.Title.Value.ToString().Contains(filter);

        private static bool FilterOnSubtitle(IObjectEditorViewModel<T> objectEditorViewModel, string filter)
            => objectEditorViewModel.Subtitle.Value.ToString().Contains(filter);

        private static bool FilterOnDescription(IObjectEditorViewModel<T> objectEditorViewModel, string filter)
            => objectEditorViewModel.Description.Value.ToString().Contains(filter);

        private static bool FilterOnNativeProperties(IObjectEditorViewModel<T> objectEditorViewModel, string filter)
            => objectEditorViewModel.NativeProperties.Any(x => x.Value.ToString().Contains(filter));

        private static T CreateNewItem()
            => Activator.CreateInstance<T>();

        public async void Refresh() => ItemsSource = await _getItemsFunc();

        public async void Delete(T item)
        {
            await _removeItemFunc(item);
            Refresh();
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

            Refresh();
            Transitioner.MoveFirstCommand?.Execute(null, null);
        }

        #endregion METHODS
    }
}