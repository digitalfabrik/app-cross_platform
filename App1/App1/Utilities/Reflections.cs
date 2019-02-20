using System;
using System.Reflection;

namespace App1.Utilities
{
    public static class Reflections
    {
        public static bool IsAssignableFrom(Type first, Type second)
        {
            return first.GetTypeInfo().IsAssignableFrom(second.GetTypeInfo());
        }

    }
}
