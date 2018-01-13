using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Prism.Mvvm;
using Shared.ViewModelInterfaces;
using TypelessDatabaseManager.Models;

namespace TypelessDatabaseManager.ViewModels
{
    public class PropertyViewModel : BindableBase, IPropertyViewModel
    {
        #region FIELDS

        private object _value;

        #endregion FIELDS


        #region PROPERTIES

        public Property Property { get; }
  
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
        public bool IsId { get; }

        public bool IsReadOnly { get; }

        #endregion PROPERTIES


        #region CONSTRUCTORS

        public PropertyViewModel()
        {
        }

        public PropertyViewModel(Property property) : this()
        {
            Property = property;

            DisplayName = property.GetDisplayName();
            Name = property.Name;

            Value = property.Value;

            Type = property.Type;
            HasNativeType = property.HasNativeType();
            IsImage = property.HasImageType();
            IsColor = typeof(Color).IsAssignableFrom(property.Type);

            IsBrowsable = property.IsBrowsable();
            IsTitle = property.IsTitle();
            IsSubTitle = property.IsSubtitle();
            IsDescription = property.IsDescription();
            IsPicture = property.IsPicture();

            IsReadOnly = property.Writable;
        }
        
        #endregion CONSTRUCTORS


        #region METHODS

        public static IEnumerable<IPropertyViewModel> ConvertToProperties(IEnumerable<Property> propertyInfos,
            object parent)
            => propertyInfos.Select(x => new PropertyViewModel(x));
        
        protected override bool SetProperty<T>(ref T storage, T value, string propertyName = null)
        {
            var ret = base.SetProperty(ref storage, value, propertyName);
            RaisePropertyChanged(Name);
            return ret;
        }

        #endregion METHODS
    }
}