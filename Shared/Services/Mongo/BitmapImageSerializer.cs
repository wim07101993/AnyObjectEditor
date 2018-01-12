using System.Windows.Media.Imaging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Shared.Services.Converter;

namespace Shared.Services.Mongo
{
    public class BitmapImageSerializer : SerializerBase<BitmapImage>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
            BitmapImage value)
        {
            if (value == null)
                context.Writer.WriteNull();
            else
                context.Writer.WriteBytes(DatabaseValueConverter.ConvertImage(value));
        }

        public override BitmapImage Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.Binary)
                return DatabaseValueConverter.ConvertToImage(context.Reader.ReadBytes());

            context.Reader.SkipValue();
            return null;
        }
    }
}