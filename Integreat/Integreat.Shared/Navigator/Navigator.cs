using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Pages;
using Integreat.Shared.ViewFactory;
using System;
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
        

        public async Task<IViewModel> PopAsync()
        {
            var view = await Navigation.PopAsync();
            var viewModel = view.BindingContext as IViewModel;
            viewModel?.NavigatedFrom();
            return viewModel;
        }

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

        public async Task<TViewModel> PushAsyncToTopWithNavigation<TViewModel>(TViewModel viewModel = null)
    where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
           // Application.Current.MainPage = new MyNavigationPage(view); //Hacky workaround but PopToRootAsync wont work!
            //Navigation.InsertPageBefore(view, Navigation.NavigationStack[0]);
            //await Navigation.PopToRootAsync(false);
            viewModel.NavigatedTo();
            var tcs = new TaskCompletionSource<TViewModel>();
            tcs.SetResult(viewModel);
            return await tcs.Task;
        }

        public async Task<TViewModel> PushAsyncToTop<TViewModel>(TViewModel viewModel = null)
            where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            Application.Current.MainPage = view; //Hacky workaround but PopToRootAsync wont work!
            //Navigation.InsertPageBefore(view, Navigation.NavigationStack[0]);
            //await Navigation.PopToRootAsync(false);
            viewModel.NavigatedTo();
            var tcs = new TaskCompletionSource<TViewModel>();
            tcs.SetResult(viewModel);
            return await tcs.Task;
        }

        public async Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = _viewFactory.Resolve(out viewModel, setStateAction);
            await Navigation.PushAsync(view);
            viewModel.NavigatedTo();
            return viewModel;
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            await Navigation.PushAsync(view);
            viewModel.NavigatedTo();
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = _viewFactory.Resolve(out viewModel, setStateAction);
            await Navigation.PushModalAsync(view);
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            await Navigation.PushModalAsync(view);
            return viewModel;
        }

        public void HideToolbar<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            NavigationPage.SetHasNavigationBar(view, false);
        }

        public void ShowToolbar<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            NavigationPage.SetHasNavigationBar(view, true);
        }

        public void SetHasBackButton<TViewModel>(TViewModel viewModel, bool v) where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            NavigationPage.SetHasBackButton(view, v);
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel, INavigation onNavigation) where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            await onNavigation.PushAsync(view);
            viewModel.NavigatedTo();
            return viewModel;
        }
    }
}
