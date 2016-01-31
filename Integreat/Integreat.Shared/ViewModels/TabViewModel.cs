
using Integreat.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class TabViewModel : BaseViewModel
    {
        public PagesViewModel PagesViewModel { get; }
        public EventPagesViewModel EventPagesViewModel { get; }

        private readonly Func<IEnumerable<PageViewModel>, SearchViewModel> _pageSearchViewModelFactory;
        private readonly IDialogProvider _dialogProvider;
        private readonly INavigator _navigator;
        private readonly PersistenceService _persistence;
        private Location _location;

        public TabViewModel(IDialogProvider dialogProvider, INavigator navigator, PagesViewModel pagesViewModel, 
            EventPagesViewModel eventPagesViewModel,Func<IEnumerable<PageViewModel>, SearchViewModel> pageSearchViewModelFactory, PersistenceService persistence)
        {
            Title = "Tabs";
            Console.WriteLine("TabViewModel initialized");
            PagesViewModel = pagesViewModel;
            EventPagesViewModel = eventPagesViewModel;
            _dialogProvider = dialogProvider;
            _navigator = navigator;
            _pageSearchViewModelFactory = pageSearchViewModelFactory;
            _persistence = persistence;
        }

        private async Task<IEnumerable<Language>> LoadLanguages()
        {
            return await _persistence.GetLanguages(_location ?? (_location = await _persistence.Get<Location>(Preferences.Location())));
        }


        private async void OnChangeLanguageClicked()
        {
            var languages = (await LoadLanguages()).ToList();
            var action = await _dialogProvider.DisplayActionSheet("Select a Language?", "Cancel", null, languages.Select(x => x.Name).ToArray());
            var selectedLanguage = languages.FirstOrDefault(x => x.Name.Equals(action));
            Console.WriteLine(selectedLanguage?.Name ?? "No language selected");
            if (selectedLanguage != null)
            {
                //TODO load pages, update page/event and add data to navigation too
            }
        }

        async void OnSearchClicked()
        {
            var allPages = new Collection<PageViewModel>();
            allPages.AddRange(PagesViewModel.VisiblePages);
            allPages.AddRange(EventPagesViewModel.EventPages);
            await _navigator.PushAsync(_pageSearchViewModelFactory(allPages));
        }

        private Command _openSearchCommand;
        public Command OpenSearchCommand => _openSearchCommand ??
                                            (_openSearchCommand = new Command(OnSearchClicked));

        private Command _changeLanguageCommand;
        public Command ChangeLanguageCommand => _changeLanguageCommand ??
                                                (_changeLanguageCommand = new Command(OnChangeLanguageClicked));
    }
}
