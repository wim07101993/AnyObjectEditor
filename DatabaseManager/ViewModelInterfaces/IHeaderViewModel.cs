using System.ComponentModel;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IHeaderViewModel : INotifyPropertyChanged
    {
        IPropertyViewModel Title { get; set; }
        IPropertyViewModel Subtitle { get; set; }
        IPropertyViewModel Description { get; set; }
        IPropertyViewModel Picture { get; set; }
    }
}
