using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Integreat
{
	public partial class PersistanceService {

		public IEnumerable<Page> GetPages(Language language, Location location)
		{
			lock (Locker) {
				return (from i in _database.Table<Page> ().Where (x => x.Language.Id == language.Id
				            && x.Language.Location.Id == location.Id)
				        select i);
			}
		}

		public Page GetPage(int id){
			lock (Locker) {
				return _database.Table<Page>().FirstOrDefault(x => x.Id == id);
			}
		}

		public int SavePage (Page page) 
		{
			lock (Locker) {
				if (page.Id != 0) {
					_database.Update(page);
					return page.Id;
				} else {
					return _database.Insert(page);
				}
			}
		}

		public int DeletePage(int id)
		{
			lock (Locker) {
				return _database.Delete<Page>(id);
			}
		}
	}
}


