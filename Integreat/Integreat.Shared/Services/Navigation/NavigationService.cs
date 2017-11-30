using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.ViewFactory;
using Xamarin.Forms;

namespace Integreat.Shared.Services.Navigation
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
            await GetCurrentPage().Navigation.PopToRootAsync();
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            if (GetCurrentPage().Navigation.NavigationStack.Last() != view)
            {
                await GetCurrentPage().Navigation.PushAsync(view);
            }
            return viewModel;
        }

        public async Task PushAsync(Page page)
        {
            if(GetCurrentPage().Navigation.NavigationStack.Last() !=page){
                await GetCurrentPage().Navigation.PushAsync(page);
            }
        }

        public void HideToolbar<TViewModel>(TViewModel viewModel) where TViewModel: class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            NavigationPage.SetHasNavigationBar(view, false);
        }

        public void RemovePage(Page page)
        {
            Navigation.RemovePage(page);
        }

        public System.Collections.Generic.IReadOnlyList<Page> GetNavigationStack()
        {
            return Navigation.NavigationStack;
        }

    }
}
