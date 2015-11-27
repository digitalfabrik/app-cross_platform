using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Integreat
{
	public partial class PersistanceService {
		public IEnumerable<Location> GetLocations()
		{
			lock (Locker) {
				return (from i in _database.Table<Location> () select i);
			}
		}

		public Location GetLocation(int id){
			lock (Locker) {
				return _database.Table<Location>().FirstOrDefault(x => x.Id == id);
			}
		}

		public int SaveLocation (Location location) 
		{
			lock (Locker) {
				if (location.Id != 0) {
					_database.Update(location);
					return location.Id;
				} else {
					return _database.Insert(location);
				}
			}
		}

		public int DeleteLocation(int id)
		{
			lock (Locker) {
				return _database.Delete<Location>(id);
			}
		}
	}
}


