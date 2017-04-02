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
}
