using System.Collections.Generic;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IObjectEditorViewModel
    {
        object Value { get; }

        IHeaderViewModel HeaderViewModel { get; }

        IPropertyViewModel Title { get; }
        IPropertyViewModel Subtitle { get; }
        IPropertyViewModel Description { get; }
        IPropertyViewModel Picture { get; }

        IEnumerable<IPropertyViewModel> NativeProperties { get; }
        IEnumerable<IPropertyViewModel> OtherProperties { get; }
    }
}
