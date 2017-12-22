using System.Windows.Input;
using System.Windows.Media.Imaging;
using DatabaseManager.ViewModelInterfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace DatabaseManager.ViewModel
{
    public class HeaderViewModel : BindableBase, IHeaderViewModel
    {
        private string _title;
        private string _subtitle;
        private string _description;
        private BitmapSource _picture;
        private string _id;

        public string ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Subtitle
        {
            get => _subtitle;
            set => SetProperty(ref _subtitle, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public BitmapSource Picture
        {
            get => _picture;
            set => SetProperty(ref _picture, value);
        }

        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }


        public HeaderViewModel()
        {
            DeleteCommand = new DelegateCommand(Delete);
            SaveCommand = new DelegateCommand(Save);
        }


        private void Delete()
        {
        }

        private void Save()
        {
        }
    }
}