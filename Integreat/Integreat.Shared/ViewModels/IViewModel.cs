using System.ComponentModel;

namespace Integreat.Shared.ViewFactory
{
    public interface IViewModel : INotifyPropertyChanged
    {
        string Title { get; set; }
    }
}
