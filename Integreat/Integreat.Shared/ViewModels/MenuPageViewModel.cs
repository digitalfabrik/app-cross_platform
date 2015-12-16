using System;
using System.Collections.ObjectModel;
using Integreat.Shared.Models;
using Xamarin.Forms;
using Page = Integreat.Models.Page;

namespace Integreat.Shared.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        public ObservableCollection<HomeMenuItem> Pages { get; set; }

        public MenuPageViewModel()
        {
            Title = "Augsburg";
            ImageSource = new UriImageSource
            {
                Uri =
                    new Uri("http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/11/cropped-Augsburg.jpg"),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(1, 0, 0, 0)
            };
            Icon = null;
            Pages = new ObservableCollection<HomeMenuItem>();
        }
    }
}
