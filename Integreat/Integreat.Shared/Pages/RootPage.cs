using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.ViewModels;
using System.Threading.Tasks;
using Autofac;
using Integreat.ApplicationObject;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Controls;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Persistance;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
    public class RootPage : MasterDetailPage
    {
        public PageLoader PageLoader;
        Dictionary<int, NavigationPage> Pages { get; set; }
        public MenuPage _menuPage;
        public MyNavigationPage _navigationPage;
        public OverviewPage _overviewPage;
        public List<Integreat.Models.Page> _pages; 

        public RootPage()
        {
            BindingContext = new BaseViewModel
            {
                Title = "Integreat",
                Icon = null
            };
            Pages = new Dictionary<int, NavigationPage>();
            Master = _menuPage = new MenuPage(this);
            _overviewPage = new OverviewPage(LoadPagesCommand);
            Detail = _navigationPage = new MyNavigationPage(_overviewPage);
            InitPageLoader();
        }


        public void InitPageLoader()
        {
            using (AppContainer.Container.BeginLifetimeScope())
            {
                var network = AppContainer.Container.Resolve<INetworkService>();
                var persistence = AppContainer.Container.Resolve<PersistenceService>();
                //TODO remove hardcoded data
                var language = new Language { ShortName = "de" };
                var location = new Location { Path = "/wordpress/augsburg/" };
                PageLoader = new PageLoader(language, location, persistence, network);
            }
        }

        public async Task LoadPages()
        {
            Console.WriteLine("LoadPages called");
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            LoadPagesCommand.ChangeCanExecute();
            try
            {
                _pages = await PageLoader.Load(); var orderedPages = _pages
                .Where(x => x.ParentId <= 0)
                .OrderBy(x => x.Order)
                .Select(x =>
                    new HomeMenuItem
                    {
                        Title = x.Title,
                        PageId = x.PrimaryKey,
                        ImageSource = new UriImageSource
                        {
                            Uri = x.Thumbnail != null ? new Uri(x.Thumbnail) : null,
                            CachingEnabled = true,
                            CacheValidity = new TimeSpan(1, 0, 0, 0)
                        },
                    }).ToList();
                _menuPage.ViewModel.Pages.Clear();
                _menuPage.ViewModel.Pages.AddRange(orderedPages);
                _overviewPage.PagesLoaded(_pages);
            }
            catch (Exception)
            {
                var page = new ContentPage();
                await page.DisplayAlert("Error", "Unable to load pages.", "OK");
            }
            Console.WriteLine("Pages received:" + _pages.Count);
            IsBusy = false;
            LoadPagesCommand.ChangeCanExecute();
            Console.WriteLine("LoadPages stopped");
        }

        public async Task NavigateAsync(int pageId)
        {
            _overviewPage.PageSelected(pageId);
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadPagesCommand.Execute(null);
        }


        private Command _loadPagesCommand;

        public Command LoadPagesCommand
        {
            get
            {
                return _loadPagesCommand ??
                       (_loadPagesCommand = new Command(async () =>
                       {
                           await LoadPages();
                       }, () => !IsBusy));
            }
        }
    }

}
