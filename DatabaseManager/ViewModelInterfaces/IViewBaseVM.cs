using System.ComponentModel;
using ClassLibrary.Portable.Collections;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IViewBaseVM: INotifyPropertyChanged
    {
        IPropertyVM Title { get; }
        IPropertyVM Subtitle { get; }
        IPropertyVM Description { get; }

        ObservableCollection<IPropertyVM> NativeProperties { get; }
    }
}
