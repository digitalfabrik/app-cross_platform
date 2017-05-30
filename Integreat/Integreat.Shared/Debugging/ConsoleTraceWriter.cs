using System;
using System.Diagnostics;
using System.Security;
using Newtonsoft.Json.Serialization;
using TraceLevel = Newtonsoft.Json.TraceLevel;

namespace Integreat.Shared.Debugging
{
    Severity Code    Description Project File Line    Suppression State
    Message IDE0019 Use pattern matching Integreat.Droid, Integreat.iOS C:\Users\RK-003\Source\Repos\Integreat\Integreat\Integreat.Shared\Pages\BaseContentPage.xaml.cs 21	Active

    public class ConsoleTraceWriter : ITraceWriter
    {
        [SecurityCritical]
        public void Trace(TraceLevel level, string message, Exception ex) {
            Debug.Write(level + ":" + message);
        }

        public TraceLevel LevelFilter => TraceLevel.Verbose;
    }
}
