using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Integreat.Shared.Data.Loader
{
    public interface IDataLoader
    {
        string FileName { get; }
        DateTime LastUpdated { get; set; }
        string Id { get; }
    }

    public interface IDataLoader<T> : IDataLoader
    {
        Task<T> Load(bool forceRefresh = false);
    }
}
