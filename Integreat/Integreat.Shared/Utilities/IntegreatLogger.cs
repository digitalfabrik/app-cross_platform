using System;
using System.Collections;
using System.Diagnostics;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

[assembly:Dependency(typeof(IntegreatLogger))]
namespace Integreat.Shared.Utilities
{
    public class IntegreatLogger : ILogger
    {
        public void TrackPage(string page, string id = null)
            => Debug.WriteLine($"{nameof(TrackPage)}: Page-{page} ---Id-{id}");

        public void Track(string trackIdentifier)
            => Debug.WriteLine($"{nameof(Track)}: TrackIdentifier-{trackIdentifier}");

        public void Track(string trackIdentifier, string key, string value)
            => Debug.WriteLine($"{nameof(Track)}: TrackIdentifier-{trackIdentifier}; key: {key}; value {value}");

        public void Report(Exception exception = null, Severity warningLevel = Severity.Warning)
            => Debug.WriteLine($"{nameof(Report)} ToDO");

        public void Report(Exception exception, IDictionary extraData, Severity warningLevel = Severity.Warning)
            => Debug.WriteLine($"{nameof(Report)} ToDO");

        public void Report(Exception exception, string key, string value, Severity warningLevel = Severity.Warning)
            => Debug.WriteLine($"{nameof(Report)} ToDO");
    }
}
