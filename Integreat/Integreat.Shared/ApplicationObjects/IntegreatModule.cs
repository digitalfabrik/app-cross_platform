using Autofac;
using Integreat.Shared.ViewModels;
using System;
using System.Net.Http;
using System.Security;
using Integreat.Shared.Factories;
using Integreat.Shared.Factories.Loader;
using Integreat.Shared.Factories.Loader.Targets;
using Integreat.Shared.Pages;
using Integreat.Shared.Pages.General;
using Integreat.Shared.Pages.Main;
using Integreat.Shared.Pages.Settings;
using Integreat.Shared.ViewModels.Events;
using Integreat.Shared.ViewModels.General;
using Integreat.Shared.ViewModels.Settings;
using Integreat.Utilities;
using ModernHttpClient;
using Newtonsoft.Json;
using Refit;
using Xamarin.Forms;
using Debug = System.Diagnostics.Debug;
using Page = Xamarin.Forms.Page;
using Integreat.Shared.Factories.Services;

namespace Integreat.Shared.Factories
{
    /// <summary>
    /// In the Integreat module we fill the IoC container and create necessary services 
    /// </summary>
    [SecurityCritical]
    public class IntegreatModule : Module
    {
        [SecurityCritical]
        public IntegreatModule()
        {
        }

        [SecurityCritical]
        protected override void Load(ContainerBuilder builder)
        {
            //
            // VIEW MODELS
            // 

            builder.RegisterType<PageViewModel>();
            builder.RegisterType<EventPageViewModel>();


            builder.RegisterType<LocationsViewModel>();
            builder.RegisterType<LanguagesViewModel>(); // can have multiple instances


            builder.RegisterType<SearchViewModel>();
            // redesign
            builder.RegisterType<ContentContainerViewModel>();
            builder.RegisterType<MainContentPageViewModel>();
            builder.RegisterType<ExtrasContentPageViewModel>();
            builder.RegisterType<EventsContentPageViewModel>();

            // main
            builder.RegisterType<MainTwoLevelViewModel>();

            // general
            builder.RegisterType<GeneralWebViewPageViewModel>();
            builder.RegisterType<PdfWebViewPageViewModel>();
            builder.RegisterType<ImagePageViewModel>();

            // settings
            builder.RegisterType<SettingsPageViewModel>();

            //
            // PAGES
            //

            // register views
            builder.RegisterType<LanguagesPage>();
            builder.RegisterType<LocationsPage>();
            builder.RegisterType<SearchListPage>();
            // redesign
            builder.RegisterType<ContentContainerPage>();
            builder.RegisterType<MainContentPage>();
            builder.RegisterType<ExtrasContentPage>();
            builder.RegisterType<EventsContentPage>();

            // main
            builder.RegisterType<MainTwoLevelPage>();

            // events
            builder.RegisterType<EventsSingleItemDetailViewModel>();

            // extras
            builder.RegisterType<JobOffersPage>();
            builder.RegisterType<Careers4RefugeesViewModel>();
            builder.RegisterType<SprungbrettViewModel>();

            // general
            builder.RegisterType<GeneralWebViewPage>();
            builder.RegisterType<PdfWebViewPage>();
            builder.RegisterType<ImageViewPage>();

            // settings
            builder.RegisterType<SettingsPage>();

            // current page resolver
            builder.RegisterInstance<Func<Page>>(Instance);

            builder.RegisterInstance(DataLoadServiceFactory.Create());
            builder.RegisterType<DataLoaderProvider>();
            builder.RegisterType<LocationsDataLoader>();
            builder.RegisterType<LanguagesDataLoader>();
            builder.RegisterType<DisclaimerDataLoader>();
            builder.RegisterType<EventPagesDataLoader>();
            builder.RegisterType<PagesDataLoader>();
        }

        [SecurityCritical]
        private static Page Instance() => Application.Current.MainPage;
    }
}
