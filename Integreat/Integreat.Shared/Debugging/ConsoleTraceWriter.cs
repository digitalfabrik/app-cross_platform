using System;
using System.Diagnostics;
using System.Security;
using Newtonsoft.Json.Serialization;
using TraceLevel = Newtonsoft.Json.TraceLevel;

namespace Integreat.Shared.Debugging
{
    [SecurityCritical]
    public class ConsoleTraceWriter : ITraceWriter
    {
        [SecurityCritical]
        public void Trace(TraceLevel level, string message, Exception ex) {
            Debug.Write(level + ":" + message);
        }

        public TraceLevel LevelFilter => TraceLevel.Verbose;
    }
}
