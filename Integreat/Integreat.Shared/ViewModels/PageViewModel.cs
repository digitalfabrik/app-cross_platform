using System;
using System.Linq;
using Integreat.Shared.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;
        private readonly IDialogProvider _dialogProvider;

        public Models.Page Page { get; set; }

        public PageViewModel(INavigator navigator, Models.Page page, IDialogProvider dialogProvider)
        {
            Title = page.Title;
            _navigator = navigator;
            _dialogProvider = dialogProvider;
            Page = page;
            ShowPageCommand = new Command(ShowPage);
        }

        public string Content => Page.Content;
        public string Description => Page.Description;
        public string Thumbnail => Page.Thumbnail;

        public ICommand ShowPageCommand { get; set; }

        private async void ShowPage()
        {
            // await _navigator.PopModalAsync();
            await _navigator.PushAsync(this);
        }

        private Command _openSearchCommand;
        public Command OpenSearchCommand => _openSearchCommand ?? (_openSearchCommand = new Command(OnSearchClicked));

        private void OnSearchClicked()
        {
        }

        private Command _changeLanguageCommand;
        public Command ChangeLanguageCommand => _changeLanguageCommand ?? (_changeLanguageCommand = new Command(OnChangeLanguageClicked));

        private async void OnChangeLanguageClicked()
        {
            if (Page.AvailableLanguages.IsNullOrEmpty())
            {
                //TODO maybe show user message that its not possible to switch language
                return;
            }
            var action = await _dialogProvider.DisplayActionSheet("Select a Language?", "Cancel", null,
                        Page.AvailableLanguages.Select(x => x.Language).ToArray());
            var selectedLanguage = Page.AvailableLanguages.FirstOrDefault(x => x.Language.Equals(action));
            Console.WriteLine(selectedLanguage?.Language ?? "No language selected");
            //TODO load language with primary key selectedLanguage.PrimaryKey
        }
    }
}
