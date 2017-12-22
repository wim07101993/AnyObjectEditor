using System.ComponentModel;
using System.Windows.Media.Imaging;
using DatabaseManager.Helpers.Attributes;
using DatabaseManager.Models.Bases;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prism.Mvvm;

namespace DatabaseManager.Models
{
    public class Person : BindableBase, IMongoModel
    {
        private string _firstName;
        private string _lastName;
        private int _age;
        private BitmapSource _picture;
        private string _summary;

        [BsonId]
        [Id]
        [Browsable(false)]
        public ObjectId ObjectId { get; set; }

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
        [Helpers.Attributes.Description]
        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }


        public Person()
        {
            //ObjectId = ObjectId.GenerateNewId();
        }
    }
}