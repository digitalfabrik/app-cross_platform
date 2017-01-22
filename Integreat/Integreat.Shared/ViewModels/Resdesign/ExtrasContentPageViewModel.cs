using System.Threading.Tasks;
using System.Collections.Generic;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;


namespace Integreat.Shared.ViewModels.Resdesign {
    public class ExtrasContentPageViewModel : BaseViewModel {
        private INavigator _navigator;

        private Location _lastLoadedLocation; // the last loaded location
        private Language _lastLoadedLanguage; // the last loaded language
        private PersistenceService _persistenceService; // persistence service for online or offline loading of data

        public ExtrasContentPageViewModel(IAnalyticsService analytics, INavigator navigator,PersistenceService persistanceService)
        : base(analytics) {
            Title = "Extras";
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _persistenceService = persistanceService;
            LoadSettings();
        }

        protected override async void OnRefresh()
        {
            // wait until we're not busy anymore
            await Task.Run(() => {
                while (IsBusy) ;
            });
            LoadOffers();
            await Task.Run(() => {
                while (IsBusy) ;
            });
        }

        /// <summary>
        /// Loads the location and language from the settings and finally loads their models from the persistence service.
        /// </summary>
        private async void LoadSettings()
        {
            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            IsBusy = true;
            _lastLoadedLanguage = await _persistenceService.Get<Language>(languageId);
            _lastLoadedLocation = await _persistenceService.Get<Location>(locationId);
            IsBusy = false;
        }

        private async void LoadOffers()
        {
            string url;
            switch (_lastLoadedLocation.Name.ToLower())
            {
                case "stadt regensburg":
                    url = "http://www.careers4refugees.de/jobsearch/exports/integreat_regensburg";
                    break;
                case "bad tölz":
                    url = "http://www.careers4refugees.de/jobsearch/exports/integreat_bad-toelz";
                    break;
                    /*
                        url = "http://www.careers4refugees.de/jobsearch/exports/integreat_"+_lastLoadedLocation.Name.ToLower();
                        Dormagen http://www.careers4refugees.de/jobsearch/exports/integreat_dormagen
                        Ahaus http://www.careers4refugees.de/jobsearch/exports/integreat_ahaus
                        Main-Taunus-Kreis http://www.careers4refugees.de/jobsearch/exports/integreat_main-taunus-kreis
                        Regensburg http://www.careers4refugees.de/jobsearch/exports/integreat_regensburg
                        Kissing http://www.careers4refugees.de/jobsearch/exports/integreat_kissing
                        Bad Tölz http://www.careers4refugees.de/jobsearch/exports/integreat_bad-toelz
                        Augsburg http://www.careers4refugees.de/jobsearch/exports/integreat_augsburg
                    */
                default:
                    url = "http://www.careers4refugees.de/jobsearch/exports/integreat_" + _lastLoadedLocation.Name.ToLower();
                    break;
            }

            try
            {
                Offers = await XmlWebParser.ParseXmlFromAddressAsync<List<Careers4RefugeesTemp.CareerOffer>>(url, "anzeigen");
            }
            catch { }
        }
        public List<Careers4RefugeesTemp.CareerOffer> Offers
        {
            get
            {
                return _offers;
            }
            private set
            {
                SetProperty(ref _offers, value);
            }
        }
        private List<Careers4RefugeesTemp.CareerOffer> _offers;
    }
}
