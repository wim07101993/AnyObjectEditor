using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace DatabaseManager.ViewModel
{
    public class TypelessObjectEditorViewModel : ObjectEditorViewModel<JObject>
    {
        #region FIELDS

        private JObject _value;
        private readonly IDictionary<string, object> _attributes;

        #endregion FIELDS

        #region PROPERTIES

        public override JObject Value
        {
            get => _value;
            set
            {
                if (_value != null)
                    UnRegisterOnPropertyChanges();

                if (!SetProperty(ref _value, value) || value == null)
                    return;

                CreatePropertiesFromValue();
                RegisterOnPropertyChanges();
                RaiseAllPropertyChanges();
            }
        }

        #endregion PROPERTIES


        #region CONSTRUCTOR

        protected override void CreatePropertiesFromValue()
        {
            var knownTypeProperties = new List<PropertyViewModel>();
            var otherProperties = new List<PropertyViewModel>();

            foreach (var propertyName in ((IDictionary<string, JToken>) _value).Keys)
            {
                var propertyVM = new PropertyViewModel(propertyName, _value[propertyName], _attributes);

                if (!propertyVM.IsBrowsable)
                    continue;

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

        public TypelessObjectEditorViewModel()
        {
        }

        public TypelessObjectEditorViewModel(JObject value, IDictionary<string, object> attributes)
        {
            _attributes = attributes;
            Value = value;
        }

        #endregion CONSTRUCTOR


        #region METHODS

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = (PropertyViewModel) sender;
            property.PropertyInfo.SetValue(Value, property.Value);
        }

        #endregion METHODS
    }
}