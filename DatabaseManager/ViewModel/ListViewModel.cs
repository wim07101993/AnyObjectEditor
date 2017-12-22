﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DatabaseManager.Helpers.Extensions;
using DatabaseManager.Models.Bases;
using DatabaseManager.Services;
using DatabaseManager.ViewModelInterfaces;
using MaterialDesignThemes.Wpf.Transitions;
using Prism.Commands;
using Prism.Mvvm;

namespace DatabaseManager.ViewModel
{
    public class ListViewModel : BindableBase, IListViewModel
    {
        #region FIELDS

        private readonly Func<Task<IEnumerable>> _getItemsFunc;
        private readonly Func<object, Task> _insertItemFunc;
        private readonly Func<object, Task> _updateItemFunc;
        private readonly Func<object, Task> _removeItemFunc;

        private IEnumerable _itemsSource;
        private IEnumerable<IObjectEditorViewModel> _convertedItemsSource;
        private IObjectEditorViewModel _emptyElement;
        private IObjectEditorViewModel _selectedItem;

        #endregion FIELDS


        #region PROPERTIES

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

                RaisePropertyChanged(nameof(IsListEditable));

                if (!EnumerableExtensions.IsNullOrEmpty(ItemsSource))
                    EmptyElement = new ObjectEditorViewModel(CreateNewItem());
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

        public ListViewModel(Func<Task<IEnumerable>> getItemsFunc, Func<object, Task> insertItemFunc,
            Func<object, Task> updateItemFunc, Func<object, Task> removeItemFunc)
        {
            Init();

            _getItemsFunc = getItemsFunc;
            Task.Factory.StartNew(async () => { return ItemsSource = await _getItemsFunc(); });

            _insertItemFunc = insertItemFunc;
            _updateItemFunc = updateItemFunc;
            _removeItemFunc = removeItemFunc;
        }

        public ListViewModel(IDataService dataService)
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

        public object CreateNewItem()
            => Activator.CreateInstance(ItemsSource
                .Cast<object>()
                .First()
                .GetType());

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

                Transitioner.MoveFirstCommand?.Execute(null, null);

                await _insertItemFunc(item);
            }

            ItemsSource = await _getItemsFunc();
        }

        #endregion METHODS
    }
}