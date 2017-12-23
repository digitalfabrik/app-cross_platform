using System;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Integreat.Shared.DTO.Utilities
{
    /// <summary>
    /// ExtensionMethods for utitlity classes
    /// </summary>
    public static class Extensions
    {
        private static readonly IFormatProvider Culture = CultureInfo.InvariantCulture;

        /// <summary>Convert to a rest acceptable datetime string. </summary>
        /// <param name="dateTime">The dateTime.</param>
        /// <returns></returns>
        public static string ToRestAcceptableString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", Culture);
        }

        /// <summary> Convert the datetime from rest string. </summary>
        /// <param name="dateTimeString">The string.</param>
        /// <returns></returns>
        public static DateTime DateTimeFromRestString(this string dateTimeString)
        {
            if (DateTime.TryParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", Culture,
                DateTimeStyles.AssumeLocal, out var date))
            {
                return date;
            }
            if (DateTime.TryParseExact(dateTimeString, "yyyy-MM-dd'T'HH:mm:ssz", Culture,
                DateTimeStyles.AssumeLocal, out date))
            {
                return date;
            }
            return DateTime.TryParse(dateTimeString, out date) ? date : DateTime.Now;
        }

        /// <summary> Determines whether this instance is true. </summary>
        /// <param name="val">The value.</param>
        /// <returns>   <c>true</c> if the specified value is true; otherwise, <c>false</c>. </returns>
        public static bool IsTrue(this string val)
        {
            // ReSharper disable once InlineOutVariableDeclaration
            if (int.TryParse(val, out var intVal))
            {
                return intVal == 1;
            }
            return "true".Equals(val.ToLower());
        }

        /// <summary> URLs the or empty string. </summary>
        /// <param name="property">The property to find in the json.</param>
        /// <returns>The url or an empty string.</returns>
        public static string UrlOrEmptyString(this string property)
        {
            try
            {
                var url = (string)JObject.Parse(property)["url"];
                url = url.Replace("https:/", "http:/"); //fix problem with https strings
                return url;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary> Determines whether [is enabled safe] [the specified property]. </summary>
        /// <param name="property">The property to find in json.</param>
        /// <returns>"1" if is enabled, else "0"</returns>
        public static string IsEnabledSafe(this string property)
        {
            try
            {
                var isEnabled = (string)JObject.Parse(property)["enabled"];
                return isEnabled;
            }
            catch (Exception)
            {
                return "0";
            }
        }
    }
}
