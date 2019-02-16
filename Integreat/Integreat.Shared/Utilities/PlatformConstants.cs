
namespace Integreat.Shared.Utilities
{
    //based on https://github.com/xamarin/mobile-samples/blob/master/Tasky/TaskySharedCode/TodoItemRepositoryADO.cs
    public static class PlatformConstants
    {
        public static string CachedFilePath => Helpers.Platform.GetCachedFilePath();

        public static string DatabaseFilePath => Helpers.Platform.GetDatabasePath();
    }
}
