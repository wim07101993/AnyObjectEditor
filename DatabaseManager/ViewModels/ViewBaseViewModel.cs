using System.Collections;
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

        private PropertyViewModel _title;
        private PropertyViewModel _subTitle;
        private PropertyViewModel _description;

        private ObservableCollection<IPropertyVM> _nativeProperties;
        private ObservableCollection<IEnumerable> _enumerableProperties;

        private ObservableCollection<PropertyViewModel> _lefOverProperties;

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

                SetProperty(ref _title, LeftOverProperties.SingleOrDefault(x => x.IsTitle), nameof(Title));
                LeftOverProperties -= (PropertyViewModel)Title;

                SetProperty(ref _subTitle, LeftOverProperties.SingleOrDefault(x => x.IsSubTitle), nameof(Subtitle));
                LeftOverProperties -= (PropertyViewModel)Subtitle;

                SetProperty(ref _description, LeftOverProperties.SingleOrDefault(x => x.IsDescription), nameof(Description));
                LeftOverProperties -= (PropertyViewModel)Description;

                NativeProperties = LeftOverProperties
                    .Where(x => x.Type.IsNativType())
                    .Cast<IPropertyVM>()
                    .ToCPObservableCollection();

                LeftOverProperties.RemoveWhere(x => NativeProperties.Any(y => x.Name == y.Name));

                if (_model is INotifyPropertyChanged afterUpdatePropertyChanged)
                    afterUpdatePropertyChanged.PropertyChanged += ModelPropertyChanged;
            }
        }

        public IPropertyVM Title => _title;

        public IPropertyVM Subtitle => _subTitle;

        public IPropertyVM Description => _description;

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

        public ObservableCollection<IEnumerable> EnumerableProperties
        {
            get => _enumerableProperties;
            private set
            {
                SetProperty(ref _enumerableProperties, value);
            }
        }

        public ObservableCollection<PropertyViewModel> LeftOverProperties
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