using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Integreat.Models;
using Integreat.Utilities;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using SQLiteNetExtensionsAsync.Extensions;

namespace Integreat.Shared.Services.Persistance
{
	// http://code.tutsplus.com/tutorials/an-introduction-to-xamarinforms-and-sqlite--cms-23020
	public partial class PersistanceService
	{
	    public PersistanceService(ISQLitePlatform platform, string databaseFilePath)
        {
	        var connectionString = new SQLiteConnectionString(databaseFilePath, false);
	        _connLock = new SQLiteConnectionWithLock(platform, connectionString);
        }

        public PersistanceService (ISQLitePlatform platform)
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
                Connection.CreateTableAsync<Page>()
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
	            typeof (Page));*/
	    }

	    private void DropTables()
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
                Connection.DropTableAsync<Page>()
	        };
	        Task.WaitAll(tasks);
	    }

	    public Task Insert<T>(T element)
        {
            return Connection.InsertOrReplaceWithChildrenAsync(element);
        }

        public Task<int> Delete<T>(T element)
	    {
	        return Connection.DeleteAsync(element);
	    }
        
        public Task InsertAll<T>(Collection<T> elements)
        {
            return Connection.InsertOrReplaceAllWithChildrenAsync(elements);
        }

	    public Task<T> Get<T> (object key) where T : class
	    {
	        return Connection.FindWithChildrenAsync<T>(key);
	    }
       

	}
}

