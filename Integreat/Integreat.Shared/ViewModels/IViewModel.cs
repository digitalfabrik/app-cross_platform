using System.ComponentModel;

namespace Integreat.Shared.Factories
{
    public interface IViewModel : INotifyPropertyChanged, INavigationAware
    {
        string Title { get; set; }
    }
}
