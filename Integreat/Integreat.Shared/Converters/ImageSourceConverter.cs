using System;
using System.Security;
using Xamarin.Forms;

namespace Integreat.Shared.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        [SecurityCritical]
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is string)) return null;

            var image = (string)value;
            ImageSource imageSource;
            imageSource = image.StartsWith("http") ? ImageSource.FromUri(new Uri(image)) : ImageSource.FromResource(image);
            return imageSource;
        }

        [SecurityCritical]
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
