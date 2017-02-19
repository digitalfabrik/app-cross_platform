﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using Integreat.Shared.ViewModels.Resdesign;

namespace Integreat.Shared {
    public class Careers4RefugeesViewModel : BaseContentViewModel {
        #region Fields
        private INavigator _navigator;

        private ObservableCollection<Careers4RefugeesTemp.CareerOffer> _offers;
        private bool _hasNoResults;

        #endregion

        #region Properties
        public ObservableCollection<Careers4RefugeesTemp.CareerOffer> Offers {
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
        public bool HasNoResults {
            get { return _hasNoResults; }
            set { SetProperty(ref _hasNoResults, value); }
        }

        #endregion⁄


        public Careers4RefugeesViewModel(IAnalyticsService analytics, INavigator navigator, PersistenceService persistanceService)
            : base(analytics, persistanceService) {
            Title = "Extras";
            _navigator = navigator;
            _navigator.HideToolbar(this);
        }

        public override async void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null) {
            // wait until this resource is free
            Offers?.Clear();
            await Task.Run(() => {
                while (IsBusy) ;
            });
            IsBusy = true;
            HasNoResults = false;
            if (forLocation == null) forLocation = LastLoadedLocation;
            if (forLanguage == null) forLanguage = LastLoadedLanguage;

            string url;
            switch (forLocation.Name.ToLower()) {
                case "stadt regensburg":
                    url = "http://www.careers4refugees.de/jobsearch/exports/integreat_regensburg";
                    break;
                case "bad tölz":
                    url = "http://www.careers4refugees.de/jobsearch/exports/integreat_bad-toelz";
                    break;
                case "landkreis germersheim":
                    url = "http://www.careers4refugees.de/jobsearch/exports/integreat_gemersheim";
                    break;
                case "köln":
                    url = "http://www.careers4refugees.de/jobsearch/exports/koeln";
                    break;
                case "bochum":
                    url = "http://www.careers4refugees.de/jobsearch/exports/bochum";
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
                    http://www.careers4refugees.de/jobsearch/exports/integreat_gemersheim 
                    http://www.careers4refugees.de/jobsearch/exports/bochum 
                    http://www.careers4refugees.de/jobsearch/exports/koeln 
                */
                default:
                    url = "http://www.careers4refugees.de/jobsearch/exports/integreat_" + forLocation.Name.ToLower();
                    break;
            }

            try {
                Offers =
                    new ObservableCollection<Careers4RefugeesTemp.CareerOffer>(
                        await XmlWebParser.ParseXmlFromAddressAsync<List<Careers4RefugeesTemp.CareerOffer>>(url, "anzeigen"));
            } catch (Exception e) {
                HasNoResults = true;
            } finally { IsBusy = false; }
        }
    }
}
