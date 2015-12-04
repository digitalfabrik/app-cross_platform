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

        public static void RemovePage(Language language, Location location)
        {
            AppSettings.Remove(MakePageKey(language, location));
        }

        public static void RemoveEventPage(Language language, Location location)
        {
            AppSettings.Remove(MakeEventPageKey(language, location));
        }

        public static void RemoveDisclaimer(Language language, Location location)
        {
            AppSettings.Remove(MakePageDisclaimerKey(language, location));
        }

        public static DateTime LastLocationUpdateTime()
        {
            return AppSettings.GetValueOrDefault<DateTime>(LastLocationUpdate);
        }

        public static void SetLastLocationUpdateTime()
        {
            AppSettings.AddOrUpdateValue(LastLocationUpdate, DateTime.Now);
        }

        public static DateTime LastEventPageUpdateTime(Language language, Location location)
        {
            return AppSettings.GetValueOrDefault<DateTime>(MakeEventPageKey(language, location));
        }

        private static string MakeEventPageKey(Language language, Location location)
        {
            return $"event_page_key({language.PrimaryKey})({location.Id})";
        }

        private static string MakePageKey(Language language, Location location)
        {
            return $"page_key({language.PrimaryKey})({location.Id})";
        }

        private static string MakePageDisclaimerKey(Language language, Location location)
        {
            return $"disclaimer_key({language.PrimaryKey})({location.Id})";
        }

        public static void SetLastEventPageUpdateTime(Language language, Location location)
        {
            AppSettings.AddOrUpdateValue(MakeEventPageKey(language, location), DateTime.Now);
        }

        public static DateTime LastLanguageUpdateTime(Location location)
        {
            return AppSettings.GetValueOrDefault<DateTime>(MakeLocationKey(location));
        }

        private static string MakeLocationKey(Location location)
        {
            return $"location_key({location.Id})";
        }

        public static void SetLastLanguageUpdateTime(Location location)
        {
            AppSettings.AddOrUpdateValue(MakeLocationKey(location), DateTime.Now.Ticks);
        }

        public static DateTime LastPageUpdateTime(Language language, Location location)
        {
            return AppSettings.GetValueOrDefault<DateTime>(MakePageKey(language, location));
        }

        public static void SetLastPageUpdateTime(Language language, Location location)
        {
            AppSettings.AddOrUpdateValue(MakePageKey(language, location), DateTime.Now);
        }

        public static DateTime LastPageDisclaimerUpdateTime(Language language, Location location)
        {
            return AppSettings.GetValueOrDefault<DateTime>(MakePageDisclaimerKey(language, location));
        }

        public static void SetLastPageDisclaimerUpdateTime(Language language, Location location)
        {
            AppSettings.AddOrUpdateValue(MakePageDisclaimerKey(language, location), DateTime.Now);
        }
    }
}
