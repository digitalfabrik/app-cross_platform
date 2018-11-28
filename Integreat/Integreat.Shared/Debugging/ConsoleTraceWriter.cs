using System;
using System.Diagnostics;
using System.Security;
using Newtonsoft.Json.Serialization;

namespace Integreat.Shared.Debugging
{
    public class ConsoleTraceWriter : ITraceWriter
    {
        [SecurityCritical]
        public void Trace(TraceLevel level, string message, Exception ex)
        {
            Debug.Write($"{level}:{message}\n{ex.StackTrace}");
        }

        public TraceLevel LevelFilter => TraceLevel.Verbose;
    }
}
