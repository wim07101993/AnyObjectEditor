using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Shared.Helpers.Extensions;
using Shared.ViewModelInterfaces;

namespace Shared.ViewModelAbstracts
{
    public abstract class AListViewModel<T> : BindableBase, IListViewModel<T>
    {
        #region FIELDS

        protected Func<Task<IEnumerable<T>>> GetItemsFunc;
        protected Func<T, Task> InsertItemFunc;
        protected Func<T, Task> UpdateItemFunc;
        protected Func<T, Task> RemoveItemFunc;

        private IEnumerable<T> _itemsSource;
        private IEnumerable<IObjectEditorViewModel<T>> _convertedItemsSource;
        private IEnumerable<IObjectEditorViewModel<T>> _filteredItemsSource;

        private IObjectEditorViewModel<T> _emptyElement;
        private IObjectEditorViewModel<T> _selectedItem;
        private string _searchString;
        private int _currentPage;

        #endregion FIELDS


        #region PROPERTIES

        public virtual IEnumerable<T> ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (!SetProperty(ref _itemsSource, value))
                    return;

                ConvertedItemsSource = _itemsSource
                    .Select(ConvertToObjectEditor)
                    .OrderBy(x => x.Title?.Value)
                    .ThenBy(x => x.Subtitle?.Value)
                    .ThenBy(x => x.Description?.Value);

                RaisePropertyChanged(nameof(IsListEditable));

                if (!EnumerableExtensions.IsNullOrEmpty(ItemsSource))
                    EmptyElement = ConvertToObjectEditor(CreateNewItem());
            }
        }

        public IEnumerable<IObjectEditorViewModel<T>> ConvertedItemsSource
        {
            get => _convertedItemsSource;
            protected set
            {
                if (!SetProperty(ref _convertedItemsSource, value))
                    return;
                FilterItemsSource();
            }
        }

        public IEnumerable<IObjectEditorViewModel<T>> FilteredItemsSource
        {
            get => _filteredItemsSource;
            protected set
            {
                if (!SetProperty(ref _filteredItemsSource, value))
                    return;
                RaisePropertyChanged(nameof(FilteredItemsSource));
            }
        }

        public IObjectEditorViewModel<T> SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public IObjectEditorViewModel<T> EmptyElement
        {
            get => _emptyElement;
            set => SetProperty(ref _emptyElement, value);
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

        public int CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        #endregion PROPERTIES


        #region CONSTRUCTORS

        protected AListViewModel()
        {
            Init();
        }

        protected AListViewModel(Func<Task<IEnumerable<T>>> getItemsFunc, Func<T, Task> insertItemFunc,
            Func<T, Task> updateItemFunc, Func<T, Task> removeItemFunc)
        {
            Init();

            GetItemsFunc = getItemsFunc;
            Task.Factory.StartNew(async () => { ItemsSource = await GetItemsFunc(); });

            InsertItemFunc = insertItemFunc;
            UpdateItemFunc = updateItemFunc;
            RemoveItemFunc = removeItemFunc;
        }

        protected void Init()
        {
            SaveCommand = new DelegateCommand(SaveAsync);
            DeleteCommand = new DelegateCommand<T>(DeleteAsync);
        }

        #endregion CONSTRUCTORS


        #region METHODS

        public virtual async void RefreshAsync() => ItemsSource = await GetItemsFunc();

        public virtual async void DeleteAsync(T item)
        {
            await RemoveItemFunc(item);
            RefreshAsync();
        }

        public abstract void SaveAsync();


        protected abstract T CreateNewItem();

        protected abstract IObjectEditorViewModel<T> ConvertToObjectEditor(T item);


        protected void FilterItemsSource()
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
                    case nameof(IObjectEditorViewModel<T>.Title):
                        FilteredItemsSource = ConvertedItemsSource.Where(x => FilterOnTitle(x, split[1]));
                        return;
                    case nameof(IObjectEditorViewModel<T>.Subtitle):
                        FilteredItemsSource = ConvertedItemsSource.Where(x => FilterOnSubtitle(x, split[1]));
                        return;
                    case nameof(IObjectEditorViewModel<T>.Description):
                        FilteredItemsSource = ConvertedItemsSource.Where(x => FilterOnDescription(x, split[1]));
                        return;
                    case nameof(IObjectEditorViewModel<T>.KnownTypeProperties):
                        FilteredItemsSource = ConvertedItemsSource.Where(x => FilterOnKnownTypeProperties(x, split[1]));
                        return;
                }

            var titleFiltered = ConvertedItemsSource.Where(x => FilterOnTitle(x, SearchString));
            var subtitleFiltered = ConvertedItemsSource.Where(x => FilterOnSubtitle(x, SearchString));
            var descriptionFiltered = ConvertedItemsSource.Where(x => FilterOnDescription(x, SearchString));
            var nativePropertiesFiltered =
                ConvertedItemsSource.Where(x => FilterOnKnownTypeProperties(x, SearchString));

            var filteredItems = titleFiltered.ToList();
            var allOtherItems = subtitleFiltered.ToList();
            allOtherItems.AddRange(descriptionFiltered);
            allOtherItems.AddRange(nativePropertiesFiltered);

            foreach (var item in allOtherItems)
                if (filteredItems.Any(x => x.Value.Equals(item.Value)))
                    filteredItems.Add(item);

            FilteredItemsSource = filteredItems;
        }

        protected static bool FilterOnTitle(IObjectEditorViewModel<T> objectEditorViewModel, string filter)
            => objectEditorViewModel.Title.Value.ToString().Contains(filter);

        protected static bool FilterOnSubtitle(IObjectEditorViewModel<T> objectEditorViewModel, string filter)
            => objectEditorViewModel.Subtitle.Value.ToString().Contains(filter);

        protected static bool FilterOnDescription(IObjectEditorViewModel<T> objectEditorViewModel, string filter)
            => objectEditorViewModel.Description.Value.ToString().Contains(filter);

        protected static bool FilterOnKnownTypeProperties(IObjectEditorViewModel<T> objectEditorViewModel,
            string filter)
            => objectEditorViewModel.KnownTypeProperties.Any(x => x.Value.ToString().Contains(filter));

        #endregion METHODS
    }
}