using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DatabaseManager.Helpers.Extensions;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;

namespace DatabaseManager.ViewModel
{
    public class TypelessListViewModel : BindableBase
    {
        #region FIELDS

        protected Func<Task<IEnumerable<JObject>>> GetItemsFunc;
        protected Func<JObject, Task> InsertItemFunc;
        protected Func<JObject, Task> UpdateItemFunc;
        protected Func<JObject, Task> RemoveItemFunc;

        private IDictionary<string, object> _attributes;
        private IEnumerable<JObject> _itemsSource;
        private IEnumerable<TypelessObjectEditorViewModel> _convertedItemsSource;
        private IEnumerable<TypelessObjectEditorViewModel> _filteredItemsSource;

        private TypelessObjectEditorViewModel _emptyElement;
        private TypelessObjectEditorViewModel _selectedItem;
        private string _searchString;
        private int _currentPage;

        #endregion FIELDS


        #region PROPERTIES

        public IDictionary<string, object> Attributes
        {
            get => _attributes;
            set
            {
                if (!SetProperty(ref _attributes, value))
                    return;
                ConvertItemsSource();
            }
        }

        public IEnumerable<JObject> ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (!SetProperty(ref _itemsSource, value))
                    return;
                ConvertItemsSource();
            }
        }

        public IEnumerable<TypelessObjectEditorViewModel> ConvertedItemsSource
        {
            get => _convertedItemsSource;
            private set
            {
                if (!SetProperty(ref _convertedItemsSource, value))
                    return;
                FilterItemsSource();
            }
        }

        public IEnumerable<TypelessObjectEditorViewModel> FilteredItemsSource
        {
            get => _filteredItemsSource;
            private set
            {
                if (!SetProperty(ref _filteredItemsSource, value))
                    return;
                RaisePropertyChanged(nameof(FilteredItemsSource));
            }
        }

        public TypelessObjectEditorViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public TypelessObjectEditorViewModel EmptyElement
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

        protected TypelessListViewModel()
        {
            Init();
        }

        public TypelessListViewModel(Func<Task<IEnumerable<JObject>>> getItemsFunc, Func<JObject, Task> insertItemFunc,
            Func<JObject, Task> updateItemFunc, Func<JObject, Task> removeItemFunc,
            IDictionary<string, object> attributes)
        {
            Init();
            Attributes = attributes;
            GetItemsFunc = getItemsFunc;
            Task.Factory.StartNew(async () => { return ItemsSource = await GetItemsFunc(); });

            InsertItemFunc = insertItemFunc;
            UpdateItemFunc = updateItemFunc;
            RemoveItemFunc = removeItemFunc;
        }

        protected void Init()
        {
            SaveCommand = new DelegateCommand(SaveAsync);
            DeleteCommand = new DelegateCommand<JObject>(DeleteAsync);
        }

        #endregion CONSTRUCTORS


        #region METHODS

        private void ConvertItemsSource()
        {
            ConvertedItemsSource = _itemsSource
                .Select(x => new TypelessObjectEditorViewModel(x, Attributes))
                .OrderBy(x => x.Title?.Value)
                .ThenBy(x => x.Subtitle?.Value)
                .ThenBy(x => x.Description?.Value);

            RaisePropertyChanged(nameof(IsListEditable));

            if (!EnumerableExtensions.IsNullOrEmpty(ItemsSource))
                EmptyElement = new TypelessObjectEditorViewModel(CreateNewItem(), Attributes);
        }

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
                    case nameof(TypelessObjectEditorViewModel.Title):
                        FilteredItemsSource = ConvertedItemsSource.Where(x => FilterOnTitle(x, split[1]));
                        return;
                    case nameof(TypelessObjectEditorViewModel.Subtitle):
                        FilteredItemsSource = ConvertedItemsSource.Where(x => FilterOnSubtitle(x, split[1]));
                        return;
                    case nameof(TypelessObjectEditorViewModel.Description):
                        FilteredItemsSource = ConvertedItemsSource.Where(x => FilterOnDescription(x, split[1]));
                        return;
                    case nameof(TypelessObjectEditorViewModel.KnownTypeProperties):
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

        private static bool FilterOnTitle(TypelessObjectEditorViewModel objectEditorViewModel, string filter)
            => objectEditorViewModel.Title.Value.ToString().Contains(filter);

        private static bool FilterOnSubtitle(TypelessObjectEditorViewModel objectEditorViewModel, string filter)
            => objectEditorViewModel.Subtitle.Value.ToString().Contains(filter);

        private static bool FilterOnDescription(TypelessObjectEditorViewModel objectEditorViewModel, string filter)
            => objectEditorViewModel.Description.Value.ToString().Contains(filter);

        private static bool FilterOnKnownTypeProperties(TypelessObjectEditorViewModel objectEditorViewModel,
            string filter)
            => objectEditorViewModel.KnownTypeProperties.Any(x => x.Value.ToString().Contains(filter));

        protected virtual JObject CreateNewItem()
            => Activator.CreateInstance<JObject>();

        public virtual async void RefreshAsync() => ItemsSource = await GetItemsFunc();

        public virtual async void DeleteAsync(JObject item)
        {
            await RemoveItemFunc(item);
            RefreshAsync();
        }

        public virtual async void SaveAsync()
        {
            if (SelectedItem != null)
                await UpdateItemFunc(SelectedItem.Value);
            else
            {
                var item = CreateNewItem();
                EmptyElement.Title?.PropertyInfo.SetValue(item, EmptyElement.Title.Value);
                EmptyElement.Subtitle?.PropertyInfo.SetValue(item, EmptyElement.Subtitle.Value);
                EmptyElement.Description?.PropertyInfo.SetValue(item, EmptyElement.Description.Value);
                EmptyElement.Picture?.PropertyInfo.SetValue(item, EmptyElement.Picture.Value);

                if (EnumerableExtensions.IsNullOrEmpty(EmptyElement.KnownTypeProperties))
                    foreach (var property in EmptyElement.KnownTypeProperties)
                        property.PropertyInfo.SetValue(item, property.Value);

                await InsertItemFunc(item);
            }

            RefreshAsync();

            CurrentPage = 0;
        }

        #endregion METHODS
    }
}