using System.Collections.ObjectModel;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;

namespace Integreat.Shared.ViewModels
{
	public class LocationsViewModel : BaseViewModel
	{
		public LocationsLoader _locationsLoader;

        public LocationsViewModel(LocationsLoader locationsLoader)
        {
            Title = "Locations";
            _locationsLoader = locationsLoader;
            Items = new ObservableCollection<Location>();
            ExecuteLoadLocations();
        }

		public ObservableCollection<Location> Items { get; set; }

		private async void ExecuteLoadLocations ()
		{
//			var loadedItems = await LocationsLoader.Load ();
//			Console.WriteLine ("Locations loaded");

			Items.Clear ();
//			Items.AddRange (loadedItems);
			for (int i = 0; i < 10; i++) {
				Items.Add (new Location () { 
					Name = string.Format ("Location {0}", i),
					CityImage = "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//wp-content//uploads//sites//10//2015//10//cropped-Regensburg.jpg"
				});
			}
		}
	}
}
