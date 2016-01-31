using Autofac;
using Integreat.ApplicationObject;
using Integreat.Shared.Models;
using Integreat.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistance;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace Integreat.Shared
{
	public class LanguagesViewModel : BaseViewModel
	{
		public LanguagesLoader LanguagesLoader;

		public LanguagesViewModel (Func<Location, LanguagesLoader> languageLoaderFactory, PersistenceService persistenceService)
        {
            Title = "Languages";
            var locationId = Preferences.Location();
		    var location = persistenceService.Get<Location>(locationId).Result;
		    LanguagesLoader = languageLoaderFactory(location); //TODO wont work
			Items = new ObservableCollection<Language> ();
			ExecuteLoadLanguages ();
		}

		public ObservableCollection<Language> Items { get; set; }

		private async void ExecuteLoadLanguages ()
		{
			//			var loadedItems = await LocationsLoader.Load ();
			//			Console.WriteLine ("Locations loaded");

			Items.Clear ();
			//			Items.AddRange (loadedItems);
			for (int i = 0; i < 10; i++) {
				Items.Add (new Language () {
					Name = string.Format ("Location {0}", i),
					IconPath = "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//wp-content//uploads//sites//10//2015//10//cropped-Regensburg.jpg"
				});
			}
		}
	}
}

