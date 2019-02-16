using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Integreat.Shared.Utilities
{
    /// <summary>
    /// Grouping class used to group items in a listView with a given key K, and a resulting group T.
    /// </summary>
    /// <typeparam name="TK">Type of the grouping key.</typeparam>
    /// <typeparam name="T">Group of Type t for a key.</typeparam>
    public class Grouping<TK, T> : ObservableCollection<T> {
        /// <summary>
        /// Gets the key for the group.
        /// </summary>
        public TK Key { get; private set; }

        public Grouping(TK key, IEnumerable<T> items) {
            Key = key;
            foreach (var item in items) {
                Items.Add(item);
            }
        }
    }
}
