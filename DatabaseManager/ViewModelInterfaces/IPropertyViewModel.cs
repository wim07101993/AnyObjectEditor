using System;
using System.ComponentModel;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IPropertyViewModel : INotifyPropertyChanged
    {
        string Name { get; }
        object Value { get; set; }
        Type Type { get; }

        bool IsBrowsable { get; }
        bool IsTitle { get; }
        bool IsSubTitle { get; }
        bool IsDescription { get; }
        bool IsReadOnly { get; }
    }

}
