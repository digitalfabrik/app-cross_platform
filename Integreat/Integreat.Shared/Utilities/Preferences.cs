using System;
using Integreat.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Integreat.Shared.Utilities
{
    public class Preferences
    {
        private static ISettings AppSettings => CrossSettings.Current;
        private const string LastLocationUpdate = "last_location_update";

        public static void RemoveLocation()
        {
            AppSettings.Remove(LastLocationUpdate);
        }

        public static void RemoveLanguage(Location location)
        {
            AppSettings.Remove(MakeLocationKey(location));
        }

        public static void RemovePage<T>(Language language, Location location)
        {
            AppSettings.Remove(MakePageKey<T>(language, location));
        }

        public static DateTime LastLocationUpdateTime()
        {
            return AppSettings.GetValueOrDefault<DateTime>(LastLocationUpdate);
        }

        public static void SetLastLocationUpdateTime()
        {
            AppSettings.AddOrUpdateValue(LastLocationUpdate, DateTime.Now);
        }

        public static DateTime LastLanguageUpdateTime(Location location)
        {
            return AppSettings.GetValueOrDefault<DateTime>(MakeLocationKey(location));
        }

        public static void SetLastLanguageUpdateTime(Location location)
        {
            AppSettings.AddOrUpdateValue(MakeLocationKey(location), DateTime.Now.Ticks);
        }

        public static DateTime LastPageUpdateTime<T>(Language language, Location location)
        {
            return AppSettings.GetValueOrDefault<DateTime>(MakePageKey<T>(language, location));
        }

        public static void SetLastPageUpdateTime<T>(Language language, Location location)
        {
            AppSettings.AddOrUpdateValue(MakePageKey<T>(language, location), DateTime.Now);
        }

        private static string MakeLocationKey(Location location)
        {
            return $"location_key({location.Id})";
        }
        
        private static string MakePageKey<T>(Language language, Location location)
        {
            return $"{typeof(T).FullName}({language.Id})({location.Id})";
        }
        
    }
}
