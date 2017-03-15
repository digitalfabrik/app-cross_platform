using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using Integreat.Shared.ViewModels.Resdesign;

namespace Integreat.Shared
{
	public class SprungbrettViewModel : BaseContentViewModel
	{
        #region Fields
        private INavigator _navigator;

        private List<Careers4RefugeesTemp.CareerOffer> _offers;

        #endregion

        #region Properties
        public List<Careers4RefugeesTemp.CareerOffer> Offers {
            get {
                return _offers;
            }
            private set {
                SetProperty(ref _offers, value);
            }
        }
        #endregion⁄


        public SprungbrettViewModel(IAnalyticsService analytics, INavigator navigator, PersistenceService persistanceService)
            : base(analytics, persistanceService) {
            Title = "Extras";
            _navigator = navigator;
            _navigator.HideToolbar(this);
        }

        public override async void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null)
        {
            return;
            // wait until this resource is free
            await Task.Run(() => {
                while (IsBusy) ;
            });
            IsBusy = false;

            if (forLocation == null) forLocation = LastLoadedLocation;
            if (forLanguage == null) forLanguage = LastLoadedLanguage;

            IsBusy = false; 
        }
    }
}
