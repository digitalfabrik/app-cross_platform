using Autofac;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.ViewModels;
using System;
using System.Net.Http;
using Fusillade;
using Integreat.Shared.Pages;
using Integreat.Shared.Pages.Redesign;
using Integreat.Shared.Pages.Redesign.Events;
using Integreat.Shared.Pages.Redesign.General;
using Integreat.Shared.Pages.Redesign.Main;
using Integreat.Shared.Services.Network;
using Integreat.Shared.ViewModels.Resdesign;
using Integreat.Shared.ViewModels.Resdesign.Events;
using Integreat.Shared.ViewModels.Resdesign.General;
using Integreat.Shared.ViewModels.Resdesign.Main;
using ModernHttpClient;
using Newtonsoft.Json;
using Refit;
using Xamarin.Forms;
using Debug = System.Diagnostics.Debug;
using NavigationPage = Xamarin.Forms.NavigationPage;
using Page = Xamarin.Forms.Page;

namespace Integreat.Shared.ApplicationObjects
{
    public class IntegreatModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance<Func<Priority, INetworkService>>(Instance);

            //
            // VIEW MODELS
            // 

            // register loader
            builder.RegisterType<PageLoader>();
            builder.RegisterType<LocationsLoader>();
            builder.RegisterType<LanguagesLoader>();
            builder.RegisterType<EventPageLoader>();
            builder.RegisterType<DisclaimerLoader>();

            // register view models
            builder.RegisterType<PagesViewModel>();
            builder.RegisterType<DetailedPagesViewModel>();
            builder.RegisterType<PageViewModel>();

            builder.RegisterType<EventPagesViewModel>();
            builder.RegisterType<EventPageViewModel>();

            builder.RegisterType<DisclaimerViewModel>();

            builder.RegisterType<LocationsViewModel>();
            builder.RegisterType<LanguagesViewModel>(); // can have multiple instances

            builder.RegisterType<NavigationViewModel>();
            builder.RegisterType<TabViewModel>();
            builder.RegisterType<MainPageViewModel>();

            builder.RegisterType<SearchViewModel>();
            // redesign
            builder.RegisterType<ContentContainerViewModel>();
            builder.RegisterType<MainContentPageViewModel>();
            builder.RegisterType<ExtrasContentPageViewModel>();
            builder.RegisterType<EventsContentPageViewModel>();
            builder.RegisterType<SettingsContentPageViewModel>();

            // main
            builder.RegisterType<MainSingleItemDetailViewModel>();
            builder.RegisterType<MainTwoLevelViewModel>();

            // general
            builder.RegisterType<GeneralWebViewPageViewModel>();

            //
            // PAGES
            //

            // register views
            builder.RegisterType<EventDetailPage>();
            builder.RegisterType<EventsOverviewPage>();
            builder.RegisterType<InformationOverviewPage>();
            builder.RegisterType<DetailedInformationPage>();
            builder.RegisterType<DisclaimerListPage>();
            builder.RegisterType<LanguagesPage>();
            builder.RegisterType<LocationsPage>();
            builder.RegisterType<MainPage>();
            builder.RegisterType<NavigationDrawerPage>();
            builder.RegisterType<DetailPage>();
            builder.RegisterType<SearchListPage>();
            builder.RegisterType<TabPage>();
            // redesign
            builder.RegisterType<ContentContainerPage>();
            builder.RegisterType<MainContentPage>();
            builder.RegisterType<ExtrasContentPage>();
            builder.RegisterType<EventsContentPage>();
            builder.RegisterType<SettingsContentPage>();

            // main
            builder.RegisterType<MainSingleItemDetailPage>();
            builder.RegisterType<MainTwoLevelPage>();

            // events
            builder.RegisterType<EventsSingleItemDetailPage>();
            builder.RegisterType<EventsSingleItemDetailViewModel>();

			// extras
			builder.RegisterType<Careers4RefugeesPage>();
			builder.RegisterType<Careers4RefugeesViewModel>();
			builder.RegisterType<SprungbrettPage>();
			builder.RegisterType<SprungbrettViewModel>();

            // general
            builder.RegisterType<GeneralWebViewPage>();

            // current page resolver
            builder.RegisterInstance<Func<Page>>(Instance);
        }

        private static INetworkService Instance(Priority priority)
        {
            Func<HttpMessageHandler, INetworkService> createClient = messageHandler =>
            {
                // service registration
                var networkServiceSettings = new RefitSettings
                {
                    JsonSerializerSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Error = (sender, args) => Debug.WriteLine(args)
                        //, TraceWriter = new ConsoleTraceWriter() // debug tracer to see the json input
                    }
                };

                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri("http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/")
                };

                return RestService.For<INetworkService>(client, networkServiceSettings);
            };

            return
                new SafeNetworkService(
                    createClient(new RateLimitedHttpMessageHandler(new NativeMessageHandler(), priority)));
        }

        private static Page Instance()
        {
            var masterDetailPage = Application.Current.MainPage as MasterDetailPage;
            if (masterDetailPage == null)
            {
                return Application.Current.MainPage;
            }
            var navigationPage = masterDetailPage.Detail as NavigationPage;
            return navigationPage != null ? navigationPage.CurrentPage : masterDetailPage.Detail;
        }
    }
}
