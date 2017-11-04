using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Portable.Extensions;
using DatabaseManager.Helpers;

namespace DatabaseManager.Views.Controls
{
    public class ObjectEditor : UserControl
    {
        #region FIELDS

        #endregion FIELDS

        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            nameof(Model),
            typeof(object),
            typeof(ObjectEditor),
            new PropertyMetadata(default(object), OnModelChanged));

        #region headers

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(ObjectEditor),
            new PropertyMetadata(default(string), OnTitleChanged));

        public static readonly DependencyProperty TitlePropertyNameProperty = DependencyProperty.Register(
            nameof(TitlePropertyName),
            typeof(string),
            typeof(ObjectEditor),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
            nameof(Subtitle),
            typeof(string),
            typeof(ObjectEditor),
            new PropertyMetadata(default(string), OnSubtitleChanged));

        public static readonly DependencyProperty SubtitlePropertyNameProperty = DependencyProperty.Register(
            nameof(SubtitlePropertyName),
            typeof(string),
            typeof(ObjectEditor),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description),
            typeof(string),
            typeof(ObjectEditor),
            new PropertyMetadata(default(string), OnDescriptionChanged));

        public static readonly DependencyProperty DescriptionPropertyNameProperty = DependencyProperty.Register(
            nameof(DescriptionPropertyName),
            typeof(string),
            typeof(ObjectEditor),
            new PropertyMetadata(default(string)));

        #endregion headers

        #endregion DEPENDENCY PROPERTIES

        #region PROPERTIES

        public object Model
        {
            get => GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        #region headers

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string TitlePropertyName
        {
            get => (string) GetValue(TitlePropertyNameProperty);
            set => SetValue(TitlePropertyNameProperty, value);
        }

        public string Subtitle
        {
            get => (string) GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }

        public string SubtitlePropertyName
        {
            get => (string) GetValue(SubtitlePropertyNameProperty);
            set => SetValue(SubtitlePropertyNameProperty, value);
        }

        public string Description
        {
            get => (string) GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public string DescriptionPropertyName
        {
            get => (string) GetValue(DescriptionPropertyNameProperty);
            set => SetValue(DescriptionPropertyNameProperty, value);
        }

        #endregion headers

        #endregion PROPERTIES


        #region CONSTRUCTOR

        public ObjectEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ObjectEditor),
                new FrameworkPropertyMetadata(typeof(ObjectEditor)));
        }

        #endregion CONSTRUCTOR

        #region METHODS

        private static void OnModelChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var This = (ObjectEditor) obj;

            var model = args.NewValue;
            var properties = Property.ConvertToProperties(model.GetType().GetProperties(), model).ToCPList();

            foreach (var property in properties)
            {
                if (property.IsTitle)
                {
                    obj.SetValue(TitleProperty, property.Value);
                    continue;
                }
                if (property.IsSubTitle)
                {
                    obj.SetValue(SubtitleProperty, property.Value);
                    continue;
                }
                if (property.IsDescription)
                {
                    obj.SetValue(DescriptionProperty, property.Value);
                }
            }

            This.ModelChanged?.Invoke(This, new ValueChangedEventArgs(args.OldValue, args.NewValue));
        }

        private static void OnTitleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var model = obj.GetValue(ModelProperty);
            model.GetType()
                .GetProperty((string) obj.GetValue(TitlePropertyNameProperty))
                ?.SetValue(args.NewValue, model);
        }

        private static void OnSubtitleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var model = obj.GetValue(ModelProperty);
            model.GetType()
                .GetProperty((string) obj.GetValue(SubtitlePropertyNameProperty))
                ?.SetValue(args.NewValue, model);
        }

        private static void OnDescriptionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var model = obj.GetValue(ModelProperty);
            model.GetType()
                .GetProperty((string) obj.GetValue(SubtitlePropertyNameProperty))
                ?.SetValue(args.NewValue, model);
        }

        #endregion METHODS

        #region EVENTS

        public event EventHandler<ValueChangedEventArgs> ModelChanged;

        #endregion EVENTS
    }
}