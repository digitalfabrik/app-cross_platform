using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public NavigationViewModel NavigationViewModel { get; }
        public TabViewModel TabViewModel { get; }
        private readonly PagesViewModel _pagesViewModel;

        private readonly Func<IEnumerable<PageViewModel>, SearchViewModel> _pageSearchViewModelFactory;
        private readonly IDialogProvider _dialogProvider;
        private readonly INavigator _navigator;
        private readonly PersistenceService _persistence;
        private Location _location;
        private Language _language;

        public MainPageViewModel(PagesViewModel pagesViewModel, NavigationViewModel navigationViewModel,
            TabViewModel tabViewModel,
            IDialogProvider dialogProvider, INavigator navigator,
            Func<IEnumerable<PageViewModel>, SearchViewModel> pageSearchViewModelFactory, PersistenceService persistence)
        {
            Title = "Information";

            _pagesViewModel = pagesViewModel;
            NavigationViewModel = navigationViewModel;
            NavigationViewModel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName.Equals("SelectedPage"))
                {
                    _pagesViewModel.SelectedPage = NavigationViewModel.SelectedPage;
                }
            };
            _pagesViewModel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName.Equals("LoadedPages"))
                {
                    var pages = _pagesViewModel.LoadedPages;
                    NavigationViewModel.Pages =
                        new ObservableCollection<PageViewModel>(pages.Where(x => x.Page.ParentId <= 0)
                            .OrderBy(x => x.Page.Order));
                }
            };
            TabViewModel = tabViewModel;
            if (_pagesViewModel.LoadPagesCommand.CanExecute(null))
            {
                _pagesViewModel.LoadPagesCommand.Execute(null);
            }

            _dialogProvider = dialogProvider;
            _navigator = navigator;
            _pageSearchViewModelFactory = pageSearchViewModelFactory;
            _persistence = persistence;

            Init();
        }

        private async void Init()
        {
            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            _language = await _persistence.Get<Language>(languageId);
            _location = await _persistence.Get<Location>(locationId);

            TabViewModel.SetLocation(_location);
            TabViewModel.SetLanguage(_language);
            TabViewModel.ChangeLanguageCommand = new Command(OnChangeLanguageClicked);
            TabViewModel.OpenSearchCommand = new Command(OnSearchClicked);

            NavigationViewModel.SetLocation(_location);
            NavigationViewModel.SetLanguage(_language);
        }

        private async Task<IEnumerable<Language>> LoadLanguages()
        {
            return
                await
                    _persistence.GetLanguages(_location ??
                                              (_location = await _persistence.Get<Location>(Preferences.Location())));
        }
        
        private async void OnSearchClicked()
        {
            var allPages = TabViewModel.GetPages();
            await _navigator.PushAsync(_pageSearchViewModelFactory(allPages));
        }
        

        private async void OnChangeLanguageClicked()
        {
            var languages = (await LoadLanguages()).ToList();
            var action =
                await
                    _dialogProvider.DisplayActionSheet("Select a Language?", "Cancel", null,
                        languages.Select(x => x.Name).ToArray());
            var selectedLanguage = languages.FirstOrDefault(x => x.Name.Equals(action));
            Console.WriteLine(selectedLanguage?.Name ?? "No language selected");

            // this will refresh pages and events, and trigger to update the navigation too
            if (selectedLanguage != null)
            {
                Preferences.SetLanguage(Preferences.Location(), selectedLanguage);
                TabViewModel.SetLanguage(selectedLanguage);
            }
        }
    }
}
