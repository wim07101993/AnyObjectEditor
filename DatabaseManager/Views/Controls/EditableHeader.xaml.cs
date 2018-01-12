using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Shared.Helpers;
using Shared.ViewModelInterfaces;

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
            var path = DialogHelper.OpenImageDialog();
            
            if (string.IsNullOrWhiteSpace(path))
                return;

            var propertyInfo = DataContext
                .GetType()
                .GetProperties()
                .SingleOrDefault(x =>
                    x.Name == "Picture" && typeof(IPropertyViewModel).IsAssignableFrom(x.PropertyType));

            if (propertyInfo != null && propertyInfo.GetValue(DataContext) is IPropertyViewModel propertyViewModel)
                propertyViewModel.Value = new BitmapImage(new Uri(path));
        }
    }
}