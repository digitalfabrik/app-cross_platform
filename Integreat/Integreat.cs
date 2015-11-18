using System;

using Xamarin.Forms;

namespace Integreat
{
    public class App : Application
    {

        public static Rectangle ScreenBounds;
        public static Rectangle ContentBounds;

        public App()
        {
            
            // The root page of your application
            MainPage = new overview();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

