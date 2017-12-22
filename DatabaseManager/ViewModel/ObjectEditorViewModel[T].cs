﻿using System;
using System.Collections.Generic;
using DatabaseManager.Helpers.Extensions;
using DatabaseManager.ViewModelInterfaces;

namespace DatabaseManager.ViewModel
{
    public class ObjectEditorViewModel<T> : IObjectEditorViewModel<T>, IObjectEditorViewModel
    {
        private T _value;

        public IHeaderViewModel HeaderViewModel { get; private set; }

        public IPropertyViewModel Title { get; private set; }
        public IPropertyViewModel Subtitle { get; private set; }
        public IPropertyViewModel Description { get; private set; }
        public IPropertyViewModel Picture { get; private set; }
        public IEnumerable<IPropertyViewModel> NativeProperties { get; private set; }

        object IObjectEditorViewModel.Value
            => Value;

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value))
                    return;

                _value = value;

                var properties = _value.GetType().GetProperties();
                var nativeProperties = new List<IPropertyViewModel>();
                foreach (var property in properties)
                {
                    if (property.IsTitle())
                        Title = new PropertyViewModel(property, _value);
                    else if (property.IsSubtitle())
                        Subtitle = new PropertyViewModel(property, _value);
                    else if (property.IsDescription())
                        Description = new PropertyViewModel(property, _value);
                    else if (property.IsPicture())
                        Picture = new PropertyViewModel(property, _value);
                    else if (property.HasNativeType())
                        nativeProperties.Add(new PropertyViewModel(property, _value));
                }

                NativeProperties = nativeProperties;

                HeaderViewModel = new HeaderViewModel
                {
                    Title = Title,
                    Subtitle = Subtitle,
                    Description = Description,
                    Picture = Picture
                };
            }
        }


        public ObjectEditorViewModel()
        {
        }

        public ObjectEditorViewModel(T value)
        {
            Value = value;
        }
    }
}