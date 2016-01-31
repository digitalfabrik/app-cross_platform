using System;
using System.Collections.ObjectModel;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
	public class DisclaimerViewModel : BaseViewModel
    {
        public ObservableCollection<PageViewModel> Pages {get; set;}
	    private readonly DisclaimerLoader _loader;
	    private readonly Func<Models.Page, PageViewModel> _pageFactory;

        public DisclaimerViewModel(DisclaimerLoader loader, Func<Models.Page, PageViewModel> pageFactory, Func<Language, Location, DisclaimerLoader> disclaimerLoaderFactory)
        {
            Title = "Information";
            Pages = new ObservableCollection<PageViewModel>();
            _loader = loader;
            _pageFactory = pageFactory;

            var locationId = Preferences.Location();// new Location { Path = "/wordpress/augsburg/" };
                                                    //				var location = await persistence.Get<Location> (locationId);
                                                    //				var languageId = Preferences.Language (location); // new Language { ShortName = "de" };
                                                    //				var language = await persistence.Get<Language> (languageId);
            var language = new Language(0, "de", "Deutsch", "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//augsburg//wp-content//plugins//sitepress-multilingual-cms//res//flags//de.png");
            var location = new Location(0, "Augsburg",
                               "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//wp-content//uploads//sites//2//2015//10//cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg",
                               "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/",
                               "Es schwäbelt", "yellow", "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/11/cropped-Augsburg.jpg",
                               0, 0, false);
            _loader = disclaimerLoaderFactory(language, location);

            Refresh();
        }
        private Command _loadPagesCommand;
        public Command LoadPagesCommand => _loadPagesCommand ??  (_loadPagesCommand = new Command(Refresh));

        private async void Refresh()
        {
            var pages = await _loader.Load();
            Pages.Clear();
            Pages.AddRange(pages.Select(x => _pageFactory(x)));
        }
    }
}
