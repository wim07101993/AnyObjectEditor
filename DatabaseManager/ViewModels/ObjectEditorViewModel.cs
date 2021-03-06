﻿using System.Collections.Generic;
using System.ComponentModel;
using Shared.Helpers.Extensions;
using Shared.ViewModelAbstracts;
using Shared.ViewModelInterfaces;

namespace DatabaseManager.ViewModels
{
    public class ObjectEditorViewModel<T> : AObjectEditorViewModel<T>
    {
        #region CONSTRUCTOR

        public ObjectEditorViewModel()
        {
        }

        public ObjectEditorViewModel(T value)
        {
            Value = value;
        }

        #endregion CONSTRUCTOR


        #region METHODS

        protected override void CreatePropertiesFromValue()
        {
            var properties = Value.GetType().GetProperties();
            var knownTypeProperties = new List<IPropertyViewModel>();
            var otherProperties = new List<IPropertyViewModel>();

            foreach (var property in properties)
            {
                if (!property.IsBrowsable())
                    continue;

                var propertyVM = new PropertyViewModel(property, Value);
                if (propertyVM.IsTitle)
                    Title = propertyVM;
                else if (propertyVM.IsSubTitle)
                    Subtitle = propertyVM;
                else if (propertyVM.IsDescription)
                    Description = propertyVM;
                else if (propertyVM.IsPicture)
                    Picture = propertyVM;
                else if (propertyVM.HasNativeType || propertyVM.IsImage || propertyVM.IsColor)
                    knownTypeProperties.Add(propertyVM);
                else
                    otherProperties.Add(propertyVM);
            }

            KnownTypeProperties = knownTypeProperties;
            OtherProperties = otherProperties;

            HeaderViewModel = new HeaderViewModel
            {
                Title = Title,
                Subtitle = Subtitle,
                Description = Description,
                Picture = Picture
            };
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = sender as PropertyViewModel;
            property?.PropertyInfo.SetValue(Value, property.Value);
        }

        #endregion METHODS
    }
}