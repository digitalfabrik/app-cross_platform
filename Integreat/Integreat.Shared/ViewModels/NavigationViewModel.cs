using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        private DisclaimerPresenter DisclaimerPresenter;
        public ObservableCollection<PageViewModel> Pages { get; internal set; }
        private DisclaimerLoader _disclaimerLoader;
        private IDialogProvider _dialogProvider;

        public string Thumbnail => "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/11/cropped-Augsburg.jpg";

        public NavigationViewModel(Func<Language, Location, DisclaimerLoader> disclaimerLoaderFactory, IDialogProvider dialogProvider)
        {
            Console.WriteLine("NavigationViewModel initialized");
            Title = "Navigation";
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
            _disclaimerLoader = disclaimerLoaderFactory(language, location);
            _dialogProvider = dialogProvider;
            Pages = new ObservableCollection<PageViewModel>();
        }

        private Command _openDisclaimerCommand;

        public Command OpenDisclaimerCommand => _openDisclaimerCommand ??
                                                (_openDisclaimerCommand = new Command(OnOpenDisclaimerClicked));

        private async void OnOpenDisclaimerClicked()
        {
            var action = await _dialogProvider.DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");
        }

    }
}
