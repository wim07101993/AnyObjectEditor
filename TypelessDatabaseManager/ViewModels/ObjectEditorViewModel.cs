using System.Collections.Generic;
using System.ComponentModel;
using Shared.ViewModelAbstracts;
using Shared.ViewModelInterfaces;
using TypelessDatabaseManager.Models;

namespace TypelessDatabaseManager.ViewModels
{
    public class ObjectEditorViewModel : AObjectEditorViewModel<Object>
    {
        public ObjectEditorViewModel()
        {   
        }

        public ObjectEditorViewModel(Object value)
        {
            Value = value;
        }


        protected override void CreatePropertiesFromValue()
        {
            var properties = Value.Properties;
            var knownTypeProperties = new List<IPropertyViewModel>();
            var otherProperties = new List<IPropertyViewModel>();

            foreach (var property in properties.Values)
            {
                if (!property.IsBrowsable())
                    continue;

                var propertyVM = new PropertyViewModel(property);
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
            
        }
    }
}