using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ClassLibrary.Portable.Extensions;
using ClassLibrary.Prism;
using DatabaseManager.Attributes;
using DatabaseManager.Extensions;
using DatabaseManager.Properties;
using DescriptionAttribute = DatabaseManager.Attributes.DescriptionAttribute;

namespace DatabaseManager
{
    public class Property : AObservableBase
    {
        #region FIELDS

        private object _value;

        #endregion FIELDS

        #region PROPERTIES

        public string Name { get; }

        public object Value
        {
            get => _value;
            set
            {
                if (!Type.IsInstanceOfType(value))
                    SetProperty(ref _value, value);
                else
                    throw new ArgumentException(Resources.Value_not_right_type_Error, nameof(value));
            }
        }

        public Type Type => Value.GetType();

        public bool IsEnumerable => Value is IEnumerable;
        public bool IsNativeType => Value.HasNativeType();

        public bool IsBrowsable { get; }
        public bool IsTitle { get; }
        public bool IsSubTitle { get; }
        public bool IsDescription { get; }
        public bool IsReadOnly { get; }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        public Property(PropertyInfo propertyInfo, object parent)
        {
            Name = propertyInfo.GetDisplayName();
            Value = propertyInfo.GetValue(parent);

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

        public static IEnumerable<Property> ConvertToProperties(IEnumerable<PropertyInfo> propertyInfos,
            object parent)
            => propertyInfos.Select(x => new Property(x, parent));

        public static IEnumerable<Property> GetPropertiesFromObject(object parent)
            => ConvertToProperties(parent.GetType().GetProperties(), parent);

        protected override bool SetProperty<T>(ref T storage, T value, string propertyName = null)
            => base.SetProperty(ref storage, value, Name);

        #endregion METHODS
    }
}