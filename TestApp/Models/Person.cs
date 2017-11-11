using System.Collections.Generic;
using System.ComponentModel;
using DatabaseManager.Attributes;
using DatabaseManager.Models;
using Prism.Mvvm;

namespace TestApp.Models
{
    public class Person : BindableBase
    {
        private string _firstName = "Jan";
        private string _lastName = "Janssens";
        private string _summary = "Dit is een verzonnen persoon voor testdoeleinden";
        private double _d = 9.3098;
        private int _i = 5;
        private bool _b = true;
        private char _c = 'C';

        private List<SomeNestedModel> _someList = new List<SomeNestedModel>
        {
            new SomeNestedModel(),
            new SomeNestedModel
            {
                Name = "aproui",
                age = 62,
            }
        };

        private List<string> _someSecondList = new List<string>
        {
            "hello",
            "world",
            "to",
            "you"
        };

        [Title]
        [DisplayName("First name")]
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        [Subtitle]
        [DisplayName("Last name")]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        [Browsable(false)]
        public int Int
        {
            get => _i;
            set => SetProperty(ref _i, value);
        }

        [DatabaseManager.Attributes.Description]
        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }

        public double Double
        {
            get => _d;
            set => SetProperty(ref _d, value);
        }

        public bool Bool
        {
            get => _b;
            set => SetProperty(ref _b, value);
        }

        public char Char
        {
            get => _c;
            set => SetProperty(ref _c, value);
        }

        public List<SomeNestedModel> SomeList
        {
            get => _someList;
            set => SetProperty(ref _someList, value);
        }

        public List<string> SomeSecondList
        {
            get => _someSecondList;
            set => SetProperty(ref _someSecondList, value);
        }
    }
}