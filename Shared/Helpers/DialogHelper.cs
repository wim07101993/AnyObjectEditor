using System;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Shared.Helpers
{
    public static class DialogHelper
    {
        public static string OpenImageDialog()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Open Image",
                Filter = "Afbeeldingsbestanden (*.bmp;*.jpg;*.jpeg,*.png)|*.bmp;*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() != true || string.IsNullOrWhiteSpace(dialog.FileName))
                return null;

            return dialog.FileName;
        }

        public static BitmapImage OpenImageDialogAndConvertToBitmapImage()
        {
            var path = OpenImageDialog();
            return string.IsNullOrWhiteSpace(path)
                ? null
                : new BitmapImage(new Uri(path));
        }
    }
}