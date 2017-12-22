using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using DatabaseManager.ViewModelInterfaces;
using Microsoft.Win32;

namespace DatabaseManager.Views.Controls
{
    public partial class EditableHeader
    {
        public EditableHeader()
        {
            InitializeComponent();
        }

        private void PictureButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Open Image",
                Filter = "Afbeeldingsbestanden (*.bmp;*.jpg;*.jpeg,*.png)|*.bmp;*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() != true)
                return;

            var propertyInfo = DataContext
                .GetType()
                .GetProperties()
                .SingleOrDefault(x =>
                    x.Name == "Picture" && typeof(IPropertyViewModel).IsAssignableFrom(x.PropertyType));

            if (propertyInfo != null && propertyInfo.GetValue(DataContext) is IPropertyViewModel propertyViewModel)
                propertyViewModel.Value = new BitmapImage(new Uri(dialog.FileName));
        }
    }
}