using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Integreat.Shared.ViewFactory
{
    public interface IViewModel : INotifyPropertyChanged, INavigationAware
    {
        string Title { get; set; }
    }
}
