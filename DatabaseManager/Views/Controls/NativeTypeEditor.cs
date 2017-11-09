using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DatabaseManager.EventArgs;

namespace DatabaseManager.Views.Controls
{
    public class NativeTypeEditor : Control
    {
        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(object),
            typeof(NativeTypeEditor),
            new PropertyMetadata(default(object), OnValueChanged));

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
            nameof(PropertyName),
            typeof(string),
            typeof(NativeTypeEditor),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            nameof(Type),
            typeof(ENativeType),
            typeof(NativeTypeEditor),
            new PropertyMetadata(ENativeType.String, OnTypeChanged));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(NativeTypeEditor),
            new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty MaxTextLengthProperty = DependencyProperty.Register(
            nameof(MaxTextLength),
            typeof(int),
            typeof(NativeTypeEditor),
            new PropertyMetadata(int.MaxValue));

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            nameof(MinValue),
            typeof(double),
            typeof(NativeTypeEditor),
            new PropertyMetadata(double.MinValue));

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            nameof(MaxValue),
            typeof(double),
            typeof(NativeTypeEditor),
            new PropertyMetadata(double.MaxValue));

        #endregion DEPENDENCY PROPERTIES

        #region PROPERTIES

        public string PropertyName
        {
            get => (string) GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public ENativeType Type
        {
            get => (ENativeType) GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public int MaxTextLength
        {
            get => (int) GetValue(MaxTextLengthProperty);
            set => SetValue(MaxTextLengthProperty, value);
        }

        public double MinValue
        {
            get => (double) GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        public double MaxValue
        {
            get => (double) GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        static NativeTypeEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NativeTypeEditor),
                new FrameworkPropertyMetadata(typeof(NativeTypeEditor)));
        }

        #endregion COSNTRUCTORS

        #region METHODS

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue != null && obj.GetValue(TypeProperty) == null)
                obj.SetValue(TypeProperty, args.NewValue?.GetType().Name);

            ((ICommand) obj.GetValue(CommandProperty))?.Execute(args.NewValue);

            var This = (NativeTypeEditor) obj;
            This.ValueChanged?.Invoke(This, new ValueChangedEventArgs(args.OldValue, args.NewValue));
        }

        private static void OnTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            switch ((ENativeType) args.NewValue)
            {
                case ENativeType.Bool:
                    break;
                case ENativeType.String:
                    obj.SetValue(MaxTextLengthProperty, int.MaxValue);
                    break;
                case ENativeType.Char:
                    obj.SetValue(MaxTextLengthProperty, 1);
                    break;
                case ENativeType.Byte:
                    obj.SetValue(MaxValueProperty, (double) byte.MaxValue);
                    obj.SetValue(MinValueProperty, (double) byte.MinValue);
                    break;
                case ENativeType.SByte:
                    obj.SetValue(MaxValueProperty, (double) sbyte.MaxValue);
                    obj.SetValue(MinValueProperty, (double) sbyte.MinValue);
                    break;
                case ENativeType.Short:
                    obj.SetValue(MaxValueProperty, (double) short.MaxValue);
                    obj.SetValue(MinValueProperty, (double) short.MinValue);
                    break;
                case ENativeType.UShort:
                    obj.SetValue(MaxValueProperty, (double) ushort.MaxValue);
                    obj.SetValue(MinValueProperty, (double) ushort.MinValue);
                    break;
                case ENativeType.Int:
                    obj.SetValue(MaxValueProperty, (double) int.MaxValue);
                    obj.SetValue(MinValueProperty, (double) int.MinValue);
                    break;
                case ENativeType.UInt:
                    obj.SetValue(MaxValueProperty, (double) uint.MaxValue);
                    obj.SetValue(MinValueProperty, (double) uint.MinValue);
                    break;
                case ENativeType.Long:
                    obj.SetValue(MaxValueProperty, (double) long.MaxValue);
                    obj.SetValue(MinValueProperty, (double) long.MinValue);
                    break;
                case ENativeType.ULong:
                    obj.SetValue(MaxValueProperty, (double) ulong.MaxValue);
                    obj.SetValue(MinValueProperty, (double) ulong.MinValue);
                    break;
                case ENativeType.Float:
                    obj.SetValue(MaxValueProperty, float.MaxValue);
                    obj.SetValue(MinValueProperty, float.MinValue);
                    break;
                case ENativeType.Decimal:
                    obj.SetValue(MaxValueProperty, decimal.MaxValue);
                    obj.SetValue(MinValueProperty, decimal.MinValue);
                    break;
                case ENativeType.Double:
                    obj.SetValue(MaxValueProperty, double.MaxValue);
                    obj.SetValue(MinValueProperty, double.MinValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion METHODS


        #region EVENTS

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion EVENTS
    }
}