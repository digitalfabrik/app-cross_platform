using System;

namespace Integreat.Shared.Data.Services
{
    /// <summary>
    /// Describes a property, which can be persisted in a file.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class JsonPersistAttribute : Attribute
    {
        public readonly string Name;
        public readonly string Id;

        public JsonPersistAttribute(string name, string id)
        {
            Name = name;
            Id = id;
        }
    }
}
