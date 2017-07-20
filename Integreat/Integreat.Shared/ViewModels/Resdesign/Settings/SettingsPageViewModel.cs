using Integreat.Shared.Services.Tracking;
using localization;

namespace Integreat.Shared.ViewModels.Resdesign.Settings
{
    public class SettingsPageViewModel : BaseViewModel
    {
        public SettingsPageViewModel(IAnalyticsService analyticsService) : base(analyticsService)
        {
            Title = AppResources.Settings;
        }
    }
}
