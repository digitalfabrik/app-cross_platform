using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Integreat
{
	public partial class PersistanceService {
		public IEnumerable<Language> GetLanguages(Location location)
		{
			lock (Locker) {
				return (from i in _database.Table<Language> ().Where (x => x.Location.Id == location.Id)
					select i);
			}
		}

		public Language GetLanguage(int id){
			lock (Locker) {
				return _database.Table<Language>().FirstOrDefault(x => x.Id == id);
			}
		}

		public int SaveLanguage (Language language) 
		{
			lock (Locker) {
				if (language.Id != 0) {
					_database.Update(language);
					return language.Id;
				} else {
					return _database.Insert(language);
				}
			}
		}

		public int DeleteLanguage(int id)
		{
			lock (Locker) {
				return _database.Delete<Language>(id);
			}
		}
	}
}


