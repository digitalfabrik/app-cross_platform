using System;
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

        /// <summary> Gets or sets the type of the connection. </summary>
        /// <value> The type of the connection. </value>
        public static ConnectionType ConnectionType
        {
            get => (ConnectionType)AppSettings.GetValueOrDefault(ConnectionTypeKey, (int)ConnectionType.Cellular);
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

        /// <summary>
        /// Removes the location.
        /// </summary>
        public static void RemoveLocation()
        {
            AppSettings.Remove(LastLocationKey);
        }

        /// <summary>
        /// Sets the location.
        /// </summary>
        /// <param name="location">The location.</param>
        public static void SetLocation(Location location)
        {
            AppSettings.AddOrUpdateValue(LastLocationKey, location.Id);
        }

        /// <summary>
        /// Locations this instance.
        /// </summary>
        /// <returns></returns>
        public static int Location()
        {
            return AppSettings.GetValueOrDefault(LastLocationKey, -1);
        }

        /// <summary>
        /// Sets the HTML raw view.
        /// </summary>
        /// <param name="valueToSet">if set to <c>true</c> [value to set].</param>
        public static void SetHtmlRawView(bool valueToSet)
        {
            AppSettings.AddOrUpdateValue(HtmlRawView, valueToSet);
        }

        /// <summary>
        /// if this setting is true the html content should be displayed in raw view
        /// </summary>
        /// <returns>true displayes raw view, false html content</returns>
        public static bool GetHtmlRawViewSetting() => AppSettings.GetValueOrDefault(HtmlRawView, false);
        public static string Language(int locationId)
        {
            return AppSettings.GetValueOrDefault(MakeLocationKey(locationId), string.Empty);
        }

        /// <summary> Languages the specified location. </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public static string Language(Location location)
        {
            return null == location ? null : AppSettings.GetValueOrDefault(MakeLocationKey(location),string.Empty);
        }

        /// <summary> Sets the language. </summary>
        /// <param name="location">The location.</param>
        /// <param name="language">The language.</param>
        public static void SetLanguage(Location location, Language language)
        {
            AppSettings.AddOrUpdateValue(MakeLocationKey(location), language.PrimaryKey);
        }

        /// <summary> Sets the language. </summary>
        /// <param name="locationId">The location identifier.</param>
        /// <param name="language">The language.</param>
        public static void SetLanguage(int locationId, Language language)
        {
            AppSettings.AddOrUpdateValue(MakeLocationKey(locationId), language.PrimaryKey);
        }

        /// <summary> Removes the language. </summary>
        /// <param name="location">The location.</param>
        public static void RemoveLanguage(Location location)
        {
            AppSettings.Remove(MakeLocationKey(location));
        }

        /// <summary> Removes the page. </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="language">The language.</param>
        /// <param name="location">The location.</param>
        public static void RemovePage<T>(Language language, Location location)
        {
            AppSettings.Remove(MakePageKey<T>(language, location));
        }

        /// <summary> Lasts the location update time. </summary>
        /// <returns></returns>
        public static DateTime LastLocationUpdateTime()
        {
            return AppSettings.GetValueOrDefault(LastLocationUpdate, default(DateTime));
        }

        /// <summary> Sets the last location update time. </summary>
        /// <param name="to">To.</param>
        public static void SetLastLocationUpdateTime(DateTime to)
        {
            AppSettings.AddOrUpdateValue(LastLocationUpdate, to);
        }

        /// <summary> Lasts the language update time. </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public static DateTime LastLanguageUpdateTime(Location location)
        {
            return AppSettings.GetValueOrDefault(MakeLocationUpdateKey(location), default(DateTime));
        }

        /// <summary> Sets the last language update time. </summary>
        /// <param name="location">The location.</param>
        /// <param name="to">To.</param>
        public static void SetLastLanguageUpdateTime(Location location, DateTime to)
        {
            AppSettings.AddOrUpdateValue(MakeLocationUpdateKey(location), to.Ticks);
        }

        /// <summary> Lasts the page update time. </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="language">The language.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public static DateTime LastPageUpdateTime<T>(Language language, Location location)
        {
            return AppSettings.GetValueOrDefault(MakePageKey<T>(language, location), default(DateTime));
        }

        /// <summary> Sets the last page update time. </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="language">The language.</param>
        /// <param name="location">The location.</param>
        /// <param name="to">To.</param>
        public static void SetLastPageUpdateTime<T>(Language language, Location location, DateTime to)
        {
            AppSettings.AddOrUpdateValue(MakePageKey<T>(language, location), to);
        }

        /// <summary> Makes the location update key. </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        private static string MakeLocationUpdateKey(Location location)
        {
            return MakeLocationKey(location) + "_update";
        }

        /// <summary> Makes the location key. </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        private static string MakeLocationKey(Location location)
        {
            return $"location_key({location.Id})";
        }

        /// <summary> Makes the location key. </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        private static string MakeLocationKey(int location)
        {
            return $"location_key({location})";
        }

        /// <summary> Makes the page key. </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="language">The language.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        private static string MakePageKey<T>(Language language, Location location)
        {
            return $"{typeof(T).FullName}({language.Id})({location.Id})_update";
        }

        /// <summary> Clears all. </summary>
        public static void ClearAll()
        {
            SetLocation(new Location {Id = -1});
            SetLanguage(-1, new Language {Id = -1});
            SetHtmlRawView(false);
        }
    }
}
