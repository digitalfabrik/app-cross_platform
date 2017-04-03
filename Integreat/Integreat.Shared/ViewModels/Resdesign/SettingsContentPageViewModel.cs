using System;
using System.Linq;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Tracking;
using Localization;

namespace Integreat.Shared.ViewModels.Resdesign {
    public class SettingsContentPageViewModel : BaseContentViewModel {
        #region Fields

        private INavigator _navigator;
        private string _content;
        private Func<Language, Location, DisclaimerLoader> _disclaimerLoaderFactory;

        #endregion;

        #region Properties

        public string Content {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }
        #endregion


        public SettingsContentPageViewModel(IAnalyticsService analytics, INavigator navigator, DataLoaderProvider dataLoaderProvider, Func<Language, Location, DisclaimerLoader> disclaimerLoaderFactory)
        : base(analytics, dataLoaderProvider) {
            Title = AppResources.Settings;
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _disclaimerLoaderFactory = disclaimerLoaderFactory;
        }

        public override async void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null)
        {
            if (forLanguage == null) forLanguage = LastLoadedLanguage;
            if (forLocation == null) forLocation = LastLoadedLocation;

            if (forLocation == null || forLanguage == null || IsBusy) return;

            try {
                IsBusy = true;
                Content = "";
                var loader = _disclaimerLoaderFactory(forLanguage, forLocation);
                var pages = await loader.Load(forced);
                Content = string.Join("<br><br>", pages.Select(x => x.Content));
            } finally {
                IsBusy = false;
            }
        }
    }
}
