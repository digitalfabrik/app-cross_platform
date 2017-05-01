using Integreat.Shared.ViewFactory;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

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


        /// <summary>
        /// Pushes the a new page instance resolved with the given ViewModel on the given Navigation.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model of the page to be pushed.</param>
        /// <param name="onNavigation">The navigation to be used.</param>
        Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel, INavigation onNavigation)
            where TViewModel : class, IViewModel;


        Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        void HideToolbar<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;
        void ShowToolbar<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;
        void SetHasBackButton<TViewModel>(TViewModel viewModel, bool v) where TViewModel : class, IViewModel;
    }
}
