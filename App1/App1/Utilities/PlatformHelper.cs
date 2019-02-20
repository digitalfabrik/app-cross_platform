using System;
using System.Collections.Generic;
using System.Text;
using App1.Models;
using App1.ViewFactory;
using Page = Xamarin.Forms.Page;

namespace App1.Utilities
{
    public class PlatformHelper
    {
        private static PlatformHelper _instance;

        private PlatformHelper()
        {
            // singleton
        }

        public static PlatformHelper GetInstance()
            => _instance ?? (_instance = new PlatformHelper());

        public string Version { get; set; } = Constants.CurrentAppVersion;

        public string CachedFilePath { get; set; }

        public string DatabasePath { get; set; }

        public Func<IViewFactory, Page> MainPageFunc{ get; set; }
    }
}
