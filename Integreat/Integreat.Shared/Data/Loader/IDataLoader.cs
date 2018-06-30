using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Integreat.Shared.Models;

namespace Integreat.Shared.Data.Loader
{
    /// <summary>
    /// Dataloader interface.
    /// </summary>
    public interface IDataLoader
    {
        /// <summary> Gets the name of the file. </summary>
        /// <value> The name of the file. </value>
        string FileName { get; }
        /// <summary> Gets or sets the last updated. </summary>
        /// <value> The last updated. </value>
        DateTime LastUpdated { get; set; }

        /// <summary> Gets the identifier. </summary>
        /// <value> The identifier. </value>
        string Id { get; }
    }

    public interface IDataLoader<T> : IDataLoader
    {
        /// <summary> Gets the current cached pages async. </summary>
        /// <returns>The cached pages. Null if there are none.</returns>
        Task<Collection<T>> GetCachedFilesAsync();
    }
}
