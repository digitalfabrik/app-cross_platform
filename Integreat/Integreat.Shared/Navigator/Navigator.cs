using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Pages;
using Integreat.Shared.ViewFactory;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integreat.Shared.Services
{
    /// <summary>
    /// ToDo shall we use this navigator in the baseviewmodel directly?
    /// </summary>
    public class Navigator : INavigator
    {
        private readonly IPage _page;
        private readonly IViewFactory _viewFactory;

        public Navigator(IPage page, IViewFactory viewFactory)
        {
            _page = page;
            _viewFactory = viewFactory;
        }

        private INavigation Navigation => _page.Navigation;

        public async Task<IViewModel> PopModalAsync()
        {
            var view = await Navigation.PopModalAsync();
            var viewModel = view?.BindingContext as IViewModel;
            viewModel?.NavigatedFrom();
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
            viewModel.NavigatedTo();
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

            viewModel.NavigatedTo();
            return viewModel;
        }
    }
}
