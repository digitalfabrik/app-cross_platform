using System;
using System.Collections.Generic;
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
        private string _plzHwk;
        private string _noteInternetText;
        private BaseContentViewModel _activeViewModel;
        private readonly Func<string, GeneralWebViewPageViewModel> _generalWebViewFactory;
        private ICommand _itemTappedCommand;
        private ICommand _changeLanguageCommand;
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

            ContentContainerViewModel.Current.OpenLanguageSelection();
        }

        private async void OnExtraTapped(object obj)
        {
            var extra = (Extra)obj;
            var view = _generalWebViewFactory(extra.Url);
            view.Title = extra.Name;

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

            // set result text depending whether push notifications are available or not
            //NoResultText = AppResources.NoEvents;

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

            //_activeViewModel?.RefreshCommand.Execute(forced);
        }
        /*
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
        */
    }
}
