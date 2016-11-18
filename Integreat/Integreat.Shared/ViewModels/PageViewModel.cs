using System;
using System.Diagnostics;
using System.Linq;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;
        private readonly IDialogProvider _dialogProvider;

        public Models.Page Page { get; set; }

        public PageViewModel(IAnalyticsService analytics, INavigator navigator, Models.Page page, IDialogProvider dialogProvider)
        : base(analytics) {
            Title = page.Title;
            _navigator = navigator;
            _dialogProvider = dialogProvider;
            Page = page;
            ShowPageCommand = new Command(ShowPage);
        }

        public string Content => Page.Content;
        public string Description => Page.Description;
        public string Thumbnail => Page.Thumbnail;

        private Command _showPageCommand;

        public Command ShowPageCommand
        {
            get { return _showPageCommand; }
            set { SetProperty(ref _showPageCommand, value); }
        }

        public async void ShowPage(object modal)
        {

            await _navigator.PushAsync(this);
            if ("Modal".Equals(modal?.ToString()))
            {
                await _navigator.PopModalAsync();
            }
        }

        private Command _openSearchCommand;
        public Command OpenSearchCommand => _openSearchCommand ?? (_openSearchCommand = new Command(OnSearchClicked));

        private void OnSearchClicked()
        {
        }

        private Command _changeLanguageCommand;
        private Command _changeLocalLanguageCommand;
        public Command ChangeLanguageCommand => _changeLanguageCommand ?? (_changeLanguageCommand = new Command(OnChangeLanguageClicked));

        public Command ChangeLocalLanguageCommand {
            get { return _changeLocalLanguageCommand; }
            set { _changeLocalLanguageCommand = value; }
        }

        // command that gets executed, when the user wants to change the language for this page instance. Sends this as parameter

        private async void OnChangeLanguageClicked()
        {
            if (Page.AvailableLanguages.IsNullOrEmpty())
            {
                return;
            }

            /*
            var availableLanguages = Page.AvailableLanguages.Select(x => x.OtherPage.Language.Name).ToArray();
            //if (availableLanguages.Length == 0) return;
            var action = await _dialogProvider.DisplayActionSheet("Select a Language?", "Cancel", null,
                        availableLanguages);


            var selectedLanguage = Page.AvailableLanguages.FirstOrDefault(x => x.OtherPage.Language.Name.Equals(action));
            if (selectedLanguage != null)
            {
                Page = selectedLanguage.OtherPage;
            }
            else
            {
                Console.WriteLine("No language selected");
            }*/
        }
    }
}
