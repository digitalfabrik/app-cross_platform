using System;
namespace Integreat.Shared.Data.Sender
{
    /// <summary>
    /// DataSender interface
    /// </summary>
    public interface IDataSender
    {
        /// <summary> Gets the name of the file. </summary>
        /// <value> The name of the file. </value>
        string FileName { get; }
    }
}
