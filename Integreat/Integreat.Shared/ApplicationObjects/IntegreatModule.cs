﻿using Autofac;
using Integreat.Shared.Data.Factories;
using Integreat.Shared.Pages;
using Integreat.Shared.Pages.General;
using Integreat.Shared.Pages.Main;
using Integreat.Shared.Pages.Search;
using Integreat.Shared.Pages.Settings;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using Integreat.Shared.ViewModels.Events;
using Integreat.Shared.ViewModels.General;
using Integreat.Shared.ViewModels.Search;
using Integreat.Shared.ViewModels.Settings;
using Newtonsoft.Json;
using Refit;
using System;
using System.Net.Http;
using System.Security;
using Integreat.Shared.Factories;
using Integreat.Shared.Factories.Loader;
using Integreat.Shared.Factories.Loader.Targets;
using Integreat.Shared.Factories.Services;
using Xamarin.Forms;
using Debug = System.Diagnostics.Debug;
using Page = Xamarin.Forms.Page;

namespace Integreat.Shared.ApplicationObjects
{
    /// <inheritdoc />
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
            builder.RegisterType<SprungbrettViewModel>();

            // general
            builder.RegisterType<GeneralWebViewPage>();
            builder.RegisterType<PdfWebViewPage>();
            builder.RegisterType<ImageViewPage>();

            // settings
            builder.RegisterType<SettingsPage>();

            // current page resolver
            builder.RegisterInstance<Func<Page>>(Instance);
            builder.RegisterInstance(CreateDataLoadService(HttpClientFactory.GetHttpClient()));
            builder.RegisterType<DataLoaderProvider>();
            builder.RegisterType<LocationsDataLoader>();
            builder.RegisterType<LanguagesDataLoader>();
            builder.RegisterType<DisclaimerDataLoader>();
            builder.RegisterType<EventPagesDataLoader>();
            builder.Register(_=>new BackgroundDownloader(HttpClientFactory.GetHttpClient())).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PagesDataLoader>();
            builder.Register(_ => new SprungbrettParser(HttpClientFactory.GetHttpClient())). AsImplementedInterfaces().SingleInstance();
        }

        [SecurityCritical]
        private static IDataLoadService CreateDataLoadService(HttpClient client)
        {
            var networkServiceSettings = new RefitSettings
            {
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Error = (sender, args) => Debug.WriteLine(args)
                    //, TraceWriter = new ConsoleTraceWriter() // debug tracer to see the json input
                }
            };
            return RestService.For<IDataLoadService>(client, networkServiceSettings);
        }
        [SecurityCritical]
        private static Page Instance() => Application.Current.MainPage;
    }
}
