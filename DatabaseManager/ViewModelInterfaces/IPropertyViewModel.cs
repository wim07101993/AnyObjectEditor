using System;
using System.ComponentModel;
using System.Reflection;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IPropertyViewModel : INotifyPropertyChanged
    {
        PropertyInfo PropertyInfo { get; }

        string DisplayName { get; }
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
