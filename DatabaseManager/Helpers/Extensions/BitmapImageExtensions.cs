using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DatabaseManager.Helpers.Extensions
{
    public static class BitmapImageExtensions
    {
        public static byte[] ToBytes(this BitmapImage This)
        {
            byte[] ret;
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(This));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                ret = ms.ToArray();
            }

            return ret;
        }

        public static BitmapImage ToBitmapImage(this byte[] This)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var image = new BitmapImage();

                using (var ms = new MemoryStream(This))
                {
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                }

                return image;
            });
        }
    }
}