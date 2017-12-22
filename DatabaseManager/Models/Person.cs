using System.Windows.Media.Imaging;
using DatabaseManager.Helpers.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prism.Mvvm;

namespace DatabaseManager.Models
{
    public class Person : BindableBase
    {
        private ObjectId _id;
        private string _firstName;
        private string _lastName;
        private int _age;
        private BitmapSource _picture;
        private string _summary;

        [BsonId]
        public ObjectId ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        [BsonElement("firstName")]
        [Title]
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        [BsonElement("lastName")]
        [Subtitle]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        [BsonElement("age")]
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        [BsonElement("picture")]
        [Picture]
        public BitmapSource Picture
        {
            get => _picture;
            set => SetProperty(ref _picture, value);
        }

        [BsonElement("summary")]
        [Description]
        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }
    }
}