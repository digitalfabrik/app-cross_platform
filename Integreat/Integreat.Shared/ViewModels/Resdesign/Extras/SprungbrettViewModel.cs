using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign;

namespace Integreat.Shared
{
	public class SprungbrettViewModel : BaseContentViewModel
	{
        #region Fields
        private INavigator _navigator;

        private List<SprungbrettTemp.JobOffer> _offers;
	    private bool _hasNoResults;


        #endregion

        #region Properties
        public List<SprungbrettTemp.JobOffer> Offers {
            get {
                return _offers;
            }
            private set {
                SetProperty(ref _offers, value);
            }
        }

	    /// <summary>
	    /// Gets or sets a value indicating whether this instance has no results for the given location or not.
	    /// </summary>
	    public bool HasNoResults
	    {
	        get { return _hasNoResults; }
	        set { SetProperty(ref _hasNoResults, value); }
	    }
        #endregion⁄


        public SprungbrettViewModel(IAnalyticsService analytics, INavigator navigator, DataLoaderProvider dataLoaderProvider)
            : base(analytics, dataLoaderProvider) {
            Title = "Sprungbrett";
            _navigator = navigator;
            _navigator.HideToolbar(this);
        }

        protected override async void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null)
        {
         
            Offers?.Clear();
            // wait until this resource is free
            await Task.Run(() => {
                while (IsBusy) ;
            });
            IsBusy = true;
            HasNoResults = true;

            if (forLocation == null) forLocation = LastLoadedLocation;
            if (forLanguage == null) forLanguage = LastLoadedLanguage;

            IsBusy = false; 
        }
    }
}
