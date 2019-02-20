using Xamarin.Forms;

namespace App1.Dialogs
{
    public interface IPage : IDialogProvider
    {
        INavigation Navigation { get; }
    }
}
