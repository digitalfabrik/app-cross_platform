using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Integreat.Shared.Pages;
using Xamarin.Forms;

namespace Integreat.Shared.Services
{
    public class ToolbarService
    {
        private ObservableCollection<ToolbarItem> _toolbarItems;
        private MainNavigationPage _mainNavPage;

        public ToolbarService()
        {
            _toolbarItems = new ObservableCollection<ToolbarItem>();
            _mainNavPage = (MainNavigationPage)Application.Current.MainPage;

            _toolbarItems.CollectionChanged += ToolbarItemsChanged;
        }

        public IList<ToolbarItem> ToolbarItems { get => _toolbarItems; }

        private void ToolbarItemsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }
    }
}
