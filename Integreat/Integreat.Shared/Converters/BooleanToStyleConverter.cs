using System;
using System.Globalization;
using Xamarin.Forms;

namespace Integreat.Shared.Converters
{
    /// <summary>
    /// converts a boolean to an style
    /// </summary>
    public class BooleanToStyleConverter
    {
        public Style FalseStyle { get; set; }
        public Style TrueStyle { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            var newState = (bool)value;
            return newState ? TrueStyle : FalseStyle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
