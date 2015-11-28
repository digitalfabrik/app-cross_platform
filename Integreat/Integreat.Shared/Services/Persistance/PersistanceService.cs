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
	    private readonly SQLiteAsyncConnection _database;

        public PersistanceService(ISQLitePlatform platform, string databaseFilePath)
        {
            var connectionString = new SQLiteConnectionString(databaseFilePath, true);
            _database = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(platform, connectionString));
        }

        public PersistanceService (ISQLitePlatform platform)
        {
            var connectionString = new SQLiteConnectionString(Constants.DatabaseFilePath, true);
            _database = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(platform, connectionString));
		}

	    public void Init(bool drop)
        {
            if (drop)
            {
                DropTables();
            }
            Task[] tasks =
	        {
	            //_database.CreateTableAsync<Author>(),
	            //_database.CreateTableAsync<AvailableLanguage>(),
	            //_database.CreateTableAsync<Event>(),
	            //_database.CreateTableAsync<EventCategory>(),
	            //_database.CreateTableAsync<EventLocation>(),
	            //_database.CreateTableAsync<EventPage>(),
	           // _database.CreateTableAsync<EventTag>(),
	            _database.CreateTableAsync<Location>(),
	            _database.CreateTableAsync<Language>()
	            //_database.CreateTableAsync<Page>(),
	        };
            Task.WaitAll(tasks);
	    }

	    private void DropTables()
	    {
            Task[] tasks =
            {
	            //_database.DropTableAsync<Author>(),
	            //_database.DropTableAsync<AvailableLanguage>(),
	            //_database.DropTableAsync<Event>(),
	            //_database.DropTableAsync<EventCategory>(),
	            //_database.DropTableAsync<EventLocation>(),
	            //_database.DropTableAsync<EventPage>(),
	           // _database.DropTableAsync<EventTag>(),
	            _database.DropTableAsync<Location>(),
                _database.DropTableAsync<Language>()
	            //_database.DropTableAsync<Page>(),
	        };
            Task.WaitAll(tasks);
        }

	    public Task Insert<T>(T element)
        {
            return _database.InsertOrReplaceWithChildrenAsync(element);
        }

        public Task<int> Delete<T>(T element)
	    {
	        return _database.DeleteAsync(element);
	    }
        
        public Task InsertAll<T>(Collection<T> elements)
        {
            return _database.InsertOrReplaceAllWithChildrenAsync(elements);
        }

	    public Task<T> Get<T> (object key) where T : class
	    {
	        return _database.FindWithChildrenAsync<T>(key);
	    }

	}
}

