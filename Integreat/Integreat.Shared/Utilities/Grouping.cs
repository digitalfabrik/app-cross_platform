using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Integreat.Shared.Utilities
{
    /// <summary>
    /// Grouping class used to group items in a listView with a given key K, and a resulting group T.
    /// </summary>
    /// <typeparam name="K">Type of the grouping key.</typeparam>
    /// <typeparam name="T">Group of Type t for a key.</typeparam>
    public class Grouping<K, T> : ObservableCollection<T> {
        /// <summary>
        /// Gets the key for the group.
        /// </summary>
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items) {
            Key = key;
            foreach (var item in items) {
                Items.Add(item);
            }
        }
    }
}
