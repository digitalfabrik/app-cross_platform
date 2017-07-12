using System.Linq;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using localization;

namespace Integreat.Shared.ViewModels.Resdesign {
    public class ContactPageViewModel : BaseContentViewModel {
        #region Fields

        private readonly INavigator _navigator;
        private string _content;

        #endregion;

        #region Properties

        public string Content {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }
        #endregion

        public ContactPageViewModel(IAnalyticsService analytics, INavigator navigator, DataLoaderProvider dataLoaderProvider)
        : base(analytics, dataLoaderProvider) {
            Title = AppResources.Contact;
            _navigator = navigator;
            _navigator.HideToolbar(this);
        }

        protected override async void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null)
        {
            if (forLanguage == null) forLanguage = LastLoadedLanguage;
            if (forLocation == null) forLocation = LastLoadedLocation;

            if (forLocation == null || forLanguage == null || IsBusy) return;

            try {
                IsBusy = true;
                Content = "";
                var pages = await _dataLoaderProvider.DisclaimerDataLoader.Load(forced, forLanguage, forLocation);
                Content = string.Join("<br><br>", pages.Select(x => x.Content));
            } finally {
                IsBusy = false;
            }
        }
    }
}
