using System;
using Integreat.Services;
using System.Threading.Tasks;
using Integreat.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Collections.Generic;
using Integreat.Shared.Models;

namespace Integreat.Shared.Services
{
	public class NetworkServiceMock : INetworkService
	{
		Task<string> INetworkService.IsServerAlive ()
		=> Task.Factory.StartNew (() => "some string");

		Task<Collection<Disclaimer>> INetworkService.GetDisclaimers (Language language, Location location, UpdateTime time)
		=> Task.Factory.StartNew(() => new Collection<Disclaimer> {
			new Disclaimer()
		});

		Task<Collection<Page>> INetworkService.GetPages (Language language, Location location, UpdateTime time)
		=> Task.Factory.StartNew(() => new Collection<Page>{
			new Page(
		// int id, string title, string type, string status, DateTime modified, string excerpt, string content,
				1, "Seitentitel", "page", "publish", new DateTime(2016, 01, 23), "Auszug", "InhaltInhalt InhaltInhaltInhalt",
		// 				int parentId, int order, string thumbnail, Author author, bool? autoTranslated,
				0, 0, null, new Author("login", "Mux", "Mastermann"), false, 
		// List<AvailableLanguage> availableLanguages
				new List<AvailableLanguage> ())
		});

		Task<HttpResponseMessage> INetworkService.GetPagesDebug (Language language, Location location, UpdateTime time)
		=> Task.Factory.StartNew(() => new HttpResponseMessage());

		Task<Collection<EventPage>> INetworkService.GetEventPages (Language language, Location location, UpdateTime time)
		=> Task.Factory.StartNew(() => new Collection<EventPage>{});

		Task<Collection<Location>> INetworkService.GetLocations ()
		=> Task.Factory.StartNew(() => new Collection<Location>{
			new Location (0, "Augsburg", 
				"http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//wp-content//uploads//sites//2//2015//10//cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg", 
				"http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/", 
				"Es schwäbelt", "yellow", "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/11/cropped-Augsburg.jpg", 
				0, 0, false)
		});

		Task<Collection<Language>> INetworkService.GetLanguages (Location location)
		=> Task.Factory.StartNew(() => new Collection<Language>{
			new Language (0, "de", "Deutsch", "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//augsburg//wp-content//plugins//sitepress-multilingual-cms//res//flags//de.png")
		});

		Task<string> INetworkService.SubscribePush (Location location, string regId)
		=> Task.Factory.StartNew(() => "" /* TODO: need to lookup what goes here */);

		Task<string> INetworkService.UnsubscribePush (Location location, string regId)
		=> Task.Factory.StartNew(() => "" /* TODO: need to lookup what goes here */);
	}
}

