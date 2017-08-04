
using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Autofac;
using Integreat.Shared;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using localization;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Integreat.Droid
{

    [Activity(Label = "Integreat", Icon = "@mipmap/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            Forms.Init(this, bundle);

            try
            {
                DisplayCrashReport();
                
            }
            catch (Exception)
            {
                // supress all errors on crash reporting
            }
            ContinueApplicationStartup();
        }

        private void ContinueApplicationStartup()
        {

            ToolbarResource = Resource.Layout.toolbar;
            TabLayoutResource = Resource.Layout.tabs;

            var cb = new ContainerBuilder();
            cb.RegisterInstance(CreateAnalytics());
            LoadApplication(new IntegreatApp(cb));
        }


        private IAnalyticsService CreateAnalytics()
        {
            var instance = AnalyticsService.GetInstance();
            instance.Initialize(this);
            return instance;
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

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                const string errorFileName = "Fatal.log";
                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                var errorMessage = String.Format("Time: {0}\r\n{1}\r\n{2}",
                DateTime.Now, AppResources.ErrorGeneral, exception);
                File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error(AppResources.CrashReport, errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        // If there is an unhandled exception, the exception information is displayed 
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        private bool DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            // check if there is an error file present
            if (!File.Exists(errorFilePath))
            {
                // if not, no error happened
                return false;
            }

            // an error occurred last time the app was running. Clear cache to fix eventual corrupt cache issues
            Cache.ClearCachedResources();
            Cache.ClearCachedContent();

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
                        var clipboardmanager = (ClipboardManager)Forms.Context.GetSystemService(ClipboardService);
                        clipboardmanager.PrimaryClip = ClipData.NewPlainText(AppResources.CrashReport, File.ReadAllText(errorFilePath));
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

            return true;
        }


        //iOS: Different than Android. Must be in FinishedLaunching, not in Main.
        // In AppDelegate
        /*public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary options)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;  
    ...
}*/

    }
}

