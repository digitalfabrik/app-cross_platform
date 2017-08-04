using System;
using System.IO;

namespace Integreat.Utilities
{
    //based on https://github.com/xamarin/mobile-samples/blob/master/Tasky/TaskySharedCode/TodoItemRepositoryADO.cs
    public class Constants
    {

        //public const string IntegreatReleaseUrl = "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/";
        public const string IntegreatReleaseUrl = "https://web.integreat-app.de";
        public const string IntegreatTestUrl = "https://cms-test.integreat-app.de/";

        public static string CachedFilePath {
            get {
                const string filePrefix = "";
#if NETFX_CORE
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, filePrefix);
#else

#if SILVERLIGHT
// Windows Phone expects a local path, not absolute
				var path = sqliteFilename;
#else

#if __ANDROID__
// Just use whatever directory SpecialFolder.Personal returns
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
#else
                // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                // (they don't want non-user-generated data in Documents)
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
#endif
                var path = Path.Combine(libraryPath, filePrefix);
#endif

#endif
                return path;
            }
        }

        public static string DatabaseFilePath
        {
            get
            {
                var sqliteFilename = "_v2_";
#if NETFX_CORE
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
#else

#if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
				var path = sqliteFilename;
#else

#if __ANDROID__
                // Just use whatever directory SpecialFolder.Personal returns
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
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
