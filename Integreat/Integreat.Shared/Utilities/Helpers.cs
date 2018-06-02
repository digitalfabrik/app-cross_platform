
using System;
using System.IO;
using Integreat.Shared.ViewFactory;
using Integreat.Shared.ViewModels;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace Integreat.Utilities
{
    public static class Helpers
    { /// <summary>
      /// This is the Settings static class that can be used in your Core solution or in any
      /// of your client applications. All settings are laid out the same exact way with getters
      /// and setters. 
      /// </summary>
        public static class Settings
        {
            private static ISettings AppSettings => CrossSettings.Current;

            #region Setting Constants

            private const string SettingsKey = "settings_key";
            private static readonly string SettingsDefault = string.Empty;

            #endregion

            public static string GeneralSettings
            {
                get => AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
                set => AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        public static class Platform
        {
            public static string GetVersion()
            {
                // ReSharper disable once RedundantAssignment

#if __ANDROID__
                var context = global::Android.App.Application.Context;
                var version = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
#elif __IOS__
                var version = Foundation.NSBundle.MainBundle.InfoDictionary[new Foundation.NSString("CFBundleVersion")].ToString();
#else
                version = Constants.CurrentAppVersion;
#endif
                return version;
            }

            public static string GetCachedFilePath()
            {
                const string filePrefix = "";
#if NETFX_CORE
			var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, filePrefix);
#else

#if __ANDROID__
                // Just use whatever directory SpecialFolder.Personal returns
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
                // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                // (they don't want non-user-generated data in Documents)
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                var libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
#endif
                var path = Path.Combine(libraryPath, filePrefix);
#endif
                return path;
            }

            public static string GetDatabasePath()
            {

                const string sqliteFilename = "_v2_";

#if __ANDROID__
                // Just use whatever directory SpecialFolder.Personal returns
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
                // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                // (they don't want non-user-generated data in Documents)
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                var libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
#endif
                var path = Path.Combine(libraryPath, sqliteFilename);

                return path;
            }

            public static void GetCurrentMainPage(IViewFactory viewFactory)
            {
#if __ANDROID__
                Application.Current.MainPage = new NavigationPage(viewFactory.Resolve<ContentContainerViewModel>());
#else
                Application.Current.MainPage = viewFactory.Resolve<ContentContainerViewModel>();
#endif
            }
        }
    }
}
