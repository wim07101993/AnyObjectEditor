using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Shared.Helpers;
using Shared.Helpers.Extensions;

namespace Shared.Controls
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
            new PropertyMetadata(default(ENativeType)));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(NativeTypeEditor),
            new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty IsBoolProperty = DependencyProperty.Register(
            nameof(IsBool),
            typeof(bool),
            typeof(NativeTypeEditor),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsNumericProperty = DependencyProperty.Register(
            nameof(IsNumeric),
            typeof(bool),
            typeof(NativeTypeEditor),
            new PropertyMetadata(default(bool)));

        #endregion DEPENDENCY PROPERTIES

        #region PROPERTIES

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public ENativeType Type
        {
            get => (ENativeType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public bool IsBool => (bool)GetValue(IsBoolProperty);

        public bool IsNumeric => (bool)GetValue(IsNumericProperty);

        #endregion PROPERTIES

        #region CONSTRUCTORS

        static NativeTypeEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NativeTypeEditor),
                new FrameworkPropertyMetadata(typeof(NativeTypeEditor)));
        }

        public NativeTypeEditor()
        {

        }

        #endregion COSNTRUCTORS

        #region METHODS

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (!args.NewValue.GetType().IsNativeType())
                return;

            if (args.OldValue?.GetType() != args.NewValue?.GetType())
                obj.SetValue(TypeProperty, args.NewValue?.GetType().Name.ConvertToENativeType());

            ((ICommand)obj.GetValue(CommandProperty))?.Execute(args.NewValue);

            var This = (NativeTypeEditor)obj;
            This.ValueChanged?.Invoke(This, new ValueChangedEventArgs(args.OldValue, args.NewValue));

        }

        #endregion METHODS


        #region EVENTS

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion EVENTS
    }
}
