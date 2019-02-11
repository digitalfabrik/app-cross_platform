using Autofac;
using Integreat.Shared.Data;
using Integreat.Shared.Data.Factories;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Data.Loader.Targets;
using Integreat.Shared.Data.Sender;
using Integreat.Shared.Data.Sender.Targets;
using Integreat.Shared.Data.Services;
using Integreat.Shared.Pages;
using Integreat.Shared.Pages.General;
using Integreat.Shared.Pages.Main;
using Integreat.Shared.Pages.Search;
using Integreat.Shared.Pages.Settings;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using Integreat.Shared.ViewModels.Events;
using Integreat.Shared.Views;
using Integreat.Utilities;
using Newtonsoft.Json;
using Refit;
using System;
using System.Net.Http;
using System.Security;
using Xamarin.Forms;
using Debug = System.Diagnostics.Debug;
using Page = Xamarin.Forms.Page;

namespace Integreat.Shared.ViewFactory
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
            builder.RegisterType<DataSenderProvider>();
            builder.RegisterType<LocationsDataLoader>();
            builder.RegisterType<LanguagesDataLoader>();
            builder.RegisterType<DisclaimerDataLoader>();
            builder.RegisterType<EventPagesDataLoader>();
            builder.RegisterType<ExtrasDataLoader>();
            builder.RegisterType<FeedbackDataSender>();
            builder.Register(_ => new BackgroundDownloader(HttpClientFactory.GetHttpClient(
                new Uri(Constants.IntegreatReleaseUrl)))).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PagesDataLoader>();
            builder.Register(_ => new Parser(HttpClientFactory.GetHttpClient(new Uri(
                Constants.IntegreatReleaseUrl)))).AsImplementedInterfaces().SingleInstance();
        }

        private static void RegisterGeneralPageTypes(ContainerBuilder builder)
        {
            builder.RegisterType<GeneralWebViewPage>();
            builder.RegisterType<GeneralContentPage>();
            builder.RegisterType<PdfWebViewPage>();
            builder.RegisterType<ImageViewPage>();

            builder.RegisterType<FeedbackView>();
            builder.RegisterType<Pages.Feedback.FeedbackDialogView>();
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

            builder.RegisterType<FeedbackDialogViewModel>();

            builder.RegisterType<SearchViewModel>();

            builder.RegisterType<ContentContainerViewModel>().SingleInstance();
            builder.RegisterType<MainContentPageViewModel>().SingleInstance();
            builder.RegisterType<ExtrasContentPageViewModel>().SingleInstance();
            builder.RegisterType<EventsContentPageViewModel>().SingleInstance();

            builder.RegisterType<MainTwoLevelViewModel>();

            builder.RegisterType<GeneralWebViewPageViewModel>();
            builder.RegisterType<GeneralContentPageViewModel>();
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
