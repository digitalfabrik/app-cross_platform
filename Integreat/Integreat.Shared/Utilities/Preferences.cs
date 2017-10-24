﻿using System;
using Integreat.Shared.Models;
using Plugin.Connectivity.Abstractions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Integreat.Shared.Utilities
{
    public class Preferences
    {
        private static bool _wifiOnly;
        private static ISettings AppSettings => CrossSettings.Current;

        private const string LastLocationKey = "last_location";
        private const string LastLocationUpdate = "last_location_update";
        private const string HtmlRawView = "html_raw_view";
        private const string ConnectionTypeKey = "connection_type";
        private const string WifiOnlyKey = "wifi_only";

        public static ConnectionType ConnectionType
        {
            get => (ConnectionType)AppSettings.GetValueOrDefaultExceptionSafe(ConnectionTypeKey, (int)ConnectionType.Cellular);
            set => AppSettings.AddOrUpdateValue(ConnectionTypeKey, (int)value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use [wifi only] for updating content.
        /// </summary>
        public static bool WifiOnly
        {
            get => AppSettings.GetValueOrDefault(WifiOnlyKey, false);
            set => AppSettings.AddOrUpdateValue(WifiOnlyKey, value);
        }


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
            return AppSettings.GetValueOrDefaultExceptionSafe<int>(LastLocationKey);
        }

        public static void SetHtmlRawView(bool valueToSet)
        {
            AppSettings.AddOrUpdateValue(HtmlRawView, valueToSet);
        }

        /// <summary>
        /// if this setting is true the html content should be displayed in raw view
        /// </summary>
        /// <returns>true displayes raw view, false html content</returns>
        public static bool GetHtmlRawViewSetting() => AppSettings.GetValueOrDefaultExceptionSafe<bool>(HtmlRawView);
        public static string Language(int locationId)
        {
            return AppSettings.GetValueOrDefaultExceptionSafe<string>(MakeLocationKey(locationId));
        }
        public static string Language(Location location)
        {
            return null == location ? null : AppSettings.GetValueOrDefaultExceptionSafe<string>(MakeLocationKey(location));
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
            return AppSettings.GetValueOrDefaultExceptionSafe<DateTime>(LastLocationUpdate);
        }

        public static void SetLastLocationUpdateTime(DateTime to)
        {
            AppSettings.AddOrUpdateValue(LastLocationUpdate, to);
        }

        public static DateTime LastLanguageUpdateTime(Location location)
        {
            return AppSettings.GetValueOrDefaultExceptionSafe<DateTime>(MakeLocationUpdateKey(location));
        }

        public static void SetLastLanguageUpdateTime(Location location, DateTime to)
        {
            AppSettings.AddOrUpdateValue(MakeLocationUpdateKey(location), to.Ticks);
        }

        public static DateTime LastPageUpdateTime<T>(Language language, Location location)
        {
            return AppSettings.GetValueOrDefaultExceptionSafe<DateTime>(MakePageKey<T>(language, location));
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

        public static void ClearAll()
        {
            SetLocation(new Location {Id = -1});
            SetLanguage(-1, new Language {Id = -1});
            SetHtmlRawView(false);
        }
    }
}
