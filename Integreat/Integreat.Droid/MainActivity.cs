
using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Autofac;
using Integreat.Shared;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Integreat.Droid
{
	[Activity(Label = "Integreat", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);


			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

			Forms.Init(this, bundle);

		    DisplayCrashReport();
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

	    [SecurityCritical]
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
				var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
				DateTime.Now, exception.ToString());
				File.WriteAllText(errorFilePath, errorMessage);

				// Log to Android Device Logging.
				Android.Util.Log.Error("Crash Report", errorMessage);
			}
			catch
			{
				// just suppress any error logging exceptions
			}
		}

		/// <summary>
		// If there is an unhandled exception, the exception information is diplayed 
		// on screen the next time the app is started (only in debug configuration)
		/// </summary>
		private bool DisplayCrashReport()
		{
			const string errorFilename = "Fatal.log";
			var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			var errorFilePath = Path.Combine(libraryPath, errorFilename);

			if (!File.Exists(errorFilePath))
			{
				return false;
			}

			var errorText = File.ReadAllText(errorFilePath);
			new AlertDialog.Builder(this)
				.SetPositiveButton("Clear", (sender, args) =>
				{
					File.Delete(errorFilePath);
					ContinueApplicationStartup();
				})
				.SetNegativeButton("Close", (sender, args) =>
				{
					ContinueApplicationStartup();
		})
				.SetMessage(errorText)
				.SetTitle("Crash Report")
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

