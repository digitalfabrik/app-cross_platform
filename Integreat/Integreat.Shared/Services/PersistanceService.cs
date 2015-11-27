using Xamarin.Forms;
using SQLite.Net;

namespace Integreat
{
	// http://code.tutsplus.com/tutorials/an-introduction-to-xamarinforms-and-sqlite--cms-23020
	public partial class PersistanceService : IPersistanceService
	{
	    private static readonly object Locker = new object ();
	    readonly SQLiteConnection _database;

		public PersistanceService ()
		{
			_database = DependencyService.Get<ISqLite>().GetConnection ();
			_database.CreateTable<Author>();
			_database.CreateTable<AvailableLanguage>();
			_database.CreateTable<Event>();
			_database.CreateTable<EventCategory>();
			_database.CreateTable<EventLocation>();
			_database.CreateTable<EventPage>();
			_database.CreateTable<EventTag>();
			_database.CreateTable<Language>();
			_database.CreateTable<Location>();
			_database.CreateTable<Page>();
		}

	}
}

