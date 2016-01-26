using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.ViewModels;
using System.Threading.Tasks;
using Autofac;
using Integreat.ApplicationObject;
using Integreat.Services;
using Integreat.Shared.Controls;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Persistance;
using Xamarin.Forms;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Pages
{
	public class RootPage : MasterDetailPage
	{
		private PageLoader PageLoader;
		private DisclaimerPresenter DisclaimerPresenter;

		Dictionary<int, NavigationPage> Pages { get; set; }

		public MenuPage _menuPage;
		public MyNavigationPage _navigationPage;
		public OverviewPage _overviewPage;
		public List<Integreat.Shared.Models.Page> _pages;

		public RootPage ()
		{
			BindingContext = new BaseViewModel {
				Title = "Integreat",
				Icon = null
			};
			Pages = new Dictionary<int, NavigationPage> ();
			Master = _menuPage = new MenuPage (this);
			_overviewPage = new OverviewPage (LoadPagesCommand);
			Detail = _navigationPage = new MyNavigationPage (_overviewPage);
			DisclaimerPresenter = new DisclaimerPresenter ();
			_menuPage.DisclaimerPresenter = DisclaimerPresenter;
			InitLoader ();
		}


		public async void InitLoader ()
		{
			using (AppContainer.Container.BeginLifetimeScope ()) {
				var network = AppContainer.Container.Resolve<INetworkService> ();
				var persistence = AppContainer.Container.Resolve<PersistenceService> ();
//				var locationId = Preferences.Location ();// new Location { Path = "/wordpress/augsburg/" };
//				var location = await persistence.Get<Location> (locationId);
//				var languageId = Preferences.Language (location); // new Language { ShortName = "de" };
//				var language = await persistence.Get<Language> (languageId);
				var language = new Language (0, "de", "Deutsch", "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//augsburg//wp-content//plugins//sitepress-multilingual-cms//res//flags//de.png");
				var location = new Location (0, "Augsburg", 
					               "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//wp-content//uploads//sites//2//2015//10//cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg", 
					               "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/", 
					               "Es schwäbelt", "yellow", "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/11/cropped-Augsburg.jpg", 
					               0, 0, false);
				PageLoader = new PageLoader (language, location, persistence, network);
				DisclaimerPresenter.DisclaimerLoader = new DisclaimerLoader (language, location, persistence, network);
			}
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			LoadPagesCommand.Execute (null);
			DisclaimerPresenter.LoadDisclaimerCommand.Execute (null);
		}

		public async Task LoadPages ()
		{
			Console.WriteLine ("LoadPages called");
			if (IsBusy) {
				return;
			}
			IsBusy = true;
			LoadPagesCommand.ChangeCanExecute ();
			try {
				_pages = await PageLoader.Load ();
				var preparedPages = PreparePages (_pages);
				_menuPage.ViewModel.Pages.Clear ();
				_menuPage.ViewModel.Pages.AddRange (preparedPages);
				_overviewPage.PagesLoaded (_pages);
			} catch (Exception e) {
				var page = new ContentPage ();
				await page.DisplayAlert ("Error", "Unable to load pages: " + e.Message, "OK");
			} finally {
				IsBusy = false;
				LoadPagesCommand.ChangeCanExecute ();
			}
			Console.WriteLine ("Pages received:" + _pages.Count);
			Console.WriteLine ("LoadPages stopped");
		}

		public async Task NavigateAsync (int pageId)
		{
			_overviewPage.PageSelected (pageId);
		}

		private List<HomeMenuItem> PreparePages (List<Integreat.Shared.Models.Page> pages)
		{
			return pages
				.Where (x => x.ParentId <= 0)
				.OrderBy (x => x.Order)
				.Select (x => new HomeMenuItem {
				Title = x.Title,
				PageId = x.PrimaryKey,
				ImageSource = new UriImageSource {
					Uri = String.IsNullOrEmpty (x.Thumbnail) ? null : new Uri (x.Thumbnail),
					CachingEnabled = true,
					CacheValidity = new TimeSpan (1, 0, 0, 0)
				},
			}).ToList ();
		}

		private Command _loadPagesCommand;

		public Command LoadPagesCommand {
			get {
				return _loadPagesCommand ??
				(_loadPagesCommand = new Command (async () => {
					await LoadPages ();
				}, () => !IsBusy));
			}
		}
	}
}
