using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace DatabaseManager.Views.Controls
{
    public class NativeTypeEditorList : Control
    {
        #region FIELDS

        #endregion FIELDS

        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource), typeof(IEnumerable), typeof(NativeTypeEditorList),
            new PropertyMetadata(default(IEnumerable)));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation), typeof(Orientation), typeof(NativeTypeEditorList),
            new PropertyMetadata(default(Orientation)));

        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
            nameof(ItemMargin), typeof(Thickness), typeof(NativeTypeEditorList),
            new PropertyMetadata(default(Thickness)));

        #endregion DEPENDENCY PROPERTIES

        #region PROPERTIES

        public IEnumerable ItemsSource
        {
            get => (IEnumerable) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public Orientation Orientation
        {
            get => (Orientation) GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public Thickness ItemMargin
        {
            get => (Thickness) GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        static NativeTypeEditorList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NativeTypeEditorList),
                new FrameworkPropertyMetadata(typeof(NativeTypeEditorList)));
        }

        #endregion CONSTRUCTORS

        #region METHODS

        #endregion METHODS
    }
}