using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;
using Google.Analytics;

[assembly: Dependency(typeof(IAnalyticsService))]
namespace Integreat.iOS
{
    internal class AnalyticsService : IAnalyticsService
    {
        public string TrackingId = "XX-XXXXXXXX-X";

        private static ITracker _tracker;
        private static AnalyticsService _instance;

        private AnalyticsService()
        {
            
        }

        public static AnalyticsService GetInstance()
        {
            return _instance ?? (_instance = new AnalyticsService());
        }

		public void Initialize()
        {
            _instance = GetInstance();
			Gai.SharedInstance.DispatchInterval = 10;
			Gai.SharedInstance.TrackUncaughtExceptions = true;

			//Enable for debugging:
			//Gai.SharedInstance.DryRun = true; //don't send stuff to server
			//Gai.SharedInstance.Logger.SetLogLevel(LogLevel.Verbose);

			_tracker = Gai.SharedInstance.GetTracker(TrackingId);
			_tracker.SetAllowIdfaCollection(true);
        }

        public void TrackPage(string pageName)
        {
			_tracker.Set("kGAIScreenName", pageName);
			_tracker.Send(DictionaryBuilder.CreateScreenView().Build());
        }

        public void TrackEvent(string category, string eventName, string label)
        {
			var builder = DictionaryBuilder.CreateEvent(category, eventName, "label", null);
			_tracker.Send(builder.Build());
        }

        public void TrackException(string exception, bool isFatal)
        {
			var builder = DictionaryBuilder.CreateException(exception, isFatal);

            _tracker.Send(builder.Build());
        }
    }
}