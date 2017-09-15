using System;
using System.Globalization;
using System.Security;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Integreat.Shared.Converters {
    /// <summary>
    /// Converts a HTML text into plain text. (Ignoring all tags)
    /// </summary>
    public class HtmlToPlainConverter : IValueConverter {
        [SecurityCritical]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? null : GetContent(value.ToString());
        }
        [SecurityCritical]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes every tag from the given string and replaces certain tags with special characters.
        /// </summary>
        /// <param name="forHtml">The target HTML text.</param>
        /// <returns>String containing the HTML without any tags.</returns>
        private string GetContent(string forHtml) {
            var str = forHtml;
            // replace list starts, list elements and HTML newlines with newlines characters
            str = Regex.Replace(str, "<ul>", "\r\n");
            str = Regex.Replace(str, "<br>", "\r\n");
            str = Regex.Replace(str, "<li>", "\r\n");

            str = Regex.Replace(str, "<p><\\/p>", "\r\n"); // replace empty paragraphs with newlines

            str = Regex.Replace(str, "<\\/[^>]*>", " "); // replace closing tags with spaces

            str = Regex.Replace(str, "&nbsp;", " "); // replace non breaking spaces
            return Regex.Replace(str, "<[\\/]*[^>]*>", ""); // replace any tag with an empty string
        }
    }
}
