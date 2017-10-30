using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ClassLibrary.Portable.Extensions;
using ClassLibrary.Prism;
using DatabaseManager.Attributes;
using DatabaseManager.ViewModelInterfaces;
using DescriptionAttribute = DatabaseManager.Attributes.DescriptionAttribute;

namespace DatabaseManager.ViewModels
{
    public class PropertyViewModel : AObservableBase, IPropertyVM
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

            IsBrowsable = ((BrowsableAttribute) Attribute.GetCustomAttribute(
                              propertyInfo,
                              typeof(BrowsableAttribute)))
                          ?.Browsable != false;

            IsTitle = propertyInfo.HasAttribute(typeof(TitleAttribute));
            IsSubTitle = propertyInfo.HasAttribute(typeof(SubtitleAttribute));
            IsDescription = propertyInfo.HasAttribute(typeof(DescriptionAttribute));

            IsReadOnly = propertyInfo.CanWrite;
        }

        #endregion CONSTRUCTORS

        #region METHODS

        public static IEnumerable<PropertyViewModel> ConvertToProperties(IEnumerable<PropertyInfo> propertyInfos, object parent)
            => propertyInfos.Select(x => new PropertyViewModel(x, parent));

        public static IEnumerable<PropertyViewModel> GetPropertiesFromObject(object parent)
            => ConvertToProperties(parent.GetType().GetProperties(), parent);

        protected override bool SetProperty<T>(ref T storage, T value, string propertyName = null)
            => base.SetProperty(ref storage, value, Name);
        

        #endregion METHODS
    }
}