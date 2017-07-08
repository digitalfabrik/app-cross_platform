using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Integreat.Shared.Converters
{
    public class HtmlSourceConverter : IValueConverter
    {
        [SecurityCritical]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // check if the value is a URL (starts with http), if so return it merely as string
            var str = value.ToString();
            if (str.StartsWith("http")) return str;

            var html = new HtmlWebViewSource {Html = str};

            return html;
        }
        [SecurityCritical]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
