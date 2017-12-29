using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DatabaseManager.Helpers;

namespace DatabaseManager.Views.Controls
{
    [TemplatePart(Name = ElementEditButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementClearButton, Type = typeof(Button))]
    public class ImagePicker : Control
    {
        #region FIELDS

        private const string ElementEditButton = "EditButton";
        private const string ElementClearButton = "ClearButton";

        private Button _editButton;
        private Button _clearButton;

        #endregion FIELDS

        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image), typeof(BitmapImage), typeof(ImagePicker), new PropertyMetadata(default(BitmapImage)));

        public static readonly DependencyProperty ImageCommandProperty = DependencyProperty.Register(
            nameof(ImageCommand), typeof(ICommand), typeof(ImagePicker), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register(
            nameof(EditCommand), typeof(ICommand), typeof(ImagePicker), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty ClearCommandProperty = DependencyProperty.Register(
            nameof(ClearCommand), typeof(ICommand), typeof(ImagePicker), new PropertyMetadata(default(ICommand)));

        #endregion DEPENDENCY PROPERTIES


        #region PROPERTIES

        public BitmapImage Image
        {
            get => (BitmapImage) GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public ICommand ImageCommand
        {
            get => (ICommand) GetValue(ImageCommandProperty);
            set => SetValue(ImageCommandProperty, value);
        }

        public ICommand EditCommand
        {
            get => (ICommand) GetValue(EditCommandProperty);
            set => SetValue(EditCommandProperty, value);
        }

        public ICommand ClearCommand
        {
            get => (ICommand) GetValue(ClearCommandProperty);
            set => SetValue(ClearCommandProperty, value);
        }

        #endregion PROPERTIES


        #region CONSTRUCTOR

        static ImagePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImagePicker),
                new FrameworkPropertyMetadata(typeof(ImagePicker)));
        }

        #endregion CONSTRUCTOR


        #region METHODS

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _editButton = GetTemplateChild(ElementEditButton) as Button;
            _clearButton = GetTemplateChild(ElementClearButton) as Button;

            if (_editButton == null)
                throw new InvalidOperationException(
                    $"You have missed to specify {ElementEditButton} in your template.");
            if (_clearButton == null)
                throw new InvalidOperationException(
                    $"You have missed to specify {ElementClearButton} in your template.");

            _editButton.Click += (sender, e) =>
                SetValue(ImageProperty, DialogHelper.OpenImageDialogAndConvertToBitmapImage());
            _clearButton.Click += (sender, e) => SetValue(ImageProperty, null);
        }

        #endregion METHODS
    }
}