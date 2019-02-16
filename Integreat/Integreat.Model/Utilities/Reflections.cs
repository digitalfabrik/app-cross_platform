using System;
using System.Reflection;

namespace Integreat.Model.Utilities
{
    public static class Reflections
    {
        public static bool IsAssignableFrom(Type first, Type second)
            => first.GetTypeInfo().IsAssignableFrom(second.GetTypeInfo());
    }
}
