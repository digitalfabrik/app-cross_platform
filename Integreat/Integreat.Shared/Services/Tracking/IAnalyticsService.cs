namespace Integreat.Shared.Services.Tracking
{
    public interface IAnalyticsService
    {
        void TrackPage(string pageName);
        void TrackEvent(string category, string eventName, string label);
        void TrackException(string exception, bool isFatal);
    }
}
