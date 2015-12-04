using System;
using System.Reflection;
using System.Text;

namespace Integreat.Shared.Utilities
{
    public static class Reflections
    {
        public static bool IsAssignableFrom(Type first, Type second)
        {
            return first.GetTypeInfo().IsAssignableFrom(second.GetTypeInfo());
        }

    }
}
