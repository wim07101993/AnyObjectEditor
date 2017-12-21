using System.Windows.Media.Imaging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseManager.Models
{
    public class Person
    {
        [BsonId]
        public ObjectId ID { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("picture")]
        public BitmapSource Picture { get; set; }

        [BsonElement("summary")]
        public string Summary { get; set; }
    }
}