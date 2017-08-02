using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Pages;
using Integreat.Shared.ViewFactory;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integreat.Shared.Services
{
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

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel, INavigation onNavigation) where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            if (Navigation.NavigationStack.Last().GetType() != view.GetType())
            {
                await onNavigation.PushAsync(view);
            }
            viewModel.NavigatedTo();
            return viewModel;
        }
    }
}
