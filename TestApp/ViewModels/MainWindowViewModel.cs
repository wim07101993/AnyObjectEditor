using DatabaseManager;
using DatabaseManager.Extensions;
using Prism.Mvvm;
using TestApp.Models;

namespace TestApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Unity Application";
        private object _someThingStrange = false;
        private ENativeType _type;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public object SomeThingStrange
        {
            get => _someThingStrange;
            set => SetProperty(ref _someThingStrange, value);
        }

        public ENativeType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public Person Person { get; set; } = new Person();

        public MainWindowViewModel()
        {
            Type = SomeThingStrange.GetType().Name.ConvertToENativeType();
        }
    }
}