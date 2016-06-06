﻿using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class TabViewModel : BaseViewModel
    {
        public PagesViewModel PagesViewModel { get; }
        public EventPagesViewModel EventPagesViewModel { get; }

        public TabViewModel(IAnalyticsService analytics, PagesViewModel pagesViewModel, EventPagesViewModel eventPagesViewModel, INavigator navigator)
        : base (analytics) {
            Title = "Tabs";
            Console.WriteLine("TabViewModel initialized");
            PagesViewModel = pagesViewModel;
            EventPagesViewModel = eventPagesViewModel;
            navigator.HideToolbar(this);
        }

        public IEnumerable<PageViewModel> GetPages()
        {
            return PagesViewModel.LoadedPages.Union(EventPagesViewModel.EventPages);
        }


        public void SetLanguageLocation(Language language, Location location)
        {
            PagesViewModel.SetLanguageLocation(language, location);
            EventPagesViewModel.SetLanguageLocation(language, location);
        }

        // this will refresh page and events automatically
        public void SetLanguage(Language language)
        {
            PagesViewModel.Language = language;
            EventPagesViewModel.Language = language;
        }

        public void SetLocation(Location location)
        {
            PagesViewModel.Location = location;
            EventPagesViewModel.Location = location;
        }


        private Command _changeLanguageCommand;
        public Command ChangeLanguageCommand
        {
            get { return _changeLanguageCommand; }
            set { SetProperty(ref _changeLanguageCommand, value); }
        }

        private Command _openSearchCommand;
        public Command OpenSearchCommand
        {
            get { return _openSearchCommand; }
            set { SetProperty(ref _openSearchCommand, value); }
        }
        
    }
}