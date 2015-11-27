using SQLite.Net;

namespace Integreat
{
	public interface ISqLite
	{
		SQLiteConnection GetConnection();
	}
}

