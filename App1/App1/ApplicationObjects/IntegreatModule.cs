using System;
using System.Net.Http;
using System.Security;
using App1.Data.Factories;
using App1.Data.Loader;
using App1.Data.Loader.Targets;
using App1.Data.Services;
using App1.Pages;
using App1.Pages.Extras;
using App1.Pages.General;
using App1.Pages.Main;
using App1.Pages.Search;
using App1.Pages.Settings;
using App1.Utilities;
using App1.ViewModels;
using App1.ViewModels.Events;
using App1.ViewModels.Extras;
using App1.ViewModels.General;
using App1.ViewModels.Main;
using App1.ViewModels.Search;
using App1.ViewModels.Settings;
using Autofac;
using Newtonsoft.Json;
using Refit;
using Xamarin.Forms;
using Debug = System.Diagnostics.Debug;
using Page = Xamarin.Forms.Page;

namespace App1.ApplicationObjects
{
    /// <inheritdoc />
    /// <summary>
    /// In the Integreat module we fill the IoC container and create necessary services
    /// </summary>
    public class IntegreatModule : Module
    {
        [SecurityCritical]
        protected override void Load(ContainerBuilder builder)
        {
            RegisterViewModels(builder);
            RegisterMainPages(builder);
            RegisterNewsAndExtras(builder);
            RegisterGeneralPageTypes(builder);
            RegisterPageResolver(builder);
        }

        private static void RegisterPageResolver(ContainerBuilder builder)
        {
            builder.RegisterInstance<Func<Page>>(Instance);
            builder.RegisterInstance(CreateDataLoadService(HttpClientFactory.GetHttpClient(
                new Uri(Constants.IntegreatReleaseUrl))));
            builder.RegisterType<DataLoaderProvider>();
            builder.RegisterType<LocationsDataLoader>();
            builder.RegisterType<LanguagesDataLoader>();
            builder.RegisterType<DisclaimerDataLoader>();
            builder.RegisterType<EventPagesDataLoader>();
            builder.RegisterType<ExtrasDataLoader>();
            builder.Register(_ => new BackgroundDownloader(HttpClientFactory.GetHttpClient(
                new Uri(Constants.IntegreatReleaseUrl)))).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PagesDataLoader>();
            builder.Register(_ => new Parser(HttpClientFactory.GetHttpClient(new Uri(
                Constants.IntegreatReleaseUrl)))).AsImplementedInterfaces().SingleInstance();
        }

        private static void RegisterGeneralPageTypes(ContainerBuilder builder)
        {
            builder.RegisterType<GeneralWebViewPage>();
            builder.RegisterType<PdfWebViewPage>();
            builder.RegisterType<ImageViewPage>();
        }

        private static void RegisterNewsAndExtras(ContainerBuilder builder)
        {
            builder.RegisterType<EventsSingleItemDetailViewModel>();
            builder.RegisterType<JobOffersPage>();
            builder.RegisterType<SprungbrettViewModel>();
            builder.RegisterType<RaumfreiOffersPage>();
            builder.RegisterType<RaumfreiViewModel>();
            builder.RegisterType<RaumfreiOfferDetailPage>();
            builder.RegisterType<RaumfreiDetailViewModel>();
        }

        private static void RegisterMainPages(ContainerBuilder builder)
        {
            builder.RegisterType<LanguagesPage>();
            builder.RegisterType<LocationsPage>();
            builder.RegisterType<SearchListPage>();
            builder.RegisterType<ContentContainerPage>();
            builder.RegisterType<MainContentPage>();
            builder.RegisterType<ExtrasContentPage>();
            builder.RegisterType<EventsContentPage>();

            builder.RegisterType<MainTwoLevelPage>();

            builder.RegisterType<SettingsPage>();
            builder.RegisterType<FCMSettingsPage>();
            builder.RegisterType<FCMTopicsSettingsPage>();
        }

        private static void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<PageViewModel>();
            builder.RegisterType<EventPageViewModel>();

            builder.RegisterType<LocationsViewModel>();
            builder.RegisterType<LanguagesViewModel>(); // can have multiple instances

            builder.RegisterType<SearchViewModel>();

            builder.RegisterType<ContentContainerViewModel>().SingleInstance();
            builder.RegisterType<MainContentPageViewModel>().SingleInstance();
            builder.RegisterType<ExtrasContentPageViewModel>().SingleInstance();
            builder.RegisterType<EventsContentPageViewModel>().SingleInstance();

            builder.RegisterType<MainTwoLevelViewModel>();

            builder.RegisterType<GeneralWebViewPageViewModel>();
            builder.RegisterType<PdfWebViewPageViewModel>();
            builder.RegisterType<ImagePageViewModel>();

            builder.RegisterType<SettingsPageViewModel>();
            builder.RegisterType<FcmSettingsPageViewModel>();
            builder.RegisterType<FcmTopicsSettingsPageViewModel>();
        }

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

        private static Page Instance()
            => Application.Current.MainPage;
    }
}
