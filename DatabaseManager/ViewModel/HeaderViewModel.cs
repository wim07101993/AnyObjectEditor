using System.Windows.Input;
using System.Windows.Media.Imaging;
using DatabaseManager.ViewModelInterfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace DatabaseManager.ViewModel
{
    public class HeaderViewModel : BindableBase, IHeaderViewModel
    {
        private IPropertyViewModel _title;
        private IPropertyViewModel _subtitle;
        private IPropertyViewModel _description;
        private IPropertyViewModel _picture;
        private string _id;

        public string ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public IPropertyViewModel Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public IPropertyViewModel Subtitle
        {
            get => _subtitle;
            set => SetProperty(ref _subtitle, value);
        }

        public IPropertyViewModel Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public IPropertyViewModel Picture
        {
            get => _picture;
            set => SetProperty(ref _picture, value);
        }
    }
}