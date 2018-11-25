using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Integreat.Localization;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Models.Extras;
using Integreat.Shared.Services;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace Integreat.Shared.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// Class ExtrasContentPageViewModel holds all information and functionality about Extras views
    /// </summary>
    public class ExtrasContentPageViewModel : BaseContentViewModel
    {
        private ObservableCollection<Extra> _extras = new ObservableCollection<Extra>();
        private readonly INavigator _navigator;
        private string _noteInternetText;
        private readonly Func<string, GeneralWebViewPageViewModel> _generalWebViewFactory;
        private ICommand _itemTappedCommand;
        private ICommand _changeLanguageCommand;
        private readonly Func<string, SprungbrettViewModel> _sprungbrettFactory;
        private readonly Func<string, RaumfreiViewModel> _raumfreiFactory;

        public ExtrasContentPageViewModel(INavigator navigator, DataLoaderProvider dataLoaderProvider
            , Func<string, SprungbrettViewModel> sprungbrettFactory
            , Func<string, GeneralWebViewPageViewModel> generalWebViewFactory
            , Func<string, RaumfreiViewModel> raumfreiFactory)
            : base(dataLoaderProvider)
        {
            NoteInternetText = AppResources.NoteInternet;
            Title = AppResources.Extras;
            Icon = Device.RuntimePlatform == Device.Android ? null : "extras100";
            _navigator = navigator;
            _generalWebViewFactory = generalWebViewFactory;
            _sprungbrettFactory = sprungbrettFactory;
            _raumfreiFactory = raumfreiFactory;
            ItemTappedCommand = new Command(OnExtraTapped);

            Extras = new ObservableCollection<Extra>();

            ChangeLanguageCommand = new Command(OnChangeLanguage);

            // add toolbar items
            ToolbarItems = GetPrimaryToolbarItemsTranslate(ChangeLanguageCommand);
        }

        public ObservableCollection<Extra> Extras
        {
            get => _extras;
            private set => SetProperty(ref _extras, value);
        }

        public ICommand ItemTappedCommand
        {
            get => _itemTappedCommand;
            set => SetProperty(ref _itemTappedCommand, value);
        }

        public ICommand ChangeLanguageCommand
        {
            get => _changeLanguageCommand;
            set => SetProperty(ref _changeLanguageCommand, value);
        }

        public string NoteInternetText
        {
            get => _noteInternetText;
            set => SetProperty(ref _noteInternetText, value);
        }

        private async void OnChangeLanguage(object obj)
        {
            if (IsBusy) return;
            ContentContainerViewModel.Current.OpenLanguageSelection(false);
        }

        private async void OnExtraTapped(object obj)
        {
            var extra = (Extra)obj;
            BaseViewModel view;

            //special favours for sprungbrett and lehrstellenradar
            if (extra.Alias == "sprungbrett")
                view = _sprungbrettFactory(extra.Url);
            else if (extra.Alias == "wohnen" && extra.Post.TryGetValue("api-name", out var apiName) && apiName == "neuburgschrobenhausenwohnraum")
                    view = _raumfreiFactory(apiName);
            else
                    view = _generalWebViewFactory(extra.Url);


            if (extra.Alias == "lehrstellen-radar")
                ((GeneralWebViewPageViewModel)view).Source = "https://www.lehrstellen-radar.de/5100,0,lsrlist.html";

            view.Title = extra.Name;

            if (!extra.Post.IsNullOrEmpty() && view is GeneralWebViewPageViewModel model)
                model.PostData = extra.Post;


            await _navigator.PushAsync(view, Navigation);
        }

        protected override async void LoadContent(bool forced = false, Language forLanguage = null,
            Location forLocation = null)
        {
            // if location or language is null, use the last used items
            if (forLocation == null) forLocation = LastLoadedLocation;
            if (forLanguage == null) forLanguage = LastLoadedLanguage;

            if (IsBusy || forLocation == null || forLanguage == null)
            {
                Debug.WriteLine("LoadExtras could not be executed");
                return;
            }

            try
            {
                IsBusy = true;
                Extras?.Clear();
                var extras = await DataLoaderProvider.ExtrasDataLoader.Load(forced, forLanguage, forLocation);

                // sort Extras after complete insertion
                Extras = new ObservableCollection<Extra>(extras.OrderBy(e => e.Name));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
