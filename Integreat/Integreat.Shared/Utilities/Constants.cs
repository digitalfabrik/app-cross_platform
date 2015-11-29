using System;
using System.IO;

namespace Integreat.Utilities
{
    //based on https://github.com/xamarin/mobile-samples/blob/master/Tasky/TaskySharedCode/TodoItemRepositoryADO.cs
    public class Constants
    {
        public static string DatabaseFilePath
        {
            get
            {
                //TODO remove this later ;)
                var sqliteFilename = DateTime.Now.ToRestAcceptableString() + "_integreat.db3";
#if NETFX_CORE
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
#else

#if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
				var path = sqliteFilename;
#else

#if __ANDROID__
                // Just use whatever directory SpecialFolder.Personal returns
                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
#else
				// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
				// (they don't want non-user-generated data in Documents)
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
#endif
                var path = Path.Combine(libraryPath, sqliteFilename);
#endif

#endif
                return path;
            }
        }
    }
}
