using System.Collections.Generic;
using System.ComponentModel;
using DatabaseManager.Helpers.Extensions;
using DatabaseManager.ViewModelInterfaces;
using Prism.Mvvm;

namespace DatabaseManager.ViewModel
{
    public class ObjectEditorViewModel<T> : BindableBase, IObjectEditorViewModel<T>
    {
        #region FIELDS

        private T _value;

        #endregion FIELDS


        #region PROPERTIES

        public IHeaderViewModel HeaderViewModel { get; protected set; }

        public IPropertyViewModel Title { get; protected set; }
        public IPropertyViewModel Subtitle { get; protected set; }
        public IPropertyViewModel Description { get; protected set; }
        public IPropertyViewModel Picture { get; protected set; }

        public IEnumerable<IPropertyViewModel> KnownTypeProperties { get; protected set; }
        public IEnumerable<IPropertyViewModel> OtherProperties { get; protected set; }

        public virtual T Value
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

        public ObjectEditorViewModel()
        {
        }

        public ObjectEditorViewModel(T value)
        {
            Value = value;
        }

        #endregion CONSTRUCTOR


        #region METHODS

        protected virtual void CreatePropertiesFromValue()
        {
            var properties = _value.GetType().GetProperties();
            var knownTypeProperties = new List<IPropertyViewModel>();
            var otherProperties = new List<IPropertyViewModel>();

            foreach (var property in properties)
            {
                if (!property.IsBrowsable())
                    continue;

                var propertyVM = new PropertyViewModel(property, _value);
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

        protected virtual void RaiseAllPropertyChanges()
        {
            RaisePropertyChanged(nameof(Title));
            RaisePropertyChanged(nameof(Subtitle));
            RaisePropertyChanged(nameof(Description));
            RaisePropertyChanged(nameof(Picture));
            RaisePropertyChanged(nameof(KnownTypeProperties));
            RaisePropertyChanged(nameof(OtherProperties));
        }

        protected void RegisterOnPropertyChanges()
        {
            if (Title != null) Title.PropertyChanged += OnPropertyChanged;
            if (Subtitle != null) Subtitle.PropertyChanged += OnPropertyChanged;
            if (Description != null) Description.PropertyChanged += OnPropertyChanged;
            if (Picture != null) Picture.PropertyChanged += OnPropertyChanged;

            foreach (var property in KnownTypeProperties)
                property.PropertyChanged += OnPropertyChanged;
        }

        protected void UnRegisterOnPropertyChanges()
        {
            Title.PropertyChanged -= OnPropertyChanged;
            Subtitle.PropertyChanged -= OnPropertyChanged;
            Description.PropertyChanged -= OnPropertyChanged;
            Picture.PropertyChanged -= OnPropertyChanged;

            foreach (var property in KnownTypeProperties)
                property.PropertyChanged -= OnPropertyChanged;
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = (IPropertyViewModel) sender;
            property.PropertyInfo.SetValue(Value, property.Value);
        }

        #endregion METHODS
    }
}