using System.Windows;
using System.Windows.Controls.Primitives;

namespace DatabaseManager.Views.Behaviours
{
    public static class TextBoxBehaviour
    {
        public static readonly DependencyProperty FocusProperty = DependencyProperty.RegisterAttached(
            "Focus",
            typeof(bool),
            typeof(TextBoxBehaviour),
            new UIPropertyMetadata(false, OnFocusChanged));

        public static bool GetFocus(TextBoxBase target)
            => (bool)target.GetValue(FocusProperty);

        public static void SetFocus(TextBoxBase target, bool value)
            => target.SetValue(FocusProperty, value);

        private static void OnFocusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
            => ((TextBoxBase)obj).Focus();
    }
}
