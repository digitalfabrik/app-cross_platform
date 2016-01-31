using SQLite;
using SQLite.Net;
using SQLite.Net.Async;

namespace Integreat.Services
{
	public interface ISqLite
	{
		SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
	}
}

