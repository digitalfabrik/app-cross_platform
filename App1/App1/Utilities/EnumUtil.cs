using System;

namespace App1.Utilities
{
    /// <summary>
    /// Expanding the Enum class, to get the Stringvalue
    /// </summary>
    public static class EnumUtil
    {
        /// <summary> Gets the string value. </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            var type = value.GetType();

            // Get fieldinfo for this type
            var fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes

            // Return the first if there was a match.
            return fieldInfo.GetCustomAttributes(
                       typeof(StringValueAttribute), false) is StringValueAttribute[] attribs 
                       && attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }
}