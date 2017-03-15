using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Xamarin.Forms;


namespace Integreat.Shared.ViewModels.Resdesign {
    public class ExtrasContentPageViewModel : BaseContentViewModel {
        private ObservableCollection<ExtraAppEntry> _extras;
        private INavigator _navigator;
        private BaseContentViewModel _activeViewModel;

        #region Fields

        #endregion

        #region Properties

        public ObservableCollection<ExtraAppEntry> Extras {
            get { return _extras; }
            private set { SetProperty(ref _extras, value); }
        }

        #endregion


        public ExtrasContentPageViewModel(IAnalyticsService analytics, INavigator navigator, PersistenceService persistanceService
            , Func<Careers4RefugeesViewModel> careers4RefugeesFactory
            , Func<SprungbrettViewModel> sprungbrettFactory)
            : base(analytics, persistanceService) {
            Title = "Extras";
            _navigator = navigator;

            Extras = new ObservableCollection<ExtraAppEntry>();
            Extras.Add(new ExtraAppEntry { Image = "careers4refugees_de_icon.jpg", ViewModelFactory = careers4RefugeesFactory, Name = "Careers 4 Refugees", OnTapCommand = new Command(OnExtraTap) });
            Extras.Add(new ExtraAppEntry { Image = "sbi_integreat_quadratisch_farbe.jpg", ViewModelFactory = sprungbrettFactory, Name = "Sprungbrett", OnTapCommand = new Command(OnExtraTap) });


        }

        private async void OnExtraTap(object obj) {
            var asExtraAppEntry = obj as ExtraAppEntry;
            if (asExtraAppEntry == null) return;

            // push page on stack
            var vm = asExtraAppEntry.ViewModelFactory() as BaseContentViewModel;
            _activeViewModel = vm;
            _activeViewModel?.LoadContent();
            await _navigator.PushAsync(vm, Navigation);
        }

        public override void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null) {
            _activeViewModel?.LoadContent(forced, forLanguage, forLocation);
        }
    }
}
