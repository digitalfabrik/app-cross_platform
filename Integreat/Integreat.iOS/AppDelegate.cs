﻿using System;
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

            var backgroundColor = Color.FromRgb(249,249,249);
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(nfloat.Parse(backgroundColor.R.ToString(CultureInfo.InvariantCulture)), nfloat.Parse(backgroundColor.G.ToString(CultureInfo.InvariantCulture)), nfloat.Parse(backgroundColor.B.ToString(CultureInfo.InvariantCulture)));
            var cb = new ContainerBuilder();
            LoadApplication(new IntegreatApp(cb));

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
