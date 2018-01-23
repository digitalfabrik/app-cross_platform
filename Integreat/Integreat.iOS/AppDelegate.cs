using System;
using System.Globalization;
using Foundation;
using Integreat.Shared;
using UIKit;
using Autofac;
using Xamarin.Forms;
// ReSharper disable All

namespace Integreat.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    /// <inheritdoc />
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
            LoadApplication(new IntegreatApp(cb));

            FirebasePushNotificationManager.Current.Initialize();

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            FirebasePushNotificationManager.Connect();
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            FirebasePushNotificationManager.Disconnect();
        }
    }
}
