using System.Threading.Tasks;
using Integreat.Shared.ViewFactory;
using Xamarin.Forms;

namespace Integreat.Shared.Services.Navigation
{
    /// <summary>
    /// Interface for navigation service
    /// </summary>
    public interface INavigationService
    {
        Task<IViewModel> PopModalAsync();

        Task PopToRootAsync();

        Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        Task PushAsync(Page page);

        void HideToolbar<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;

        void RemovePage(Page page);

        System.Collections.Generic.IReadOnlyList<Page> GetNavigationStack();
    }
}
