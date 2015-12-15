using Xamarin.Forms;
using System;
using Integreat.Shared.ViewModels;
using Integreat.Shared.Models;
using Integreat.ApplicationObject;
using Integreat.Services;
using Autofac;
using Integreat.Shared.Services.Persistance;
using Integreat.Models;
using Integreat.Shared.Services.Loader;
using System.Linq;

namespace Integreat.Shared.Pages
{
    public partial class MenuPage : ContentPage
    {
        readonly RootPage _root;
        public PageLoader PageLoader;
        public MenuPage(RootPage root)
        {
            _root = root;
            InitializeComponent();
            ListViewMenu.Header = BindingContext = new BaseViewModel
            {
                Title = "Augsburg",
                ImageSource = new UriImageSource
                {
                    Uri = new Uri("http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/11/cropped-Augsburg.jpg"),
                    CachingEnabled = true,
                    CacheValidity = new TimeSpan(1, 0, 0, 0)
                },
            };

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (ListViewMenu.SelectedItem == null)
                {
                    return;
                }

                await this._root.NavigateAsync(((HomeMenuItem) e.SelectedItem).PageId);
            };

            using (AppContainer.Container.BeginLifetimeScope())
            {
                var network = AppContainer.Container.Resolve<INetworkService>();
                var persistence = AppContainer.Container.Resolve<PersistenceService>();
                //TODO remove hardcoded data
                var language = new Language {ShortName = "de"};
                var location = new Location {Path = "/wordpress/augsburg/"};
                PageLoader = new PageLoader(language, location, persistence, network);
            }
            LoadPages();
        }

        private async void LoadPages()
        {
            var pages = await PageLoader.Load();
            Console.WriteLine("Pages received:" + pages.Count);
           
            ListViewMenu.ItemsSource = pages
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

            ListViewMenu.SelectedItem = null;
        }
    }
}
