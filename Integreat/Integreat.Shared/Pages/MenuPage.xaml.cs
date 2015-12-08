using Xamarin.Forms;
using System;
using System.Collections.Generic;
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
        RootPage root;
        List<HomeMenuItem> menuItems;
        public PageLoader PageLoader;

        public MenuPage(RootPage root)
        {
            this.root = root;
            InitializeComponent();
            BindingContext = new BaseViewModel
            {
                Title = "Integreat",
                Subtitle = "your local guide.",
                Icon = null
            };

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (ListViewMenu.SelectedItem == null)
                {
                    return;
                }

                await this.root.NavigateAsync(((HomeMenuItem) e.SelectedItem).PageId);
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
            ListViewMenu.ItemsSource = menuItems = pages
                .Where(x => x.ParentId <= 0)
                .OrderBy(x => x.Order)
                .Select(x =>
                    new HomeMenuItem
                    {
                        Title = x.Title,
                        PageId = x.PrimaryKey,
                        Icon = null
                    }).ToList();

            ListViewMenu.SelectedItem = null;
        }
    }
}
