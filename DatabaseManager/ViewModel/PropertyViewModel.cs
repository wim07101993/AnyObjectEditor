﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DatabaseManager.Helpers.Extensions;
using DatabaseManager.ViewModelInterfaces;
using Prism.Mvvm;

namespace DatabaseManager.ViewModel
{
    public class PropertyViewModel : BindableBase, IPropertyViewModel
    {
        #region FIELDS

        private object _value;

        #endregion FIELDS

        #region PROPERTIES

        public string Name { get; }

        public object Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public Type Type { get; }

        public bool IsBrowsable { get; } = true;
        public bool IsTitle { get; }
        public bool IsSubTitle { get; }
        public bool IsDescription { get; }
        public bool IsReadOnly { get; }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        public PropertyViewModel()
        {
        }

        public PropertyViewModel(PropertyInfo propertyInfo, object parent) : this()
        {
            Name = propertyInfo.GetDisplayName();
            Value = propertyInfo.GetValue(parent);
            Type = propertyInfo.PropertyType;

            IsBrowsable = propertyInfo.IsBrowsable();
            IsTitle = propertyInfo.IsTitle();
            IsSubTitle = propertyInfo.IsSubtitle();
            IsDescription = propertyInfo.IsDescription();

            IsReadOnly = propertyInfo.CanWrite;
        }

        #endregion CONSTRUCTORS

        #region METHODS

        public static IEnumerable<IPropertyViewModel> ConvertToProperties(IEnumerable<PropertyInfo> propertyInfos,
            object parent)
            => propertyInfos.Select(x => new PropertyViewModel(x, parent));

        public static IEnumerable<IPropertyViewModel> GetPropertiesFromObject(object parent)
            => ConvertToProperties(parent.GetType().GetProperties(), parent);
        
        #endregion METHODS
    }
}