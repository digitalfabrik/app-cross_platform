using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Integreat.Shared.Pages;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.Services
{
    public class ToolbarService
    {
        private readonly ObservableCollection<IntegreatToolbarItem> _toolbarItems;
        private readonly ContentContainerPage _mainPage;

        public ToolbarService()
        {
            _toolbarItems = new ObservableCollection<IntegreatToolbarItem>();
            _mainPage = Application.Current.MainPage is ContentContainerPage?(ContentContainerPage) Application.Current.MainPage:null;

            _toolbarItems.CollectionChanged += ToolbarItemsChanged;
        }

        public IList<IntegreatToolbarItem> ToolbarItems { get => _toolbarItems; }

        private void ToolbarItemsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException("not implemented yet");
        }

        public void AddToolbarItem(IntegreatToolbarItem item)
        {
            if(!_toolbarItems.Any(i => i.Identifier == item.Identifier))
            {
                ToolbarItems.Add(item);
            }
        }
    }
}
