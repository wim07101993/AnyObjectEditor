using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Helpers.Extensions;
using Shared.Services;
using Shared.ViewModelAbstracts;
using Shared.ViewModelInterfaces;

namespace DatabaseManager.ViewModels
{
    public class ListViewModel<T> : AListViewModel<T>
    {
        protected ListViewModel()
        {
        }

        public ListViewModel(Func<Task<IEnumerable<T>>> getItemsFunc, Func<T, Task> insertItemFunc,
            Func<T, Task> updateItemFunc, Func<T, Task> removeItemFunc)
            : base(getItemsFunc, insertItemFunc, updateItemFunc, removeItemFunc)
        {
        }

        public ListViewModel(IDataService<T> dataService)
            : base(dataService.GetAllAsync, dataService.InsertAsync, dataService.UpdateAsync, dataService.RemoveAsync)
        {
        }


        public override async void SaveAsync()
        {
            if (SelectedItem != null)
                await UpdateItemFunc(SelectedItem.Value);
            else
            {
                var item = CreateNewItem();
                (EmptyElement.Title as PropertyViewModel)?.PropertyInfo.SetValue(item, EmptyElement.Title.Value);
                (EmptyElement.Subtitle as PropertyViewModel)?.PropertyInfo.SetValue(item, EmptyElement.Subtitle.Value);
                (EmptyElement.Description as PropertyViewModel)?.PropertyInfo.SetValue(item,
                    EmptyElement.Description.Value);
                (EmptyElement.Picture as PropertyViewModel)?.PropertyInfo.SetValue(item, EmptyElement.Picture.Value);

                if (EnumerableExtensions.IsNullOrEmpty(EmptyElement.KnownTypeProperties))
                    foreach (var property in EmptyElement.KnownTypeProperties)
                        (property as PropertyViewModel)?.PropertyInfo.SetValue(item, property.Value);

                await InsertItemFunc(item);
            }

            RefreshAsync();

            CurrentPage = 0;
        }

        protected override T CreateNewItem()
            => Activator.CreateInstance<T>();

        protected override IObjectEditorViewModel<T> ConvertToObjectEditor(T item)
            => new ObjectEditorViewModel<T>(item);
    }
}