using System;
using System.ComponentModel;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IPropertyVM : INotifyPropertyChanged
    {
        string Name { get; }
        object Value { get; set; }
        Type Type { get; }

        bool IsEnumerable { get; }
        bool IsNativeType { get; }
    }
}