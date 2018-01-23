using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Helpers.Extensions;
using Shared.Services;
using Shared.ViewModelAbstracts;
using Shared.ViewModelInterfaces;
using Object = TypelessDatabaseManager.Models.Object;

namespace TypelessDatabaseManager.ViewModels
{
    public class ListViewModel : AListViewModel<Object>
    {
        private IEnumerable<Object> _itemsSource;

        public override IEnumerable<Object> ItemsSource
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


        public ListViewModel()
        {
        }

        public ListViewModel(Func<Task<IEnumerable<Object>>> getItemsFunc, Func<Object, Task> insertItemFunc,
            Func<Object, Task> updateItemFunc, Func<Object, Task> removeItemFunc)
            : base(getItemsFunc, insertItemFunc, updateItemFunc, removeItemFunc)
        {
        }

        public ListViewModel(IDataService<Object> dataService)
            : base(dataService.GetAllAsync, dataService.InsertAsync, dataService.UpdateAsync, dataService.RemoveAsync)
        {
        }


        public override async void SaveAsync()
        {
            if (SelectedItem != null)
                await UpdateItemFunc(SelectedItem.Value);
            else
            {
                // TODO
            }

            RefreshAsync();

            CurrentPage = 0;
        }

        protected override Object CreateNewItem()
        {
            var obj = new Object(ItemsSource?.FirstOrDefault().Clone());
            obj.ClearValues();
            return obj;
        }

        protected override IObjectEditorViewModel<Object> ConvertToObjectEditor(Object item)
            => new ObjectEditorViewModel(item);
    }
}