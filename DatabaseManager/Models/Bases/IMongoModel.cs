using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseManager.Models.Bases
{
    public interface IMongoModel
    {
        [BsonId]
        ObjectId ObjectId { get; }
    }
}
