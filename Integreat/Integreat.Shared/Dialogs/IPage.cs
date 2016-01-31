
using Integreat.Shared.Services;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
    public interface IPage : IDialogProvider
    {
        INavigation Navigation { get; }
    }
}
