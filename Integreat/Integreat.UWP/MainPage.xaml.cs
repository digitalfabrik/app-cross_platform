using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Autofac;
using Integreat.Shared;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Tracking;
using SQLite.Net.Platform.WinRT;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Integreat.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            var cb = new ContainerBuilder();
            cb.RegisterInstance(CreatePersistenceService()).SingleInstance();
            cb.RegisterInstance(CreateAnalytics());
            LoadApplication(new IntegreatApp(cb));
        }

        private IAnalyticsService CreateAnalytics()
        {
            var instance = AnalyticsService.GetInstance();
            instance.Initialize(this);
            return instance;
        }

        private PersistenceService CreatePersistenceService()
        {
            var persistence = new PersistenceService(new SQLitePlatformWinRT());
            persistence.Init();
            return persistence;
        }

    }
}
