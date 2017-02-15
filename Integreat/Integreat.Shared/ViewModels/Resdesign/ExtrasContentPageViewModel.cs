using System.Threading.Tasks;
using System.Collections.Generic;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;


namespace Integreat.Shared.ViewModels.Resdesign {
	public class ExtrasContentPageViewModel : BaseContentViewModel {
		#region Fields
		private INavigator _navigator;

		private List<Careers4RefugeesTemp.CareerOffer> _offers;

		#endregion

		#region Properties
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
		#endregion⁄


		public ExtrasContentPageViewModel(IAnalyticsService analytics, INavigator navigator,PersistenceService persistanceService)
			: base(analytics, persistanceService) {
            Title = "Extras";
            _navigator = navigator;
            _navigator.HideToolbar(this);
        }

		protected override async void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null)
		{

			if (forLocation == null) forLocation = LastLoadedLocation;
			if (forLanguage == null) forLanguage = LastLoadedLanguage;

            string url;
			switch (forLocation.Name.ToLower())
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
					url = "http://www.careers4refugees.de/jobsearch/exports/integreat_" + forLocation.Name.ToLower();
                    break;
            }

            try
            {
                Offers = await XmlWebParser.ParseXmlFromAddressAsync<List<Careers4RefugeesTemp.CareerOffer>>(url, "anzeigen");
            }
            catch { }
        }
    }
}
