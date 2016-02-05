using Integreat.Shared.ViewFactory;
using System;
using System.Threading.Tasks;

namespace Integreat.Shared.Services
{
    public interface INavigator
    {
        Task<IViewModel> PopAsync();

        Task<IViewModel> PopModalAsync();

        Task PopToRootAsync();

        Task<TViewModel> PushAsyncToTopWithNavigation<TViewModel>(TViewModel setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushAsyncToTop<TViewModel>(TViewModel setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        void HideToolbar<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;
        void ShowToolbar<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;
    }
}
