using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Portable.Extensions;

namespace DatabaseManager.Views.Controls
{
    public class ObjectEditor : Control
    {
        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(object), typeof(ObjectEditor), new PropertyMetadata(default(object), OnValueChanged));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(Property), typeof(ObjectEditor), new PropertyMetadata(default(Property)));

        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
            nameof(Subtitle), typeof(Property), typeof(ObjectEditor), new PropertyMetadata(default(Property)));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(Property), typeof(ObjectEditor), new PropertyMetadata(default(Property)));

        public static readonly DependencyProperty NativePropertiesProperty = DependencyProperty.Register(
            nameof(NativeProperties), typeof(IEnumerable), typeof(ObjectEditor), new PropertyMetadata(default(IEnumerable)));

        public static readonly DependencyProperty TabsProperty = DependencyProperty.Register(
            "Tabs", typeof(IEnumerable), typeof(ObjectEditor), new PropertyMetadata(default(IEnumerable)));

        #endregion DEPENDENCY PROPERTIES

        #region PROPERTIES

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public Property Title
        {
            get => (Property) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public Property Subtitle
        {
            get => (Property) GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }

        public Property Description
        {
            get => (Property) GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        private PropertyList NativeProperties
        {
            get => (PropertyList)GetValue(NativePropertiesProperty);
            set => SetValue(NativePropertiesProperty, value); 
        }

        private IEnumerable Tabs
        {
            get => (IEnumerable)GetValue(TabsProperty);
            set => SetValue(TabsProperty, value);
        }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        static ObjectEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ObjectEditor),
                new FrameworkPropertyMetadata(typeof(ObjectEditor)));
        }

        #endregion CONSTRUCTORS

        #region METHODS

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var model = obj.GetValue(ValueProperty);
            var properties = Property.ConvertToProperties(model.GetType().GetProperties(), model)
                .Where(x => x.IsBrowsable)
                .ToCPList();

            var headerProperties = GetAndSetHeaderProperties(obj, properties);
            properties -= headerProperties;

            var nativeProperties = new PropertyList(properties.Where(x => x.IsNativeType));
            obj.SetValue(NativePropertiesProperty, nativeProperties);
            properties.RemoveRange(nativeProperties);
            
            var tabs = new List<object> {nativeProperties};
            tabs.AddRange(properties);
            obj.SetValue(TabsProperty, tabs);
        }

        private static IEnumerable<Property> GetAndSetHeaderProperties(DependencyObject obj,
            IEnumerable<Property> properties)
        {
            var ret = new ClassLibrary.Portable.Collections.List<Property>();

            foreach (var p in properties)
            {
                if (p.IsTitle)
                    obj.SetValue(TitleProperty, p);
                else if (p.IsSubTitle)
                    obj.SetValue(SubtitleProperty, p);
                else if (p.IsDescription)
                    obj.SetValue(DescriptionProperty, p);
                else
                    continue;

                ret += p;
            }

            return ret;
        }

        #endregion METHODS
    }
}