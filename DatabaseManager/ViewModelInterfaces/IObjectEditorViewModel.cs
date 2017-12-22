using System.Collections.Generic;
using System.Windows.Input;

namespace DatabaseManager.ViewModelInterfaces
{
    public interface IObjectEditorViewModel
    {
        IHeaderViewModel HeaderViewModel { get; }

        IPropertyViewModel Title { get; }
        IPropertyViewModel Subtitle { get; }
        IPropertyViewModel Description { get; }
        IPropertyViewModel Picture { get; }

        IEnumerable<IPropertyViewModel> NativeProperties { get; }
        
    }
}
