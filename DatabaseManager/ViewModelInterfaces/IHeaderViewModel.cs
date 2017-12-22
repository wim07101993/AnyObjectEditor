using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IHeaderViewModel : INotifyPropertyChanged
    {
        string Title { get; set; }
        string Subtitle { get; set; }
        string Description { get; set; }
        BitmapSource Picture { get; set; }

        ICommand DeleteCommand { get; }
        ICommand SaveCommand { get; }
    }
}
