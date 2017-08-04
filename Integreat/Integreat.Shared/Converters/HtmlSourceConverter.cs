using System;
using System.Globalization;
using System.Security;
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
            var param = parameter as Label;
            if (str.StartsWith("http")) return str;
            if (param != null)
            {
                if (param.Text.ToLower() == "true")
                {
                    str = ReplaceHtmlTagsInString(str);
                }
            }
            var html = new HtmlWebViewSource { Html = str };

            return html;
        }
        [SecurityCritical]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static string ReplaceHtmlTagsInString(string str)
        {
            var htmlString = str;
            while (htmlString.Contains("<") || htmlString.Contains(">"))
            {
                htmlString = htmlString.Replace("<", "&lt").Replace("> ", "&gt").Replace(">", "&gt");
            }
            return htmlString;
        }
    }
}
