using System;
using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Portable.Collections.Interfaces;
using DatabaseManager.Helpers;
using MahApps.Metro.Controls;

namespace DatabaseManager.Views.Controls
{
    public class NativeTypeEditor : UserControl
    {
        #region FIELDS

        private TextBox _textBox = new TextBox
        {
            Visibility = Visibility.Collapsed
        };

        private NumericUpDown _numericUpDown = new NumericUpDown
        {
            Visibility = Visibility.Collapsed
        };

        private ToggleSwitch _toggleSwitch = new ToggleSwitch
        {
            Visibility = Visibility.Collapsed,
            OffLabel = "",
            OnLabel = ""
        };

        #endregion FIELDS

        #region DEPENDENCY PROPERTY

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(object),
                typeof(NativeTypeEditor),
                new PropertyMetadata(default(object)));

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(
                nameof(Type),
                typeof(Type),
                typeof(NativeTypeEditor),
                new PropertyMetadata(default(Type), OnTypeChanged));

        #endregion DEPENDENCY PROPERTY

        #region PROPERTIES

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public Type Type
        {
            get => (Type) GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        public NativeTypeEditor()
        {
            var grid = new Grid();

            grid.Children.Add(_textBox);
            grid.Children.Add(_numericUpDown);
            grid.Children.Add(_toggleSwitch);

            _textBox.TextChanged += OnTextChanged;
            _numericUpDown.ValueChanged += OnNumChanged;
            _toggleSwitch.IsCheckedChanged += OnToggle;

            Content = grid;
        }
        
        #endregion CONSTRUCTORS

        #region METHODS
        
        private static void OnTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var This = (NativeTypeEditor) obj;
            This._textBox.Visibility = Visibility.Collapsed;
            This._numericUpDown.Visibility = Visibility.Collapsed;
            This._toggleSwitch.Visibility = Visibility.Collapsed;

            if ((Type) args.NewValue == typeof(bool))
            {
                This._toggleSwitch.Visibility = Visibility.Visible;
                This._toggleSwitch.IsChecked = (bool?) This.Value;
            }
            else if ((Type) args.NewValue == typeof(byte) ||
                     (Type) args.NewValue == typeof(sbyte) ||
                     (Type) args.NewValue == typeof(short) ||
                     (Type) args.NewValue == typeof(ushort) ||
                     (Type) args.NewValue == typeof(int) ||
                     (Type) args.NewValue == typeof(uint) ||
                     (Type) args.NewValue == typeof(long) ||
                     (Type) args.NewValue == typeof(ulong) ||
                     (Type) args.NewValue == typeof(decimal) ||
                     (Type) args.NewValue == typeof(double) ||
                     (Type) args.NewValue == typeof(float))
            {
                This._numericUpDown.Visibility = Visibility.Visible;
                This._numericUpDown.Value = Convert.ToDouble(This.Value);

                if ((Type) args.NewValue == typeof(byte) ||
                    (Type) args.NewValue == typeof(ushort) ||
                    (Type) args.NewValue == typeof(uint) ||
                    (Type) args.NewValue == typeof(ulong))
                    This._numericUpDown.Minimum = 0;
            }
            else
            {
                This._textBox.Visibility = Visibility.Visible;
                This._textBox.Text = This.Value.ToString();

                if ((Type) args.NewValue == typeof(char))
                    This._textBox.MaxLength = 1;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            var oldValue = Value;
            Value = _textBox.Text;
            ValueChanged?.Invoke(this, new ValueChangedEventArgs {OldValue = oldValue, NewValue = Value});
        }

        private void OnNumChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            var oldValue = Value;
            Value = _numericUpDown.Value;
            ValueChanged?.Invoke(this, new ValueChangedEventArgs { OldValue = oldValue, NewValue = Value });
        }

        private void OnToggle(object sender, EventArgs e)
        {
            var oldValue = Value;
            Value = _toggleSwitch.IsChecked;
            ValueChanged?.Invoke(this, new ValueChangedEventArgs { OldValue = oldValue, NewValue = Value });
        }

        #endregion METHODS

        #region EVENTS

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion EVENTS
    }
}