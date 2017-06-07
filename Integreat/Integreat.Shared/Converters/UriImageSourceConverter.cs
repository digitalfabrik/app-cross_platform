using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

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
			catch (Exception)
			{
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
