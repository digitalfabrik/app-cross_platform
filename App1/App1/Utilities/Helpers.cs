using System;
using System.IO;
using App1.ViewFactory;
using App1.ViewModels;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace App1.Utilities
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

            public static void GetCurrentMainPage(IViewFactory viewFactory)
            {
                Application.Current.MainPage = PlatformHelper.GetInstance().MainPageFunc(viewFactory);
            }
        }
    }
}
