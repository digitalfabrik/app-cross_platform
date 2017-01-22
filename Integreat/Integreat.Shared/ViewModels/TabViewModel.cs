using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;

namespace Integreat.Shared.ViewModels
{
    public class TabViewModel : BaseViewModel
    {
        public ObservableCollection<Page> Childs
        {
            get { return _childs; }
            set { SetProperty(ref _childs, value); }
        }


        public TabViewModel(IAnalyticsService analytics,INavigator navigator)
        : base (analytics) {
            Title = "Tabs";
            Console.WriteLine("TabViewModel initialized");
            navigator.HideToolbar(this);
            Childs = new ObservableCollection<Page>();
        }
        

        private Command _changeLanguageCommand;
        public Command ChangeLanguageCommand
        {
            get { return _changeLanguageCommand; }
            set { SetProperty(ref _changeLanguageCommand, value); }
        }

        private Command _openSearchCommand;
        private ObservableCollection<Page> _childs;

        public Command OpenSearchCommand
        {
            get { return _openSearchCommand; }
            set { SetProperty(ref _openSearchCommand, value); }
        }
        
    }
}
