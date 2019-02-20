namespace App1.Droid.Utilities
{
    public class VersionHelper
    {
        public string GetPlatformVersion()
        {
            var context = global::Android.App.Application.Context;
            return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
        }
    }
}