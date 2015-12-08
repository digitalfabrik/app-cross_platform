using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Integreat.ApplicationObject;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistance;
using Xamarin.Forms;
using Page = Integreat.Models.Page;

namespace Integreat.Shared.ViewModels
{
    public class PagesViewModel : BaseViewModel
    {
        public ObservableCollection<Page> Pages { get; set; }

        private int _selectedPagePrimaryKey;
        public PageLoader PageLoader;

        public int SelectedPagePrimaryKey
        {
            get { return _selectedPagePrimaryKey; }
            set
            {
                _selectedPagePrimaryKey = value;
                ExecuteLoadPagesCommand();
            }
        }

        public PagesViewModel()
        {
            Title = "Information";
            Icon = null;
            Pages = new ObservableCollection<Page>();
            using (AppContainer.Container.BeginLifetimeScope())
            {
                var network = AppContainer.Container.Resolve<INetworkService>();
                var persistence = AppContainer.Container.Resolve<PersistenceService>();
                //TODO remove hardcoded data
                var language = new Language {ShortName = "de"};
                var location = new Location {Path = "/wordpress/augsburg/"};
                PageLoader = new PageLoader(language, location, persistence, network);
                PageLoader.Load();
            }
            SelectedPagePrimaryKey = -1;
        }


        private Command _loadPagesCommand;

        public Command LoadPagesCommand
        {
            get
            {
                return _loadPagesCommand ??
                       (_loadPagesCommand = new Command(async () =>
                       {
                           await ExecuteLoadPagesCommand();
                       }, () => !IsBusy));
            }
        }

        public async Task ExecuteLoadPagesCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            LoadPagesCommand.ChangeCanExecute();
            var error = false;
            try
            {
                Pages.Clear();
                var loadedPages = await LoadPages(SelectedPagePrimaryKey);
                foreach (var page in loadedPages)
                {
                    Pages.Add(page);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                error = true;
            }

            if (error)
            {
                var page = new ContentPage();
                await page.DisplayAlert("Error", "Unable to load pages.", "OK");
            }

            IsBusy = false;
            LoadPagesCommand.ChangeCanExecute();
        }

        private async Task<IEnumerable<Page>> LoadPages(int parentPageId)
        {
            var pages = await PageLoader.Load();
            Console.WriteLine("Pages received:" + pages.Count);
            var filteredPages = pages
                .Where(x => parentPageId == -1 || x.ParentId <= 0)
                .OrderBy(x => x.Order);
            return filteredPages;
        }
    }
}
