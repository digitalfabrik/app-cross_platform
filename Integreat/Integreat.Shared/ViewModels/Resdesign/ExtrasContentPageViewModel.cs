using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.ViewModels.Resdesign.General;
using Xamarin.Forms;
using localization;

namespace Integreat.Shared.ViewModels.Resdesign
{
    public class ExtrasContentPageViewModel : BaseContentViewModel
    {
        private ObservableCollection<ExtraAppEntry> _extras;
        private INavigator _navigator;
        private string _plzHwk;
        private string _noteInternetText;
        private BaseContentViewModel _activeViewModel;
        private Func<string, bool, GeneralWebViewPageViewModel> _generalWebViewFactory;
        private ICommand _itemTappedCommand;
        private Func<Careers4RefugeesViewModel> _careers4RefugeesFactory;
        private Func<SprungbrettViewModel> _sprungbrettFactory;


        #region Properties

        public ObservableCollection<ExtraAppEntry> Extras {
            get { return _extras; }
            private set { SetProperty(ref _extras, value); }
        }

        public ICommand ItemTappedCommand {
            get { return _itemTappedCommand; }
            set { SetProperty(ref _itemTappedCommand, value); }
        }

        #endregion


        public ExtrasContentPageViewModel(IAnalyticsService analytics, INavigator navigator, DataLoaderProvider dataLoaderProvider
            , Func<Careers4RefugeesViewModel> careers4RefugeesFactory
            , Func<SprungbrettViewModel> sprungbrettFactory
            , Func<string, bool, GeneralWebViewPageViewModel> generalWebViewFactory)
            : base(analytics, dataLoaderProvider)
        {
            NoteInternetText = AppResources.NoteInternet;
            Title = AppResources.Extras;
            Icon = Device.RuntimePlatform == Device.Android ? null : "extras100";
            _navigator = navigator;
            _generalWebViewFactory = generalWebViewFactory;
            _careers4RefugeesFactory = careers4RefugeesFactory;
            _sprungbrettFactory = sprungbrettFactory;
            ItemTappedCommand = new Command(InvokeOnTap);

            Extras = new ObservableCollection<ExtraAppEntry>();

        }

        public string NoteInternetText {
            get { return _noteInternetText; }
            set { SetProperty(ref _noteInternetText, value); }
        }

        private void InvokeOnTap(object obj)
        {
            var extraAppEntry = obj as ExtraAppEntry;
            extraAppEntry?.OnTapCommand?.Execute(obj);
        }

        private async void OnSerloTapped(object obj)
        {
            // push a new general webView page, which will show the URL of the offer

            var view = _generalWebViewFactory("https://abc.serlo.org/try/#1", false);
            view.Title = "SerloABC";
            await _navigator.PushAsync(view, Navigation);
        }

        // Needs to developed more detailed, when we have received the POST-Docu
        private async void OnLehrstellenTapped(object obj)
        {
            // push a new general webView page, which will show the URL of the offer

            const string partner = "0006";
            const string radius = "50"; // search radius

            var view = _generalWebViewFactory(
                $"<html><body onload='document.lehrstellenradar.submit()'><form name='lehrstellenradar' action='https://www.lehrstellen-radar.de/5100,0,lsrlist.html' method='post'><input type='text' hidden='hidden' name='partner' value='{partner}'><input type='text' hidden='hidden' name='radius' value='{radius}' /><input type='text' hidden='hidden' name='plz' value='{_plzHwk}'/><input type='submit' hidden='hidden'></form></body></html>", true);

            view.Title = "Lehrstellenradar";

            await _navigator.PushAsync(view, Navigation);
        }

        private async void OnExtraTap(object obj)
        {
            var asExtraAppEntry = obj as ExtraAppEntry;
            if (asExtraAppEntry == null) return;

            // push page on stack
            var vm = asExtraAppEntry.ViewModelFactory() as BaseContentViewModel;
            _activeViewModel = vm;
            if (vm == null) return;
            _activeViewModel.Title = vm.Title;
            _activeViewModel?.RefreshCommand.Execute(false);
            await _navigator.PushAsync(vm, Navigation);
        }

        protected override void LoadContent(bool forced = false, Language forLanguage = null,
            Location forLocation = null)
        {
            IsBusy = true;
            // add extras depending on the current selected location
            Extras.Clear();

            if (forLocation == null)
            {
                forLocation = LastLoadedLocation;
            }

            if (forLocation != null)
            {
                if (forLocation.SprungbrettEnabled.IsTrue())
                {
                    Extras.Add(new ExtraAppEntry
                    {
                        Thumbnail = "sbi_integreat_quadratisch_farbe.jpg",
                        Title = "Sprungbrett",
                        ViewModelFactory = _sprungbrettFactory,
                        OnTapCommand = new Command(OnExtraTap)
                    });
                }
                if (forLocation.LehrstellenRadarEnabled.IsTrue())
                {

                    _plzHwk = forLocation.Zip;
                    Extras.Add(new ExtraAppEntry
                    {
                        Thumbnail = "lsradar.jpg",
                        Title = "Lehrstellenradar",
                        ViewModelFactory = null,
                        OnTapCommand = new Command(OnLehrstellenTapped)
                    });
                }

                if (forLocation.Careers4RefugeesEnabled.IsTrue())
                    Extras.Add(new ExtraAppEntry
                    {
                        Thumbnail = "careers4refugees_de_icon.jpg",
                        Title = "Careers 4 Refugees",
                        ViewModelFactory = _careers4RefugeesFactory,
                        OnTapCommand = new Command(OnExtraTap)
                    });

                if (forLocation.SerloEnabled.IsTrue())
                    Extras.Add(new ExtraAppEntry
                    {
                        Thumbnail = "serloabc.jpg",
                        Title = "Serlo ABC",
                        ViewModelFactory = null,
                        OnTapCommand = new Command(OnSerloTapped)
                    });
            }

            _activeViewModel?.RefreshCommand.Execute(forced);
            IsBusy = false;
        }
    }
}
