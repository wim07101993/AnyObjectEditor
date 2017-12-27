using System.Collections.Generic;
using System.ComponentModel;
using DatabaseManager.Helpers.Extensions;
using DatabaseManager.ViewModelInterfaces;
using Prism.Mvvm;

namespace DatabaseManager.ViewModel
{
    public class ObjectEditorViewModel<T> : BindableBase, IObjectEditorViewModel<T>, IObjectEditorViewModel
    {
        #region FIELDS

        private T _value;

        #endregion FIELDS


        #region PROPERTIES

        public IHeaderViewModel HeaderViewModel { get; private set; }

        public IPropertyViewModel Title { get; private set; }
        public IPropertyViewModel Subtitle { get; private set; }
        public IPropertyViewModel Description { get; private set; }
        public IPropertyViewModel Picture { get; private set; }

        public IEnumerable<IPropertyViewModel> NativeProperties { get; private set; }
        public IEnumerable<IPropertyViewModel> OtherProperties { get; private set; }

        object IObjectEditorViewModel.Value => Value;

        public T Value
        {
            get => _value;
            set
            {
                if (_value != null)
                    UnRegisterOnPropertyChanges();

                if (!SetProperty(ref _value, value) || value == null)
                    return;

                var properties = _value.GetType().GetProperties();
                var nativeProperties = new List<IPropertyViewModel>();
                var otherProperties = new List<IPropertyViewModel>();
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
                    else
                        otherProperties.Add(new PropertyViewModel(property, _value));
                }

                NativeProperties = nativeProperties;
                OtherProperties = otherProperties;

                RegisterOnPropertyChanges();

                HeaderViewModel = new HeaderViewModel
                {
                    Title = Title,
                    Subtitle = Subtitle,
                    Description = Description,
                    Picture = Picture
                };

                RaisePropertyChanged(nameof(Title));
                RaisePropertyChanged(nameof(Subtitle));
                RaisePropertyChanged(nameof(Description));
                RaisePropertyChanged(nameof(Picture));
                RaisePropertyChanged(nameof(NativeProperties));
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

        private void RegisterOnPropertyChanges()
        {
            if (Title != null) Title.PropertyChanged += OnPropertyChanged;
            if (Subtitle != null) Subtitle.PropertyChanged += OnPropertyChanged;
            if (Description != null) Description.PropertyChanged += OnPropertyChanged;
            if (Picture != null) Picture.PropertyChanged += OnPropertyChanged;

            foreach (var property in NativeProperties)
                property.PropertyChanged += OnPropertyChanged;
        }

        public void UnRegisterOnPropertyChanges()
        {
            Title.PropertyChanged -= OnPropertyChanged;
            Subtitle.PropertyChanged -= OnPropertyChanged;
            Description.PropertyChanged -= OnPropertyChanged;
            Picture.PropertyChanged -= OnPropertyChanged;

            foreach (var property in NativeProperties)
                property.PropertyChanged -= OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = (IPropertyViewModel) sender;
            property.PropertyInfo.SetValue(Value, property.Value);
        }

        #endregion METHODS
    }
}