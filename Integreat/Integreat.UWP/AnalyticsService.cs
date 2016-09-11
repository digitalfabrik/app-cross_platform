
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

[assembly: Dependency(typeof(IAnalyticsService))]
namespace Integreat.UWP
{
    internal class AnalyticsService : IAnalyticsService
    {
        public string TrackingId = "XX-XXXXXXXX-X";
        
        private static AnalyticsService _instance;

        private AnalyticsService()
        {
            
        }

        public static AnalyticsService GetInstance()
        {
            return _instance ?? (_instance = new AnalyticsService());
        }

        public void Initialize(MainPage mainPage)
        {
            _instance = GetInstance();
        }

        public void TrackPage(string pageName)
        {
        }

        public void TrackEvent(string category, string eventName, string label)
        {
        }

        public void TrackException(string exception, bool isFatal)
        {
        }
    }
}