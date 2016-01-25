
using System;
using System.Collections.ObjectModel;
using Integreat.Shared.ViewModels;
using Integreat.Models;
using System.Collections.Generic;
using Integreat.Shared.Services.Loader;
using Integreat.ApplicationObject;
using Autofac;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;

namespace Integreat.Shared.ViewModels
{
	public class LocationsViewModel : BaseViewModel
	{
		public LocationsLoader LocationsLoader;

		public LocationsViewModel ()
		{
			using (AppContainer.Container.BeginLifetimeScope ()) {
				var persistence = AppContainer.Container.Resolve<PersistenceService> ();
				var network = AppContainer.Container.Resolve<INetworkService> ();
				LocationsLoader = new LocationsLoader (persistence, network);
			}
			Items = new ObservableCollection<Location> ();
			ExecuteLoadLocations ();
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
