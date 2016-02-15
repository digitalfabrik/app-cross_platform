using Android.Content;
using Android.Gms.Analytics;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

[assembly: Dependency(typeof(IAnalyticsService))]
namespace Integreat.Droid
{
    internal class AnalyticsService : IAnalyticsService
    {
        public string TrackingId = "XX-XXXXXXXX-X";

        private static GoogleAnalytics _googleAnalytics;
        private static Tracker _tracker;
        private static AnalyticsService _instance;

        private AnalyticsService()
        {
            
        }

        public static AnalyticsService GetInstance()
        {
            return _instance ?? (_instance = new AnalyticsService());
        }

        public void Initialize(Context context)
        {
            _instance = GetInstance();
            _googleAnalytics = GoogleAnalytics.GetInstance(context.ApplicationContext);
            _googleAnalytics.SetLocalDispatchPeriod(10);

            _tracker = _googleAnalytics.NewTracker(TrackingId);
            _tracker.EnableExceptionReporting(true);
            _tracker.EnableAdvertisingIdCollection(true);
            _tracker.EnableAutoActivityTracking(true);
        }

        public void TrackPage(string pageName)
        {
            _tracker.SetScreenName(pageName);
            _tracker.Send(new HitBuilders.ScreenViewBuilder().Build());
        }

        public void TrackEvent(string category, string eventName, string label)
        {
            var builder = new HitBuilders.EventBuilder();
            builder.SetCategory(category);
            builder.SetAction(eventName);
            builder.SetLabel("label");

            _tracker.Send(builder.Build());
        }

        public void TrackException(string exception, bool isFatal)
        {
            var builder = new HitBuilders.ExceptionBuilder();
            builder.SetDescription(exception);
            builder.SetFatal(isFatal);

            _tracker.Send(builder.Build());
        }
    }
}