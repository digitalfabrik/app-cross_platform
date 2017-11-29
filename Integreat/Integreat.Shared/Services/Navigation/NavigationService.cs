using System;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Services.Navigation;
using Integreat.Shared.ViewFactory;
using Xamarin.Forms;

namespace Integreat.Shared.Services
{
    /// <summary>
    /// Service can be used to navigate between pages
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly IViewFactory _viewFactory;
        private readonly Page _page;

        public NavigationService(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        private INavigation Navigation => _page.Navigation;

        private Page GetCurrentPage(){
            return _page != null ? _page : Application.Current.MainPage;
        }

        public async Task<IViewModel> PopModalAsync()
        {
            var view = await Navigation.PopModalAsync();
            var viewModel = view?.BindingContext as IViewModel;
            return viewModel;
        }

        public async Task PopToRootAsync()
        {
            await Navigation.PopToRootAsync();
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            if (Navigation.NavigationStack.Last() != view)
            {
                await Navigation.PushAsync(view);
            }
            return viewModel;
        }

        public void HideToolbar<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            NavigationPage.SetHasNavigationBar(view, false);
        }

        /// <inheritdoc />
        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel, INavigation onNavigation) where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);

            await onNavigation.PushAsync(view);

            return viewModel;
        }
    }
}
