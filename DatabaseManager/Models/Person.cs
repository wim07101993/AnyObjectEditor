using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DatabaseManager.Helpers.Attributes;
using DatabaseManager.Models.Bases;
using DatabaseManager.Services.DataService.Mongo;
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
        private BitmapImage _favoriteImage;
        private Color _favoriteColor;

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

        [DisplayName("Favoriete afbeelding")]
        [BsonElement("favoriteImage")]
        [BsonSerializer(typeof(BitmapImageSerializer))]
        public BitmapImage FavoriteImage
        {
            get => _favoriteImage;
            set => SetProperty(ref _favoriteImage, value);
        }

        [DisplayName("Favoriete kleur")]
        [BsonElement("favoriteColor")]
        [BsonSerializer(typeof(ColorSerializer))]
        public Color FavoriteColor
        {
            get => _favoriteColor;
            set => SetProperty(ref _favoriteColor, value);
        }

        [DisplayName("Eeen random object")]
        public List<int> SomeList { get; set; }

        [DisplayName("Hond")]
        [BsonElement("dog")]
        [BsonSerializer(typeof(BitmapImageSerializer))]
        public BitmapImage Dog { get; set; }
    }
}