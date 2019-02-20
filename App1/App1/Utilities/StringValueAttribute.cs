using System;

namespace App1.Utilities
{
    
    /// <inheritdoc />
    /// <summary>
    /// new Attribute for Enums
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        public StringValueAttribute(string value)
        {
            StringValue = value;
        }

        /// <summary> Gets the string value. </summary>
        /// <value> The string value. </value>
        public string StringValue { get; }
    }
}
