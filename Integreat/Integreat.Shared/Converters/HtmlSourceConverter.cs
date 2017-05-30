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
            var html = new HtmlWebViewSource();

            if (value != null)
            {
                html.Html = value.ToString();
            }

            return html;
        }
        [SecurityCritical]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
