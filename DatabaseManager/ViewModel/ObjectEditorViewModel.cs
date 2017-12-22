using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DatabaseManager.Helpers.Extensions;
using DatabaseManager.ViewModelInterfaces;

namespace DatabaseManager.ViewModel
{
    public class ObjectEditorViewModel : IObjectEditorViewModel
    {
        private object _value;

        public IPropertyViewModel Title { get; private set; }
        public IPropertyViewModel Subtitle { get; private set; }
        public IPropertyViewModel Description { get; private set; }
        public IPropertyViewModel Picture { get; private set; }
        public IEnumerable<IPropertyViewModel> NativeProperties { get; private set; }

        public IHeaderViewModel HeaderViewModel { get; private set; }

        public object Value
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
                    Title = Title?.Value?.ToString(),
                    Subtitle = Subtitle?.Value?.ToString(),
                    Description = Description?.Value?.ToString(),
                    Picture = Picture?.Value is BitmapSource img ? img : null
                };
            }
        }


        public ObjectEditorViewModel()
        {
        }

        public ObjectEditorViewModel(object value)
        {
            Value = value;
        }
    }
}