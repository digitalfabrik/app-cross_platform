using SQLite.Net;
using SQLite.Net.Async;

namespace Integreat.Shared.Services.Persistence
{
	public interface ISqLite
	{
		SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
	}
}

