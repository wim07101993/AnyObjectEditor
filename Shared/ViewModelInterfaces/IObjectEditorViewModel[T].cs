﻿using System.Collections.Generic;

namespace Shared.ViewModelInterfaces
{
    public interface IObjectEditorViewModel<out T>
    {
        T Value { get; }

        IHeaderViewModel HeaderViewModel { get; }

        IPropertyViewModel Title { get; }
        IPropertyViewModel Subtitle { get; }
        IPropertyViewModel Description { get; }
        IPropertyViewModel Picture { get; }

        IEnumerable<IPropertyViewModel> KnownTypeProperties { get; }
        IEnumerable<IPropertyViewModel> OtherProperties { get; }
    }
}