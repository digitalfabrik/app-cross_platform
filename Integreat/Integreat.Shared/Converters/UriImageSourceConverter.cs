using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Integreat.Shared.Converters
{
	public class UriImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is string) || "".Equals(value)) return null;

            var image = (string)value;

            return new UriImageSource
            {
                Uri = new Uri(image),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(1, 0, 0, 0)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
