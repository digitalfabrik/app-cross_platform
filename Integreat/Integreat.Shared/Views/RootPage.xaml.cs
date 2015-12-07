using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Integreat.ApplicationObject;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistance;
using Xamarin.Forms;
using Page = Integreat.Models.Page;
using XamarinPage = Xamarin.Forms.Page;

namespace Integreat.Shared.Views
{
    public partial class RootPage
    {
        public PageLoader PageLoader;
        public List<Page> Pages;

        public RootPage()
        {
            InitializeComponent();
            MainMenu.ListView.ItemSelected += OnItemSelected;
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
            Console.WriteLine("Load called");
            Pages = await PageLoader.Load();
            Console.WriteLine("Pages received:" + Pages.Count);
            MainMenu.ListView.ItemsSource = Pages
                .Where(x => x.ParentId <= 0)
                .OrderBy(x => x.Order)
                .Select(x => new MainMenu.NavigationItem
                {
                    Title = x.Title,
                    Id = x.Id
                });
            OverviewPage.SetPages(Pages);
        }


        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenu.NavigationItem;
            if (item == null)
            {
                return;
            }
            var pages = Pages.Where(x => x.Id == item.Id || x.ParentId == item.Id);
            OverviewPage.SetPages(pages);
            MainMenu.ListView.SelectedItem = null;
            IsPresented = false;
        }
    }
}
