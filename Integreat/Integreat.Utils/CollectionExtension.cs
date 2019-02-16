using System.Collections.ObjectModel;
using System.Linq;

namespace Integreat.Shared.Utilities
{
    public static class CollectionExtension
    {
        /// <summary>
        /// Merges the specified collection b into collection a with a property that identifies each item uniquely.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionA">The collection a.</param>
        /// <param name="collectionB">The collection b.</param>
        /// <param name="idProperty">The identifier property.</param>
        public static void Merge<T>(this Collection<T> collectionA, Collection<T> collectionB, string idProperty)
        {
            foreach (var item in collectionB)
            {
                var id = GetPropValue(item, idProperty).ToString();
                var existingItem = collectionA.FirstOrDefault(x => GetPropValue(x, idProperty).ToString() == id);
                if (existingItem != null) collectionA.Remove(existingItem);
                collectionA.Add(item);
            }
        }

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src, null);
        }
    }
}
