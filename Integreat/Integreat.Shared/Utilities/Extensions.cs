using System;
using System.Collections.Generic;

namespace Integreat
{
	public static class Extensions
	{
		static readonly IFormatProvider Culture = new System.Globalization.CultureInfo("de-DE");

		public static string ToRestAcceptableString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd'T'HH:mm:ssz", Culture);
        }

	    public static DateTime DateTimeFromRestString(this string str)
	    {
	        DateTime date;
	        if (DateTime.TryParseExact(str, "yyyy-MM-dd HH:mm:ss", Culture, System.Globalization.DateTimeStyles.AssumeLocal, out date))
	        {
                return date;
            }
            if (DateTime.TryParseExact(str, "yyyy-MM-dd'T'HH:mm:ssz", Culture, System.Globalization.DateTimeStyles.AssumeLocal, out date))
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
            return "true".Equals(val);
        }
    }
}

