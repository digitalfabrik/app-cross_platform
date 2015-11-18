using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using ScnViewGestures.Plugin.Forms.iOS.Renderers;

namespace Integreat.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            App.ScreenBounds = new Xamarin.Forms.Rectangle(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
            App.ContentBounds = new Xamarin.Forms.Rectangle(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height - 64);
            global::Xamarin.Forms.Forms.Init();
            ViewGesturesRenderer.Init();
            // Code for starting up the Xamarin Test Cloud Agent
            #if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
            #endif

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}

