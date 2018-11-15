
namespace Integreat.Utilities
{
    //based on https://github.com/xamarin/mobile-samples/blob/master/Tasky/TaskySharedCode/TodoItemRepositoryADO.cs
    public static class Constants
    {

        public const string IntegreatReleaseUrl = "https://web.integreat-app.de";
        public const string IntegreatReleaseFallbackUrl = "https://cms.integreat-app.de";

        public const string MetaTagBuilderTag = "<!-- created with MetaTagBuilder -->";

        public const string IhkLehrstellenBoerseUrl = "https://www.ihk-lehrstellenboerse.de";

        public const string DataProtectionUrl = "https://integreat-app.de/datenschutz/";
        public const string RaumfreiUrl = "https://api.wohnen.integreat-app.de/v0/neuburgschrobenhausenwohnraum/offer";

        public const string CurrentAppVersion = "2.3.4";
        public static string CachedFilePath => Helpers.Platform.GetCachedFilePath();

        public static string DatabaseFilePath => Helpers.Platform.GetDatabasePath();
    }
}
