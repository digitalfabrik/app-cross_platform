using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Integreat.Shared.Models;
using Integreat.Shared.Models;
using Integreat.Utilities;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using SQLiteNetExtensionsAsync.Extensions;

namespace Integreat.Shared.Services.Persistance
{
	// http://code.tutsplus.com/tutorials/an-introduction-to-xamarinforms-and-sqlite--cms-23020
	public partial class PersistenceService
	{
	    public PersistenceService(ISQLitePlatform platform, string databaseFilePath)
        {
	        var connectionString = new SQLiteConnectionString(databaseFilePath, false);
	        _connLock = new SQLiteConnectionWithLock(platform, connectionString);
        }

        public PersistenceService (ISQLitePlatform platform)
        {
            var connectionString = new SQLiteConnectionString(Constants.DatabaseFilePath, false);
            _connLock = new SQLiteConnectionWithLock(platform, connectionString);
        }
        
	    private readonly SQLiteConnectionWithLock _connLock;
	    public SQLiteAsyncConnection Connection => new SQLiteAsyncConnection(() => _connLock);

        public void Init()
	    {
            Task[] tasks =
               {
                Connection.CreateTableAsync<Author>(),
                Connection.CreateTableAsync<AvailableLanguage>(),
                Connection.CreateTableAsync<Event>(),
                Connection.CreateTableAsync<EventCategory>(),
                Connection.CreateTableAsync<EventLocation>(),
                Connection.CreateTableAsync<EventPage>(),
                Connection.CreateTableAsync<EventTag>(),
                Connection.CreateTableAsync<Location>(),
                Connection.CreateTableAsync<Language>(),
                Connection.CreateTableAsync<Page>(),
                Connection.CreateTableAsync<Disclaimer>()
            };

            Task.WaitAll(tasks);
            /*return Connection.CreateTablesAsync(
	            typeof (Author),
	            typeof (AvailableLanguage),
	            typeof (Event),
	            typeof (EventCategory),
	            typeof (EventLocation),
	            typeof (EventPage),
	            typeof (EventTag),
	            typeof (Location),
	            typeof (Language),
                typeof (Disclaimer),
	            typeof (Page));*/
	    }

	    public void DropTables()
	    {
	        Task[] tasks =
	        {
                Connection.DropTableAsync<Author>(),
                Connection.DropTableAsync<AvailableLanguage>(),
                Connection.DropTableAsync<Event>(),
                Connection.DropTableAsync<EventCategory>(),
                Connection.DropTableAsync<EventLocation>(),
                Connection.DropTableAsync<EventPage>(),
                Connection.DropTableAsync<EventTag>(),
                Connection.DropTableAsync<Location>(),
                Connection.DropTableAsync<Language>(),
                Connection.DropTableAsync<Page>(),
                Connection.DropTableAsync<Disclaimer>()
            };
	        Task.WaitAll(tasks);
	    }

	    public Task Insert<T>(T element, bool recursive = true)
        {
            return Connection.InsertOrReplaceWithChildrenAsync(element, recursive);
        }

        public Task<int> Delete<T>(T element)
	    {
	        return Connection.DeleteAsync(element);
	    }
        
        public Task InsertAll<T>(Collection<T> elements, bool recursive = true)
        {
            return Connection.InsertOrReplaceAllWithChildrenAsync(elements, recursive);
        }

	    public Task<T> Get<T> (object key, bool recursive=true) where T : class
	    {
	        return Connection.FindWithChildrenAsync<T>(key, recursive);
	    }
       

	}
}

