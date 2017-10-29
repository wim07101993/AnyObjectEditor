using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ClassLibrary.Portable.Collections;
using ClassLibrary.Portable.Extensions;
using ClassLibrary.Prism;
using DatabaseManager.Attributes;
using DatabaseManager.Helpers;
using DatabaseManager.Models;
using DatabaseManager.ViewModelInterfaces;
using DescriptionAttribute = DatabaseManager.Attributes.DescriptionAttribute;

namespace DatabaseManager.ViewModels
{
    public class ViewBaseViewModel : AObservableBase, IViewBaseVM
    {
        #region FIELDS

        private object _model;

        private IPropertyVM _title;
        private IPropertyVM _subTitle;
        private IPropertyVM _description;

        private ObservableCollection<IPropertyVM> _nativeProperties;
        private ObservableCollection<IPropertyVM> _lefOverProperties;

        #endregion FIELDS

        #region PROPERTIES

        public object Model
        {
            get => _model;
            set
            {
                if (_model is INotifyPropertyChanged beforUpdatePropertyChanged)
                    beforUpdatePropertyChanged.PropertyChanged -= ModelPropertyChanged;

                SetProperty(ref _model, value);

                LeftOverProperties = PropertyViewModel.GetPropertiesFromObject(_model).ToCPObservableCollection();

                Title = LeftOverProperties.SingleOrDefault(x => x.IsTitle);
                LeftOverProperties -= Title;

                Subtitle = LeftOverProperties.SingleOrDefault(x => x.IsSubTitle);
                LeftOverProperties -= Subtitle;

                Description = LeftOverProperties.SingleOrDefault(x => x.IsDescription);
                LeftOverProperties -= Description;

                NativeProperties = LeftOverProperties
                    .Where(x => x.Type.IsNativType())
                    .ToCPObservableCollection();

                LeftOverProperties.RemoveWhere(x => NativeProperties.Any(y => x.Name == y.Name));

                if (_model is INotifyPropertyChanged afterUpdatePropertyChanged)
                    afterUpdatePropertyChanged.PropertyChanged += ModelPropertyChanged;
            }
        }

        public IPropertyVM Title
        {
            get => _title;
            private set => SetProperty(ref _title, value);
        }

        public IPropertyVM Subtitle
        {
            get => _subTitle;
            private set => SetProperty(ref _subTitle, value);
        }

        public IPropertyVM Description
        {
            get => _description;
            private set => SetProperty(ref _description, value);
        }

        public ObservableCollection<IPropertyVM> NativeProperties
        {
            get => _nativeProperties;
            private set
            {
                if (Equals(_nativeProperties, value))
                    return;

                if (_nativeProperties != null)
                {
                    RemoveListenersFromProperties(_nativeProperties);
                    _nativeProperties.CollectionChanged -= NativePropertiesCollectionChanged;
                }

                SetProperty(ref _nativeProperties, value);

                _nativeProperties.CollectionChanged += NativePropertiesCollectionChanged;
                AddListenersToProperties(_nativeProperties);
            }
        }

        public ObservableCollection<IPropertyVM> LeftOverProperties
        {
            get => _lefOverProperties;
            private set => SetProperty(ref _lefOverProperties, value);
        }

        #endregion PROPERTIES

        #region CONSTRUCTOR

        public ViewBaseViewModel()
        {
            Model = new SomeModel();
            ((SomeModel) Model).Double = 3.14;
        }

        #endregion CONSTRUCTOR

        #region METHODS

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var prop = _model.GetType()
                .GetProperties()
                .SingleOrDefault(x => x.Name == e.PropertyName);

            if (prop == null)
                return;

            var val  = prop.GetValue(_model);

            if (prop.HasAttribute(typeof(TitleAttribute)))
                Title.Value = val;
            if (prop.HasAttribute(typeof(SubtitleAttribute)))
                Subtitle.Value = val;
            if (prop.HasAttribute(typeof(DescriptionAttribute)))
                Description.Value = val;

            var name = prop.GetDisplayName();
            if (val.HasNativeType())
                NativeProperties
                    .Where(x => x.Name == name)
                    .ToList()
                    .ForEach(y => y.Value = val);
            else
                LeftOverProperties
                    .Where(x => x.Name == name)
                    .ToList()
                    .ForEach(y => y.Value = val);
        }

        private void NativePropertiesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveListenersFromProperties(e.OldItems.Cast<PropertyViewModel>());
            AddListenersToProperties(e.NewItems.Cast<PropertyViewModel>());
        }

        private void AddListenersToProperties(IEnumerable<IPropertyVM> properties)
        {
            foreach (var p in properties)
                p.PropertyChanged += SomePropertyChanged;
        }

        private void RemoveListenersFromProperties(IEnumerable<IPropertyVM> properties)
        {
            foreach (var p in properties)
                p.PropertyChanged -= SomePropertyChanged;
        }

        private void SomePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _model.GetType()
                .GetProperties()
                .SingleOrDefault(x => x.GetDisplayName() == e.PropertyName)
                ?.SetValue(_model, ((PropertyViewModel) sender).Value);
        }

        #endregion METHODS
    }
}