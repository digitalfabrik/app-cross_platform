using System;
using System.Diagnostics;
using Newtonsoft.Json.Serialization;
using TraceLevel = Newtonsoft.Json.TraceLevel;

namespace Integreat.Shared.Debugging
{
    public class ConsoleTraceWriter : ITraceWriter
    {
        public void Trace(TraceLevel level, string message, Exception ex) {
            Debug.Write(level.ToString() + ":" + message);
        }

        public TraceLevel LevelFilter => TraceLevel.Verbose;
    }
}
