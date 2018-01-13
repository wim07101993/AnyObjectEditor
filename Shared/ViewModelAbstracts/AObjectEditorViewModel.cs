using System.Collections.Generic;
using System.ComponentModel;
using Prism.Mvvm;
using Shared.Helpers.Extensions;
using Shared.ViewModelInterfaces;

namespace Shared.ViewModelAbstracts
{
    public abstract class AObjectEditorViewModel<T> : BindableBase, IObjectEditorViewModel<T>
    {
        private T _value;

        #region FIELDS

        #endregion FIELDS

        #region PROPERTIES

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

        public virtual IHeaderViewModel HeaderViewModel { get; protected set; }

        public virtual IPropertyViewModel Title { get; protected set; }
        public virtual IPropertyViewModel Subtitle { get; protected set; }
        public virtual IPropertyViewModel Description { get; protected set; }
        public virtual IPropertyViewModel Picture { get; protected set; }

        public virtual IEnumerable<IPropertyViewModel> KnownTypeProperties { get; protected set; }
        public virtual IEnumerable<IPropertyViewModel> OtherProperties { get; protected set; }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        #endregion CONSTRUCTORS

        #region METHODS

        protected abstract void CreatePropertiesFromValue();

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
            if (Title != null)
                Title.PropertyChanged += OnPropertyChanged;
            if (Subtitle != null)
                Subtitle.PropertyChanged += OnPropertyChanged;
            if (Description != null)
                Description.PropertyChanged += OnPropertyChanged;
            if (Picture != null)
                Picture.PropertyChanged += OnPropertyChanged;

            if (!EnumerableExtensions.IsNullOrEmpty(KnownTypeProperties))
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

        protected abstract void OnPropertyChanged(object sender, PropertyChangedEventArgs e);

        #endregion METHODS
    }
}