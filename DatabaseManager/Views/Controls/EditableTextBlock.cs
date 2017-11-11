using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace DatabaseManager.Views.Controls
{
    public class EditableTextBlock : Control
    {
        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(EditableTextBlock),
                new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IsInEditingModeProperty =
            DependencyProperty.Register(nameof(IsInEditingMode), typeof(bool), typeof(EditableTextBlock),
                new PropertyMetadata(default(bool), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var b = false;
            if (b)
                throw new NotImplementedException();
        }

        public static readonly DependencyProperty HasTextProperty = DependencyProperty.Register(
            nameof(HasText), typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(EditableTextBlock), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register(nameof(TextWrapping), typeof(bool), typeof(EditableTextBlock),
                new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register(
            nameof(CaretBrush), typeof(Brush), typeof(EditableTextBlock),
            new PropertyMetadata(GetDefaultSelectionBrush()));

        public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.Register(
            nameof(CharacterCasing), typeof(CharacterCasing), typeof(EditableTextBlock),
            new PropertyMetadata(default(CharacterCasing)));

        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register(
            nameof(MaxLines), typeof(int), typeof(EditableTextBlock), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(
            nameof(MaxLength), typeof(int), typeof(EditableTextBlock), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty MinLinesProperty = DependencyProperty.Register(
            nameof(MinLines), typeof(int), typeof(EditableTextBlock), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
            nameof(TextAlignment), typeof(TextAlignment), typeof(EditableTextBlock),
            new PropertyMetadata(default(TextAlignment)));

        public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register(
            nameof(TextDecorations), typeof(TextDecorationCollection), typeof(EditableTextBlock),
            new PropertyMetadata(default(TextDecorationCollection)));

        public static readonly DependencyProperty TextButtonProperty = DependencyProperty.Register(
            nameof(TextButton), typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty SelectionBrushProperty = DependencyProperty.Register(
            nameof(SelectionBrush), typeof(Brush), typeof(EditableTextBlock),
            new PropertyMetadata(GetDefaultSelectionBrush()));

        #region button

        public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.Register(
            nameof(ButtonCommand), typeof(ICommand), typeof(EditableTextBlock),
            new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty ButtonCommandParameterProperty = DependencyProperty.Register(
            nameof(ButtonCommandParameter), typeof(object), typeof(EditableTextBlock),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register(
            nameof(ButtonContent), typeof(object), typeof(EditableTextBlock), new PropertyMetadata("r"));

        public static readonly DependencyProperty ButtonContentTemplateProperty = DependencyProperty.Register(
            nameof(ButtonContentTemplate), typeof(DataTemplate), typeof(EditableTextBlock),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty ButtonFontFamilyProperty = DependencyProperty.Register(
            nameof(ButtonFontFamily), typeof(FontFamily), typeof(EditableTextBlock),
            new PropertyMetadata(new FontFamilyConverter().ConvertFromString("Marlett")));

        public static readonly DependencyProperty ButtonFontSizeProperty = DependencyProperty.Register(
            nameof(ButtonFontSize), typeof(double), typeof(EditableTextBlock),
            new PropertyMetadata(SystemFonts.MessageFontSize));

        public static readonly DependencyProperty ButtonTemplateProperty = DependencyProperty.Register(
            nameof(ButtonTemplate), typeof(ControlTemplate), typeof(EditableTextBlock),
            new PropertyMetadata(default(ControlTemplate)));

        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register(
            nameof(ButtonWidth), typeof(double), typeof(EditableTextBlock), new PropertyMetadata(22d));

        public static readonly DependencyProperty ButtonsAlignmentProperty = DependencyProperty.Register(
            nameof(ButtonsAlignment), typeof(ButtonsAlignment), typeof(EditableTextBlock),
            new PropertyMetadata(ButtonsAlignment.Right));
        
        public static readonly DependencyProperty ClearTextButtonProperty = DependencyProperty.Register(
            nameof(ClearTextButton), typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(default(bool)));

        #endregion button

        #endregion DEPENDENCY PROPERTIES

        #region PROPERTIES

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string PropertyName
        {
            get => (string) GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public bool IsInEditingMode
        {
            get => (bool) GetValue(IsInEditingModeProperty);
            set => SetValue(IsInEditingModeProperty, value);
        }

        public bool TextWrapping
        {
            get => (bool) GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        public Brush SelectionBrush
        {
            get => (Brush) GetValue(SelectionBrushProperty);
            set => SetValue(SelectionBrushProperty, value);
        }

        public bool TextButton
        {
            get => (bool) GetValue(TextButtonProperty);
            set => SetValue(TextButtonProperty, value);
        }

        public TextDecorationCollection TextDecorations
        {
            get => (TextDecorationCollection) GetValue(TextDecorationsProperty);
            set => SetValue(TextDecorationsProperty, value);
        }

        public TextAlignment TextAlignment
        {
            get => (TextAlignment) GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        public int MinLines
        {
            get => (int) GetValue(MinLinesProperty);
            set => SetValue(MinLinesProperty, value);
        }

        public int MaxLength
        {
            get => (int) GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public int MaxLines
        {
            get => (int) GetValue(MaxLinesProperty);
            set => SetValue(MaxLinesProperty, value);
        }

        public CharacterCasing CharacterCasing
        {
            get => (CharacterCasing) GetValue(CharacterCasingProperty);
            set => SetValue(CharacterCasingProperty, value);
        }

        public Brush CaretBrush
        {
            get => (Brush) GetValue(CaretBrushProperty);
            set => SetValue(CaretBrushProperty, value);
        }
        
        public bool HasText
        {
            get => (bool) GetValue(HasTextProperty);
            set => SetValue(HasTextProperty, value);
        }

        #region button

        public bool ClearTextButton
        {
            get => (bool) GetValue(ClearTextButtonProperty);
            set => SetValue(ClearTextButtonProperty, value);
        }

        public ButtonsAlignment ButtonsAlignment
        {
            get => (ButtonsAlignment) GetValue(ButtonsAlignmentProperty);
            set => SetValue(ButtonsAlignmentProperty, value);
        }

        public double ButtonWidth
        {
            get => (double) GetValue(ButtonWidthProperty);
            set => SetValue(ButtonWidthProperty, value);
        }

        public ControlTemplate ButtonTemplate
        {
            get => (ControlTemplate) GetValue(ButtonTemplateProperty);
            set => SetValue(ButtonTemplateProperty, value);
        }

        public double ButtonFontSize
        {
            get => (double) GetValue(ButtonFontSizeProperty);
            set => SetValue(ButtonFontSizeProperty, value);
        }

        public FontFamily ButtonFontFamily
        {
            get => (FontFamily) GetValue(ButtonFontFamilyProperty);
            set => SetValue(ButtonFontFamilyProperty, value);
        }

        public DataTemplate ButtonContentTemplate
        {
            get => (DataTemplate) GetValue(ButtonContentTemplateProperty);
            set => SetValue(ButtonContentTemplateProperty, value);
        }

        public object ButtonContent
        {
            get => GetValue(ButtonContentProperty);
            set => SetValue(ButtonContentProperty, value);
        }

        public object ButtonCommandParameter
        {
            get => GetValue(ButtonCommandParameterProperty);
            set => SetValue(ButtonCommandParameterProperty, value);
        }

        public ICommand ButtonCommand
        {
            get => (ICommand) GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }

        #endregion button

        #endregion PROPERTIES

        #region CONSTRUCTOR

        static EditableTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableTextBlock),
                new FrameworkPropertyMetadata(typeof(EditableTextBlock)));
        }

        public EditableTextBlock()
        {
            MouseDown += (sender, args) =>
            {
                IsInEditingMode = true;
            };
            LostFocus += (sender, args) =>
            {
                IsInEditingMode = false;
            };
        }

        #endregion CONSTRUCTOR

        #region METHODS

        private static Brush GetDefaultSelectionBrush()
        {
            var brush = new SolidColorBrush(SystemColors.HighlightColor);
            brush.Freeze();
            return brush;
        }

        #endregion METHODS
    }
}