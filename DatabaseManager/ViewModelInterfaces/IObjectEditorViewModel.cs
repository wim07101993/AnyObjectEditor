using System.Collections.Generic;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IObjectEditorViewModel
    {
        IPropertyViewModel Title { get; }
        IPropertyViewModel Subtitle { get; }
        IPropertyViewModel Description { get; }
        IPropertyViewModel Picture { get; }

        IHeaderViewModel HeaderViewModel { get; }

        IEnumerable<IPropertyViewModel> NativeProperties { get; }
    }
}
