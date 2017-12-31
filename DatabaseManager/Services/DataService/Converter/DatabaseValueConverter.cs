using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DatabaseManager.Helpers.Extensions;

namespace DatabaseManager.Services.DataService.Converter
{
    public static class DatabaseValueConverter
    {
        private const byte ByteArrayByte = 0;
        private const byte ColorByte = 1;
        private const byte ImageByte = 2;
        private const byte VideoByte = 3;
        private const byte MusicByte = 4;


        public static byte[] ConvertBytes(byte[] bytes)
        {
            var byteList = bytes.ToList();
            byteList.Insert(0, ByteArrayByte);
            return byteList.ToArray();
        }

        public static byte[] ConvertToBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 4)
                return null;

            if (!bytes[0].Equals(ByteArrayByte))
                throw new DatabaseConverterException();

            var bytesList = bytes.ToList();
            bytesList.RemoveAt(0);
            return bytesList.ToArray();
        }


        public static byte[] ConvertColor(Color color)
            => new List<byte> {ColorByte, color.A, color.R, color.G, color.B}.ToArray();

        public static Color ConvertToColor(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 4)
                return Colors.Black;

            if (!bytes[0].Equals(ColorByte))
                throw new DatabaseConverterException();

            return bytes.Length == 4
                ? new Color {R = bytes[1], G = bytes[2], B = bytes[3]}
                : new Color {A = bytes[1], R = bytes[2], G = bytes[3], B = bytes[4]};
        }

        public static byte[] ConvertColor(SolidColorBrush color)
            => new List<byte> {color.Color.A, color.Color.R, color.Color.G, color.Color.B}.ToArray();

        public static SolidColorBrush ConvertToSolidColorBrush(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 4)
                return new SolidColorBrush(Colors.Black);

            if (!bytes[0].Equals(ColorByte))
                throw new DatabaseConverterException();

            return bytes.Length == 4
                ? new SolidColorBrush(new Color {R = bytes[1], G = bytes[2], B = bytes[3]})
                : new SolidColorBrush(new Color {A = bytes[1], R = bytes[1], G = bytes[3], B = bytes[4]});
        }


        public static byte[] ConvertImage(BitmapImage image)
        {
            var byteList = image.ToBytes().ToList();
            byteList.Insert(0, ImageByte);
            return byteList.ToArray();
        }

        public static BitmapImage ConvertToImage(byte[] bytes)
        {
            if (!bytes[0].Equals(ImageByte))
                throw new DatabaseConverterException();

            var bytesList = bytes.ToList();
            bytesList.RemoveAt(0);
            return bytesList.ToArray().ToBitmapImage();
        }
    }
}