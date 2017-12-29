using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DatabaseManager.Helpers;

namespace DatabaseManager.Views.Controls.ColorPicker
{
    [TemplatePart(Name = ElementColorShadingCanvas, Type = typeof(Canvas))]
    [TemplatePart(Name = ElementColorShadeSelector, Type = typeof(Canvas))]
    [TemplatePart(Name = ElementSpectrumSlider, Type = typeof(ColorSpectrumSlider))]
    [TemplatePart(Name = ElementHexadecimalTextBox, Type = typeof(TextBox))]
    public class ColorPicker : Control
    {
        #region FIELDS

        private const string ElementColorShadingCanvas = "PART_ColorShadingCanvas";
        private const string ElementColorShadeSelector = "PART_ColorShadeSelector";
        private const string ElementSpectrumSlider = "PART_SpectrumSlider";
        private const string ElementHexadecimalTextBox = "PART_HexadecimalTextBox";

        private readonly TranslateTransform _colorShadeSelectorTransform = new TranslateTransform();
        private Canvas _colorShadingCanvas;
        private Canvas _colorShadeSelector;
        private ColorSpectrumSlider _spectrumSlider;
        private TextBox _hexadecimalTextBox;
        private Point? _currentColorPosition;
        private bool _surpressPropertyChanged;
        private bool _updateSpectrumSliderValue = true;

        #endregion FIELDS


        #region REGISTRATIONS

