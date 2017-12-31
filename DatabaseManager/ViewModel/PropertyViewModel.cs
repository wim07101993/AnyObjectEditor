using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
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

        public PropertyInfo PropertyInfo { get; }

        public string DisplayName { get; }
        public string Name { get; }

        public object Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public Type Type { get; }
        public bool HasNativeType { get; }
        public bool IsImage { get; }
        public bool IsColor { get; }

        public bool IsBrowsable { get; } = true;
        public bool IsTitle { get; }
        public bool IsSubTitle { get; }
        public bool IsDescription { get; }
        public bool IsPicture { get; }

        public bool IsReadOnly { get; }

        #endregion PROPERTIES


        #region CONSTRUCTORS

        public PropertyViewModel()
        {
        }

        public PropertyViewModel(PropertyInfo propertyInfo, object parent) : this()
        {
            PropertyInfo = propertyInfo;

            DisplayName = propertyInfo.GetDisplayName();
            Name = propertyInfo.Name;
            
            Value = propertyInfo.GetValue(parent);
            
            Type = propertyInfo.PropertyType;
            HasNativeType = propertyInfo.HasNativeType();
            IsImage = propertyInfo.HasImageType();
            IsColor = typeof(Color).IsAssignableFrom(propertyInfo.PropertyType);

            IsBrowsable = propertyInfo.IsBrowsable();
            IsTitle = propertyInfo.IsTitle();
            IsSubTitle = propertyInfo.IsSubtitle();
            IsDescription = propertyInfo.IsDescription();
            IsPicture = propertyInfo.IsPicture();

            IsReadOnly = propertyInfo.CanWrite;
        }

        #endregion CONSTRUCTORS


        #region METHODS

        public static IEnumerable<IPropertyViewModel> ConvertToProperties(IEnumerable<PropertyInfo> propertyInfos,
            object parent)
            => propertyInfos.Select(x => new PropertyViewModel(x, parent));

        public static IEnumerable<IPropertyViewModel> GetPropertiesFromObject(object parent)
            => ConvertToProperties(parent.GetType().GetProperties(), parent);

        protected override bool SetProperty<T>(ref T storage, T value, string propertyName = null)
        {
            var ret = base.SetProperty(ref storage, value, propertyName);
            RaisePropertyChanged(Name);
            return ret;
        }

        #endregion METHODS
    }
}