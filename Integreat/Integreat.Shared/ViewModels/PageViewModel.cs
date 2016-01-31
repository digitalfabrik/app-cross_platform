using Integreat.Shared.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace Integreat.Shared.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;

        public Models.Page Page { get; set; }

        public PageViewModel(INavigator navigator, Models.Page page)
        {
            Title = page.Title;
            _navigator = navigator;
            Page = page;
            ShowPageCommand = new Command(ShowPage);
        }

        public string Content => Page.Content;
        public string Description => Page.Description;
        public string Thumbnail => Page.Thumbnail;

        public ICommand ShowPageCommand { get; set; }

        private async void ShowPage()
        {
            await _navigator.PushAsync(this);
        }

        private Command _openSearchCommand;
        public Command OpenSearchCommand => _openSearchCommand ??
                                            (_openSearchCommand = new Command(OnSearchClicked));

        private void OnSearchClicked()
        {
        }

        private Command _changeLanguageCommand;
        public Command ChangeLanguageCommand => _changeLanguageCommand ??
                                                (_changeLanguageCommand = new Command(OnChangeLanguageClicked));

        private void OnChangeLanguageClicked()
        {
            throw new NotImplementedException();
        }
    }
}
