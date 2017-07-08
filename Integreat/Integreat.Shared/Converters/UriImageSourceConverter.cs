using System;
using System.Diagnostics;
using System.Security;
using Xamarin.Forms;

namespace Integreat.Shared.Converters
{
	public class UriImageSourceConverter : IValueConverter
	{
		[SecurityCritical]
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (!(value is string) || "".Equals(value)) return null;

			var image = (string)value;

			try
			{
				return new UriImageSource
				{
					Uri = new Uri(image),
					CachingEnabled = true,
					CacheValidity = new TimeSpan(1, 0, 0, 0)
				};
			}
			catch (Exception e)
			{
                Debug.WriteLine(e);
				return image;
			}
		}
		[SecurityCritical]
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
	}
}
