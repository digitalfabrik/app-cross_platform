﻿using System;
using System.Linq;
using Integreat.Shared.Services;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;
        private readonly IDialogProvider _dialogProvider;
        private readonly IEnumerable<PageViewModel> _pages;

        public Models.Page Page { get; set; }

        public PageViewModel(INavigator navigator, Models.Page page, IDialogProvider dialogProvider,
            IEnumerable<PageViewModel> pages)
        {
            Title = page.Title;
            _navigator = navigator;
            _dialogProvider = dialogProvider;
            _pages = pages;
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

        private async void ShowPage(object modal)
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
        public Command ChangeLanguageCommand => _changeLanguageCommand ?? (_changeLanguageCommand = new Command(OnChangeLanguageClicked));

        private async void OnChangeLanguageClicked()
        {
            if (Page.AvailableLanguages.IsNullOrEmpty())
            {
                await _dialogProvider.DisplayActionSheet("No other languages available", "OK", null);
                return;
            }
            var action = await _dialogProvider.DisplayActionSheet("Select a Language", "Cancel", null,
                        Page.AvailableLanguages.Select(x => x.Language).ToArray());
            var selectedLanguage = Page.AvailableLanguages.FirstOrDefault(x => x.Language.Equals(action));
            Console.WriteLine(selectedLanguage?.Language ?? "No language selected");
            // show page
            var otherPageId = selectedLanguage.OtherPageId;
            var otherPage = _pages.Where(x => x.Page.Id == otherPageId);
            ShowPage(otherPage);
        }
    }
}
