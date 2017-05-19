using System;
using Integreat.Shared.Models;
using Plugin.Connectivity.Abstractions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Integreat.Shared.Utilities
{
    public class Preferences
    {
        private static ISettings AppSettings => CrossSettings.Current;

        private const string ConnectionTypeKey = "connection_type";

        public static ConnectionType ConnectionType
        {
            get
            {
                return (ConnectionType) AppSettings.GetValueOrDefault(ConnectionTypeKey, (int)ConnectionType.Cellular);
            }
            set { AppSettings.AddOrUpdateValue(ConnectionTypeKey, (int)value); }
        }

        private const string LastLocationKey = "last_location";
        private const string LastLocationUpdate = "last_location_update";

        public static void RemoveLocation()
        {
            AppSettings.Remove(LastLocationKey);
        }

        public static void SetLocation(Location location)
        {
            AppSettings.AddOrUpdateValue(LastLocationKey, location.Id);
        }

        public static int Location()
        {
            return AppSettings.GetValueOrDefault<int>(LastLocationKey);
        }

        public static string Language(int locationId)
        {
            return AppSettings.GetValueOrDefault<string>(MakeLocationKey(locationId));
        }
        public static string Language(Location location)
        {
            if (location == null)
            {
                return null;
            }
            return AppSettings.GetValueOrDefault<string>(MakeLocationKey(location));
        }

        public static void SetLanguage(Location location, Language language)
        {
            AppSettings.AddOrUpdateValue(MakeLocationKey(location), language.PrimaryKey);
        }

        public static void SetLanguage(int locationId, Language language)
        {
            AppSettings.AddOrUpdateValue(MakeLocationKey(locationId), language.PrimaryKey);
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

        public static void SetLastLocationUpdateTime(DateTime to)
        {
            AppSettings.AddOrUpdateValue(LastLocationUpdate, to);
        }

        public static DateTime LastLanguageUpdateTime(Location location)
        {
            return AppSettings.GetValueOrDefault<DateTime>(MakeLocationUpdateKey(location));
        }

        public static void SetLastLanguageUpdateTime(Location location, DateTime to)
        {
            AppSettings.AddOrUpdateValue(MakeLocationUpdateKey(location), to.Ticks);
        }

        public static DateTime LastPageUpdateTime<T>(Language language, Location location)
        {
            return AppSettings.GetValueOrDefault<DateTime>(MakePageKey<T>(language, location));
        }

        public static void SetLastPageUpdateTime<T>(Language language, Location location, DateTime to)
        {
            AppSettings.AddOrUpdateValue(MakePageKey<T>(language, location), to);
        }

        private static string MakeLocationUpdateKey(Location location)
        {
            return MakeLocationKey(location) + "_update";
        }

        private static string MakeLocationKey(Location location)
        {
            return $"location_key({location.Id})";
        }

        private static string MakeLocationKey(int location)
        {
            return $"location_key({location})";
        }

        private static string MakePageKey<T>(Language language, Location location)
        {
            return $"{typeof(T).FullName}({language.Id})({location.Id})_update";
        }
        
    }
}
