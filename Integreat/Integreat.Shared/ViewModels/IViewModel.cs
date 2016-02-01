using System.ComponentModel;

namespace Integreat.Shared.ViewFactory
{
    public interface IViewModel : INotifyPropertyChanged, INavigationAware
    {
        string Title { get; set; }
    }
}
