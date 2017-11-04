using System.Collections.Generic;
using System.ComponentModel;
using DatabaseManager.Attributes;
using Prism.Mvvm;

namespace DatabaseManager.Models
{
    public class SomeModel : BindableBase
    {
        private string _title = "This is the title";
        private string _subtitle = "This is the subtitle";
        private string _str = "Boe";
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
            "hello", "world", "to", "you"
        };

        [Title]
        [Browsable(false)]
        [DisplayName("First name")]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        [Subtitle]
        public string Subtitle
        {
            get => _subtitle;
            set => SetProperty(ref _subtitle, value);
        }

        public int Int
        {
            get => _i;
            set => SetProperty(ref _i, value);
        }

        [Attributes.Description]
        public string Str
        {
            get => _str;
            set => SetProperty(ref _str, value);
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