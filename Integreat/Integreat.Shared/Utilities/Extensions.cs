using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Integreat
{
    public static class Extensions
    {
        private static readonly IFormatProvider Culture = CultureInfo.InvariantCulture;

        public static string ToRestAcceptableString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", Culture);
        }

        public static DateTime DateTimeFromRestString(this string str)
        {
            DateTime date;
            if (DateTime.TryParseExact(str, "yyyy-MM-dd HH:mm:ss", Culture,
                System.Globalization.DateTimeStyles.AssumeLocal, out date))
            {
                return date;
            }
            if (DateTime.TryParseExact(str, "yyyy-MM-dd'T'HH:mm:ssz", Culture,
                System.Globalization.DateTimeStyles.AssumeLocal, out date))
            {
                return date;
            }
            return DateTime.TryParse(str, out date) ? date : DateTime.Now;
        }

        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            foreach (var element in source)
                target.Add(element);
        }

        public static bool IsTrue(this string val)
        {
            int intVal;
            if (int.TryParse(val, out intVal))
            {
                return intVal == 1;
            }
            return "true".Equals(val.ToLower());
        }

        // http://stackoverflow.com/questions/34197745/continuewith-and-taskcancellation-how-to-return-default-values-if-task-fails
        public static async Task<T> DefaultIfFaulted<T>(this Task<T> @this, T defaultValue = default(T))
        {
            // Await completion regardless of resulting Status (alternatively you can use try/catch).
            await @this
                .ContinueWith(_ => { }, TaskContinuationOptions.ExecuteSynchronously)
                .ConfigureAwait(false);

            if (@this.Status == TaskStatus.Faulted)
            {
                Debug.WriteLine(@this.Status + " " + @this.Exception.InnerException.Message);
                return defaultValue;
            }
            var result = await @this.ConfigureAwait(false);
            // if task ended successfully but still returned null, return a default value (if set)
            return result.IsDefault() ? defaultValue : result;
        }

        // http://stackoverflow.com/questions/65351/null-or-default-comparison-of-generic-argument-in-c-sharp
        public static bool IsDefault<T>(this T @this)
        {
            return EqualityComparer<T>.Default.Equals(@this, default(T));
        }

        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="enumerable">The enumerable, which may be null or empty.</param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            /* If this is a list, use the Count property for efficiency. 
             * The Count property is O(1) while IEnumerable.Count() is O(N). */
            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return !enumerable.Any();
        }

        /// <summary> Clamps the specified value. </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static T Clamp<T>(T value, T max, T min)
            where T : IComparable<T>
        {
            var result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }
    }
}