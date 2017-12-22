using System.Windows.Media.Imaging;
using DatabaseManager.Helpers.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DatabaseManager.Services.Mongo
{
    public class BitmapImageSerializer : SerializerBase<BitmapImage>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
            BitmapImage value)
        {
            if (value == null)
                context.Writer.WriteNull();
            else
                context.Writer.WriteBytes(value.ToBytes());
        }

        public override BitmapImage Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.Binary)
            {
                var bytes = context.Reader.ReadBytes();
                var img = bytes.ToBitmapImage();
                return img;
            }

            context.Reader.SkipValue();
            return null;
        }
    }
}
