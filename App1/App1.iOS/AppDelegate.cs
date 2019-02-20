using App1.Utilities;
using Autofac;
using Foundation;
using System;
using System.IO;
using App1.ViewModels;
using UIKit;

namespace App1.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();

            var backgroundColor = new UIColor(249, 249, 249, 0);
            UINavigationBar.Appearance.BarTintColor = backgroundColor;
            var cb = new ContainerBuilder();
            SetVersion();
            SetCacheFilePath();
            SetDatabasePath();
            SetMainPageFunc();
            LoadApplication(new IntegreatApp(cb));

            FirebasePushNotificationManager.Initialize();

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
        private static void SetMainPageFunc()
        {
            PlatformHelper.GetInstance().MainPageFunc =
                (viewFactory) => viewFactory.Resolve<ContentContainerViewModel>();
        }

        private static void SetDatabasePath()
        {
            PlatformHelper.GetInstance().DatabasePath = GetPath("_v2_");
        }

        private static void SetCacheFilePath()
        {
            PlatformHelper.GetInstance().CachedFilePath = GetPath("");
        }

        private static string GetPath(string suffix)
        {
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
            // (they don't want non-user-generated data in Documents)
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            return Path.Combine(libraryPath, suffix);
        }


        private static void SetVersion()
        {
            var helper = PlatformHelper.GetInstance();
            helper.Version = Foundation.NSBundle.MainBundle.
                InfoDictionary[new Foundation.NSString("CFBundleVersion")].ToString();
        }
    }
}
