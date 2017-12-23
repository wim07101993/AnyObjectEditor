using System.ComponentModel;
using System.Windows.Media.Imaging;
using DatabaseManager.Helpers.Attributes;
using DatabaseManager.Models.Bases;
using DatabaseManager.Services.Mongo;
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
        private BitmapImage _picture;
        private string _summary;
        private bool _male;

        [Id]
        [BsonId]
        [Browsable(false)]
        public ObjectId ObjectId { get; set; }

        [Title]
        [DisplayName("Voornaam")]
        [BsonElement("firstName")]
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        [Subtitle]
        [DisplayName("Achternaam")]
        [BsonElement("lastName")]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        [DisplayName("Leeftijd")]
        [BsonElement("age")]
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        [DisplayName("Man/Vrouw")]
        [BsonElement("male")]
        public bool Male
        {
            get => _male;
            set => SetProperty(ref _male, value);
        }

        [Picture]
        [DisplayName("Foto")]
        [BsonElement("picture")]
        [BsonSerializer(typeof(BitmapImageSerializer))]
        public BitmapImage Picture
        {
            get => _picture;
            set => SetProperty(ref _picture, value);
        }

        [Helpers.Attributes.Description]
        [DisplayName("Korte beschrijving")]
        [BsonElement("summary")]
        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }
    }
}