using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using Shared.Helpers;
using Shared.Helpers.Attributes;
using Shared.Helpers.Extensions;

namespace TypelessDatabaseManager.Models
{
    public class Property
    {
        #region FIELDS

        private Object _value;

        #endregion FIELDS


        #region PROPERTIES

        public string Name { get; }

        public Object Value
        {
            get => _value;
            set
            {
                if (Type != null && value != null && !Type.IsInstanceOfType(value))
                    throw new ArgumentException("Value not of the right type");

                var args = new ValueChangedEventArgs(_value, value);
                _value = value;
                ValueChanged?.Invoke(this, args);
            }
        }

        public Type Type { get; }

        public bool Readable { get; }
        public bool Writable { get; }

        public IEnumerable<IAttribute> Attributes { get; }

        #endregion PROPERTIES


        #region CONSTRUCTORS

        public Property(string name, bool readable, bool writable, IEnumerable<IAttribute> attributes,
            Object value = null)
        {
            Name = name;
            Value = value;
            Type = Value?.Value?.GetType();
            Readable = readable;
            Writable = writable;
            Attributes = attributes;
        }

        #endregion CONSTRUCTORS


        #region METHODS

        public bool HasAttribute<T>()
            => Attributes.Any(x => x is T);

        public bool IsId()
        {
            var attr = Attributes.FirstOrDefault(x => x is IdAttribute);
            return attr != null && (bool) attr.Value || attr == null && IdAttribute.Default.Id;
        }

        public bool IsTitle()
        {
            var attr = Attributes.FirstOrDefault(x => x is TitleAttribute);
            return attr != null && (bool) attr.Value || attr == null && TitleAttribute.Default.Title;
        }

        public bool IsSubtitle()
        {
            var attr = Attributes.FirstOrDefault(x => x is SubtitleAttribute);
            return attr != null && (bool) attr.Value || attr == null && SubtitleAttribute.Default.Subtitle;
        }

        public bool IsDescription()
        {
            var attr = Attributes.FirstOrDefault(x => x is DescriptionAttribute);
            return attr != null && (bool) attr.Value || attr == null && DescriptionAttribute.Default.Description;
        }

        public bool IsPicture()
        {
            var attr = Attributes.FirstOrDefault(x => x is PictureAttribute);
            return attr != null && (bool) attr.Value || attr == null && PictureAttribute.Default.Picture;
        }

        public bool IsBrowsable()
        {
            var attr = Attributes.FirstOrDefault(x => x is BrowsableAttribute);
            return attr != null && (bool) attr.Value || attr == null && BrowsableAttribute.Default.Browsable;
        }

        public bool HasNativeType()
            => Type.IsNativeType();

        public bool HasImageType()
            => typeof(BitmapImage).IsAssignableFrom(Type);

        public string GetDisplayName()
        {
            var displayName = Attributes.FirstOrDefault(x => x is DisplayNameAttribute);
            return displayName != null
                ? (string) displayName.Value
                : Name;
        }

        #endregion METHODS


        #region EVENTS

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion EVENTS
    }
}