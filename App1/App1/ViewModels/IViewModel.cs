
using App1.Navigator;

namespace App1.ViewModels
{
    public interface IViewModel : INavigationAware
    {
        string Title { get; set; }
    }
}
