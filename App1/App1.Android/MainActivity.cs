using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using App1.Droid.Firebase;
using App1.Droid.Utilities;
using App1.Utilities;
using App1.ViewModels;
using Autofac;
using Integreat.Localization;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Environment = System.Environment;

namespace App1.Droid
{
    [Activity(Theme = "@style/MyTheme", Name = "tuerantuer.app.integreat.MainActivity", Label = "Integreat", Icon = "@mipmap/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetToolbarResources();

            base.OnCreate(savedInstanceState);

            Globals.Window = Window;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            Forms.Init(this, savedInstanceState);
            DisplayCrashReport();
            ContinueApplicationStartup();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }

        private static void SetToolbarResources()
        {
            ToolbarResource = Resource.Layout.Toolbar;
            TabLayoutResource = Resource.Layout.Tabbar;
        }

        private void ContinueApplicationStartup()
        {
            var cb = new ContainerBuilder();
            SetVersion();
            SetCacheFilePath();
            SetDatabasePath();
            SetMainPageFunc();
            var app = new IntegreatApp(cb);

            LoadApplication(app); // if a exception occurs here, try to delete bin and obj folder and re-build
            CrossCurrentActivity.Current.Activity = this;

            FirebasePushNotificationManager.ProcessIntent(this, Intent);

            if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;
            // Create channel to show notifications.
            const string channelId = "PushNotificationChannel";
            const string channelName = "General";
            var notificationManager = (NotificationManager)BaseContext.GetSystemService(NotificationService);

            notificationManager.CreateNotificationChannel(new NotificationChannel(channelId,
                channelName, NotificationImportance.Default));
        }

        private static void SetMainPageFunc()
        {
            PlatformHelper.GetInstance().MainPageFunc = (viewFactory)
                => new NavigationPage(viewFactory.Resolve<ContentContainerViewModel>());
        }

        private static void SetCacheFilePath()
        {
            PlatformHelper.GetInstance().CachedFilePath = GetPath("");
        }

        private static void SetDatabasePath()
        {
            PlatformHelper.GetInstance().DatabasePath = GetPath("_v2_");
        }

        private static string GetPath(string suffix)
        {
            // Just use whatever directory SpecialFolder.Personal returns
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(libraryPath, suffix);
        }

        private static void SetVersion()
        {
            var platformHelper = PlatformHelper.GetInstance();
            var context = global::Android.App.Application.Context;
            var version = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            platformHelper.Version = version;
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                var errorFilePath = GetErrorFilePath();
                var errorMessage = $"Time: {DateTime.Now}\r\n{AppResources.ErrorGeneral}\r\n{exception}";
                File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error(AppResources.CrashReport, errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        private static string GetErrorFilePath()
        {
            const string errorFileName = "Fatal.log";
            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
            var errorFilePath = Path.Combine(libraryPath, errorFileName);
            return errorFilePath;
        }

        /// <summary>
        /// If there is an unhandled exception, the exception information is displayed
        /// on screen the next time the app is started (only in debug configuration)
        /// </summary>
        private void DisplayCrashReport()
        {
            try
            {
                var errorFilePath = GetErrorFilePath();
                if (CheckIfErrorFileIsNotPresent(errorFilePath))
                {
                    return; // no errors are present
                }

                ClearOldOrCorruptCacheIssues();
                CreateAndShowAlertDialog(errorFilePath);
            }
            catch (Exception)
            {
                // suppress all errors on crash reporting
            }
        }

        private static void ClearOldOrCorruptCacheIssues()
        {
            Cache.ClearCachedResources();
            Cache.ClearCachedContent();
        }

        private static bool CheckIfErrorFileIsNotPresent(string errorFilePath) => !File.Exists(errorFilePath);

        private void CreateAndShowAlertDialog(string errorFilePath)
        {
            var errorText = File.ReadAllText(errorFilePath);
            new AlertDialog.Builder(this)
                .SetPositiveButton(AppResources.Close, (sender, args) =>
                {
                    File.Delete(errorFilePath);
                    ContinueApplicationStartup();
                })
                .SetNegativeButton(AppResources.Copy, (sender, args) =>
                {
                    // try to copy contents of file to clipboard
                    try
                    {
                        var clipboardManager = (ClipboardManager)Android.App.Application.Context.GetSystemService(ClipboardService);
                        clipboardManager.PrimaryClip = ClipData.NewPlainText(AppResources.CrashReport, File.ReadAllText(errorFilePath));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    File.Delete(errorFilePath);
                    ContinueApplicationStartup();
                })
                .SetMessage(errorText)
                .SetTitle(AppResources.CrashReport)
                .Show();
        }

#pragma warning disable S125 // Sections of code should not be "commented out"

        //iOS: Different than Android. Must be in FinishedLaunching, not in Main.
        // In AppDelegate
        /*public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary options)
                {
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

                    TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            ...
        }*/

    }
#pragma warning restore S125 // Sections of code should not be "commented out"
}