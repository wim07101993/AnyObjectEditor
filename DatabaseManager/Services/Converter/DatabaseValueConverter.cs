using System.Linq;
using System.Windows.Media.Imaging;
using DatabaseManager.Helpers.Extensions;

namespace DatabaseManager.Services.Converter
{
    public static class DatabaseValueConverter
    {
        private const byte ByteArrayByte = 0;
        private const byte ImageByte = 1;
        private const byte VideoByte = 2;
        private const byte MusicByte = 3;


        public static byte[] ConvertBytes(byte[] bytes)
        {
            var byteList = bytes.ToList();
            byteList.Insert(0, ByteArrayByte);
            return byteList.ToArray();
        }

        public static byte[] ConvertToBytes(byte[] bytes)
        {
            if( !bytes[0].Equals(ByteArrayByte))
                throw new DatabaseConverterException();

            var bytesList = bytes.ToList();
            bytesList.RemoveAt(0);
            return bytesList.ToArray();
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
