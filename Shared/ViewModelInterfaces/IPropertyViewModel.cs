using System;
using System.ComponentModel;
using System.Reflection;

namespace Shared.ViewModelInterfaces
{
    public interface IPropertyViewModel : INotifyPropertyChanged
    {
        PropertyInfo PropertyInfo { get; }

        string DisplayName { get; }
        string Name { get; }

        object Value { get; set; }

        Type Type { get; }
        bool HasNativeType { get; }
        bool IsImage { get; }
        bool IsColor { get; }

        bool IsBrowsable { get; }
        bool IsTitle { get; }
        bool IsSubTitle { get; }
        bool IsDescription { get; }
        bool IsReadOnly { get; }
    }
}