        public static readonly RoutedEvent SelectedColorChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(SelectedColorChanged), RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<Color?>), typeof(ColorPicker));


        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Color?), typeof(ColorPicker),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedColorChanged));

        public static readonly DependencyProperty AProperty = DependencyProperty.Register(nameof(A), typeof(byte),
            typeof(ColorPicker), new UIPropertyMetadata((byte) 255, OnARGBChanged));

        public static readonly DependencyProperty RProperty = DependencyProperty.Register(nameof(R), typeof(byte),
            typeof(ColorPicker), new UIPropertyMetadata((byte) 0, OnARGBChanged));

        public static readonly DependencyProperty GProperty = DependencyProperty.Register(nameof(G), typeof(byte),
            typeof(ColorPicker), new UIPropertyMetadata((byte) 0, OnARGBChanged));

        public static readonly DependencyProperty BProperty = DependencyProperty.Register(nameof(B), typeof(byte),
            typeof(ColorPicker), new UIPropertyMetadata((byte) 0, OnARGBChanged));

        public static readonly DependencyProperty HexadecimalStringProperty =
            DependencyProperty.Register(nameof(HexadecimalString), typeof(string), typeof(ColorPicker),
                new UIPropertyMetadata("", OnHexadecimalStringChanged, OnCoerceHexadecimalString));

        public static readonly DependencyProperty UsingAlphaChannelProperty = DependencyProperty.Register(
            nameof(UsingAlphaChannel), typeof(bool), typeof(ColorPicker),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnUsingAlphaChannelPropertyChanged));

        #endregion REGISTRATIONS


        #region PROPERTIES

        public Color? SelectedColor
        {
            get => (Color?) GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public byte A
        {
            get => (byte) GetValue(AProperty);
            set => SetValue(AProperty, value);
        }

        public byte R
        {
            get => (byte) GetValue(RProperty);
            set => SetValue(RProperty, value);
        }

        public byte G
        {
            get => (byte) GetValue(GProperty);
            set => SetValue(GProperty, value);
        }

        public byte B
        {
            get => (byte) GetValue(BProperty);
            set => SetValue(BProperty, value);
        }

        public string HexadecimalString
        {
            get => (string) GetValue(HexadecimalStringProperty);
            set => SetValue(HexadecimalStringProperty, value);
        }

        public bool UsingAlphaChannel
        {
            get => (bool) GetValue(UsingAlphaChannelProperty);
            set => SetValue(UsingAlphaChannelProperty, value);
        }

        #endregion PROPERTIES


        #region Constructors

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker),
                new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        #endregion Constructors


        #region METHODS

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_colorShadingCanvas != null)
            {
                _colorShadingCanvas.MouseLeftButtonDown -= ColorShadingCanvas_MouseLeftButtonDown;
                _colorShadingCanvas.MouseLeftButtonUp -= ColorShadingCanvas_MouseLeftButtonUp;
                _colorShadingCanvas.MouseMove -= ColorShadingCanvas_MouseMove;
                _colorShadingCanvas.SizeChanged -= ColorShadingCanvas_SizeChanged;
            }

            _colorShadingCanvas = GetTemplateChild(ElementColorShadingCanvas) as Canvas;

            if (_colorShadingCanvas != null)
            {
                _colorShadingCanvas.MouseLeftButtonDown += ColorShadingCanvas_MouseLeftButtonDown;
                _colorShadingCanvas.MouseLeftButtonUp += ColorShadingCanvas_MouseLeftButtonUp;
                _colorShadingCanvas.MouseMove += ColorShadingCanvas_MouseMove;
                _colorShadingCanvas.SizeChanged += ColorShadingCanvas_SizeChanged;
            }

            _colorShadeSelector = GetTemplateChild(ElementColorShadeSelector) as Canvas;

            if (_colorShadeSelector != null)
                _colorShadeSelector.RenderTransform = _colorShadeSelectorTransform;

            if (_spectrumSlider != null)
                _spectrumSlider.ValueChanged -= SpectrumSlider_ValueChanged;

            _spectrumSlider = GetTemplateChild(ElementSpectrumSlider) as ColorSpectrumSlider;

            if (_spectrumSlider != null)
                _spectrumSlider.ValueChanged += SpectrumSlider_ValueChanged;

            if (_hexadecimalTextBox != null)
                _hexadecimalTextBox.LostFocus -= HexadecimalTextBox_LostFocus;

            _hexadecimalTextBox = GetTemplateChild(ElementHexadecimalTextBox) as TextBox;

            if (_hexadecimalTextBox != null)
                _hexadecimalTextBox.LostFocus += HexadecimalTextBox_LostFocus;

            UpdateRGBValues(SelectedColor);
            UpdateColorShadeSelectorPosition(SelectedColor);

            // When changing theme, HexadecimalString needs to be set since it is not binded.
            SetHexadecimalTextBoxTextProperty(GetFormatedColorString(SelectedColor));
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);

        //    //hitting enter on textbox will update Hexadecimal string
        //    if (e.Key != Key.Enter || !(e.OriginalSource is TextBox))
        //        return;

        //    var textBox = (TextBox)e.OriginalSource;
        //    if (textBox.Name == ElementHexadecimalTextBox)
        //        SetHexadecimalStringProperty(textBox.Text, true);
        //}

        private void UpdateSelectedColor() => SelectedColor = Color.FromArgb(A, R, G, B);

        private void UpdateSelectedColor(Color? color)
            => SelectedColor = color != null
                ? (Color?) Color.FromArgb(color.Value.A, color.Value.R, color.Value.G, color.Value.B)
                : null;

        private void UpdateRGBValues(Color? color)
        {
            if (color == null)
                return;

            _surpressPropertyChanged = true;

            A = color.Value.A;
            R = color.Value.R;
            G = color.Value.G;
            B = color.Value.B;

            _surpressPropertyChanged = false;
        }

        private void UpdateColorShadeSelectorPositionAndCalculateColor(Point p, bool calculateColor)
        {
            if (_colorShadingCanvas == null || _colorShadeSelector == null)
                return;

            if (p.Y < 0)
                p.Y = 0;

            if (p.X < 0)
                p.X = 0;

            if (p.X > _colorShadingCanvas.ActualWidth)
                p.X = _colorShadingCanvas.ActualWidth;

            if (p.Y > _colorShadingCanvas.ActualHeight)
                p.Y = _colorShadingCanvas.ActualHeight;

            _colorShadeSelectorTransform.X = p.X - (_colorShadeSelector.Width / 2);
            _colorShadeSelectorTransform.Y = p.Y - (_colorShadeSelector.Height / 2);

            p.X = p.X / _colorShadingCanvas.ActualWidth;
            p.Y = p.Y / _colorShadingCanvas.ActualHeight;

            _currentColorPosition = p;

            if (calculateColor)
                CalculateColor(p);
        }

        private void UpdateColorShadeSelectorPosition(Color? color)
        {
            if (_spectrumSlider == null || _colorShadingCanvas == null || color == null)
                return;

            _currentColorPosition = null;

            var hsv = ColorUtilities.ConvertRgbToHsv(color.Value.R, color.Value.G, color.Value.B);

            if (_updateSpectrumSliderValue)
                _spectrumSlider.Value = hsv.H;

            var p = new Point(hsv.S, 1 - hsv.V);

            _currentColorPosition = p;

            _colorShadeSelectorTransform.X = (p.X * _colorShadingCanvas.Width) - 5;
            _colorShadeSelectorTransform.Y = (p.Y * _colorShadingCanvas.Height) - 5;
        }

        private void CalculateColor(Point p)
        {
            if (_spectrumSlider == null)
                return;

            var hsv = new HsvColor(360 - _spectrumSlider.Value, 1, 1)
            {
                S = p.X,
                V = 1 - p.Y
            };
            var currentColor = ColorUtilities.ConvertHsvToRgb(hsv.H, hsv.S, hsv.V);
            currentColor.A = A;

            _updateSpectrumSliderValue = false;
            SelectedColor = currentColor;
            _updateSpectrumSliderValue = true;

            SetHexadecimalStringProperty(GetFormatedColorString(SelectedColor), false);
        }

        private string GetFormatedColorString(Color? colorToFormat)
            => colorToFormat != null
                ? ColorUtilities.FormatColorString(colorToFormat.ToString(), UsingAlphaChannel)
                : string.Empty;

        private string GetFormatedColorString(string stringToFormat)
            => ColorUtilities.FormatColorString(stringToFormat, UsingAlphaChannel);

        private void SetHexadecimalStringProperty(string newValue, bool modifyFromUI)
        {
            if (modifyFromUI)
                try
                {
                    if (!string.IsNullOrEmpty(newValue))
                        ColorConverter.ConvertFromString(newValue);

                    HexadecimalString = newValue;
                }
                catch
                {
                    //When HexadecimalString is changed via UI and hexadecimal format is bad, keep the previous HexadecimalString.
                    SetHexadecimalTextBoxTextProperty(HexadecimalString);
                }
            else
                //When HexadecimalString is changed via Code-Behind, hexadecimal format will be evaluated in OnCoerceHexadecimalString()
                HexadecimalString = newValue;
        }

        private void SetHexadecimalTextBoxTextProperty(string newValue)
        {
            if (_hexadecimalTextBox != null)
                _hexadecimalTextBox.Text = newValue;
        }

        protected virtual void OnSelectedColorChanged(Color? oldValue, Color? newValue)
        {
            SetHexadecimalStringProperty(GetFormatedColorString(newValue), false);
            UpdateRGBValues(newValue);
            UpdateColorShadeSelectorPosition(newValue);

            var args = new RoutedPropertyChangedEventArgs<Color?>(oldValue, newValue)
            {
                RoutedEvent = SelectedColorChangedEvent
            };
            RaiseEvent(args);
        }

        protected virtual void OnHexadecimalStringChanged(string oldValue, string newValue)
        {
            var newColorString = GetFormatedColorString(newValue);
            var currentColorString = GetFormatedColorString(SelectedColor);
            if (!currentColorString.Equals(newColorString))
            {
                Color? col = null;
                if (!string.IsNullOrEmpty(newColorString))
                    col = ColorConverter.ConvertFromString(newColorString) as Color?;
                UpdateSelectedColor(col);
            }

            SetHexadecimalTextBoxTextProperty(newValue);
        }

        private void ColorShadingCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_currentColorPosition == null)
                return;

            var newPoint = new Point
            {
                X = ((Point) _currentColorPosition).X * e.NewSize.Width,
                Y = ((Point) _currentColorPosition).Y * e.NewSize.Height
            };

            UpdateColorShadeSelectorPositionAndCalculateColor(newPoint, false);
        }

        private void SpectrumSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_currentColorPosition != null && SelectedColor != null)
                CalculateColor((Point) _currentColorPosition);
        }

        private void HexadecimalTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textbox)
                SetHexadecimalStringProperty(textbox.Text, true);
        }

        #region mouse events

        private void ColorShadingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_colorShadingCanvas == null)
                return;

            var p = e.GetPosition(_colorShadingCanvas);
            UpdateColorShadeSelectorPositionAndCalculateColor(p, true);
            _colorShadingCanvas.CaptureMouse();
            //Prevent from closing ColorPicker after mouseDown in ListView
            e.Handled = true;
        }

        private void ColorShadingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => _colorShadingCanvas?.ReleaseMouseCapture();

        private void ColorShadingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_colorShadingCanvas == null || e.LeftButton != MouseButtonState.Pressed)
                return;

            var p = e.GetPosition(_colorShadingCanvas);
            UpdateColorShadeSelectorPositionAndCalculateColor(p, true);
            Mouse.Synchronize();
        }

        #endregion mouse events

        #region dependency property changes

        private static void OnSelectedColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is ColorPicker colorCanvas)
                colorCanvas.OnSelectedColorChanged((Color?)e.OldValue, (Color?)e.NewValue);
        }

        private static void OnARGBChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is ColorPicker colorCanvas && !colorCanvas._surpressPropertyChanged)
                colorCanvas.UpdateSelectedColor();
        }

        private static void OnHexadecimalStringChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is ColorPicker colorCanvas)
                colorCanvas.OnHexadecimalStringChanged((string)e.OldValue, (string)e.NewValue);
        }

        private static object OnCoerceHexadecimalString(DependencyObject d, object basevalue)
        {
            var colorCanvas = (ColorPicker)d;
            if (colorCanvas == null)
                return basevalue;

            var value = basevalue as string;
            var retValue = value;

            try
            {
                if (!string.IsNullOrEmpty(retValue))
                    ColorConverter.ConvertFromString(value);
            }
            catch
            {
                //When HexadecimalString is changed via Code-Behind and hexadecimal format is bad, throw.
                throw new InvalidDataException("Color provided is not in the correct format.");
            }

            return retValue;
        }

        private static void OnUsingAlphaChannelPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is ColorPicker colorCanvas)
                colorCanvas.SetHexadecimalStringProperty(
                    colorCanvas.GetFormatedColorString(colorCanvas.SelectedColor), false);
        }

        #endregion dependency property changes

        #endregion METHODS


        #region Events

        public event RoutedPropertyChangedEventHandler<Color?> SelectedColorChanged
        {
            add => AddHandler(SelectedColorChangedEvent, value);
            remove => RemoveHandler(SelectedColorChangedEvent, value);
        }

        #endregion Events
    }
}