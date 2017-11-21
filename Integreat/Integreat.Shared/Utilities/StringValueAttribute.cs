using System;
using System.Reflection;

namespace Integreat.Shared.Utilities
{
    //new Attribute for Enums
    public class StringValueAttribute:Attribute
    {
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        public string StringValue { get; protected set; }

    }
}
