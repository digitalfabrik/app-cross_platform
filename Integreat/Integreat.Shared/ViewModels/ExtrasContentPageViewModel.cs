using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Integreat.Localization;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace Integreat.Shared.ViewModels
{
    /// <summary>
    /// Class ExtrasContentPageViewModel holds all information and functionality about Extras views
    /// </summary>
    public class ExtrasContentPageViewModel : BaseContentViewModel
    {
        private ObservableCollection<ExtraAppEntry> _extras = new ObservableCollection<ExtraAppEntry>();
        private readonly INavigator _navigator;
        private string _plzHwk;
        private string _noteInternetText;
        private BaseContentViewModel _activeViewModel;
        private readonly Func<string, GeneralWebViewPageViewModel> _generalWebViewFactory;
        private ICommand _itemTappedCommand;
        private readonly Func<SprungbrettViewModel> _sprungbrettFactory;

        public ExtrasContentPageViewModel(INavigator navigator, DataLoaderProvider dataLoaderProvider
            , Func<SprungbrettViewModel> sprungbrettFactory
            , Func<string, GeneralWebViewPageViewModel> generalWebViewFactory)
            : base(dataLoaderProvider)
        {
            NoteInternetText = AppResources.NoteInternet;
            Title = AppResources.Extras;
            Icon = Device.RuntimePlatform == Device.Android ? null : "extras100";
            _navigator = navigator;
            _generalWebViewFactory = generalWebViewFactory;
            _sprungbrettFactory = sprungbrettFactory;
            ItemTappedCommand = new Command(InvokeOnTap);

            Extras = new ObservableCollection<ExtraAppEntry>();
        }

        public ObservableCollection<ExtraAppEntry> Extras
        {
            get => _extras;
            private set => SetProperty(ref _extras, value);
        }

        public ICommand ItemTappedCommand
        {
            get => _itemTappedCommand;
            set => SetProperty(ref _itemTappedCommand, value);
        }

        public string NoteInternetText
        {
            get => _noteInternetText;
            set => SetProperty(ref _noteInternetText, value);
        }

        private void InvokeOnTap(object obj)
        {
            var extraAppEntry = obj as ExtraAppEntry;
            extraAppEntry?.OnTapCommand?.Execute(obj);
        }

        private async void OnSerloTapped(object obj)
        {
            // push a new general webView page, which will show the URL of the offer

            var view = _generalWebViewFactory("https://abc-app.serlo.org/ ");
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
                $"<html><body onload='document.lehrstellenradar.submit()'><form name='lehrstellenradar' action='https://www.lehrstellen-radar.de/5100,0,lsrlist.html' method='post'><input type='text' hidden='hidden' name='partner' value='{partner}'><input type='text' hidden='hidden' name='radius' value='{radius}' /><input type='text' hidden='hidden' name='plz' value='{_plzHwk}'/><input type='submit' hidden='hidden'></form></body></html>");

            view.Title = "Lehrstellenradar";

            await _navigator.PushAsync(view, Navigation);
        }

        private async void OnIhkLerstellenboerseTapped(object obj)
        {
            var view = _generalWebViewFactory(LastLoadedLocation.IhkApprenticeshipsUrl);
            view.Title = "IHK Lehrstellenboerse";

            await _navigator.PushAsync(view, Navigation);
        }

        private async void OnIhkInternshipsTapped(object obj)
        {
            var view = _generalWebViewFactory(LastLoadedLocation.IhkInternshipsUrl);
            view.Title = "IHK Praktikumsbörse";

            await _navigator.PushAsync(view, Navigation);
        }

        private async void OnExtraTap(object obj)
        {
            if (!(obj is ExtraAppEntry asExtraAppEntry)) return;

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
                        Title = AppResources.Internships,
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
                        Title = AppResources.Apprenticeships,
                        ViewModelFactory = null,
                        OnTapCommand = new Command(OnLehrstellenTapped)
                    });
                }               

                if (forLocation.SerloEnabled.IsTrue())
                    Extras.Add(new ExtraAppEntry
                    {
                        Thumbnail = "serloabc.jpg",
                        Title = AppResources.Alphabetization,
                        ViewModelFactory = null,
                        OnTapCommand = new Command(OnSerloTapped)
                    });
                if (forLocation.IhkApprenticeshipsEnabled.IsTrue())
                {
                    Extras.Add(new ExtraAppEntry
                    {
                        Thumbnail = "ihk_lehrstellenboerse.png",
                        Title = AppResources.Apprenticeships,
                        ViewModelFactory = null,
                        OnTapCommand = new Command(OnIhkLerstellenboerseTapped)
                    });
                }
                if (forLocation.IhkInternshipsEnabled.IsTrue())
                {
                    Extras.Add(new ExtraAppEntry
                    {
                        Thumbnail = "ihk_lehrstellenboerse.png",
                        Title = AppResources.Internships,
                        ViewModelFactory = null,
                        OnTapCommand = new Command(OnIhkInternshipsTapped)
                    });
                }
            }

            // sort Extras after complete insertion
            Extras = new ObservableCollection<ExtraAppEntry>(Extras.OrderBy(e=>e.Title));

            _activeViewModel?.RefreshCommand.Execute(forced);
            IsBusy = false;
        }
    }
}
