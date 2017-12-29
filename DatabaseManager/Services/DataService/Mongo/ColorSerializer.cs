using System.Windows.Media;
using DatabaseManager.Services.DataService.Converter;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DatabaseManager.Services.DataService.Mongo
{
    public class ColorSerializer : SerializerBase<Color>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Color value)
        {
            context.Writer.WriteBytes(DatabaseValueConverter.ConvertColor(value));
        }

        public override Color Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.Binary)
                return DatabaseValueConverter.ConvertToColor(context.Reader.ReadBytes());

            context.Reader.SkipValue();
            return Colors.Transparent;
        }
    }
}
