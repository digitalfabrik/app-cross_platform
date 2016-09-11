using Autofac;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.ViewModels;
using System;
using System.Net.Http;
using Fusillade;
using Integreat.Shared.Pages;
using Integreat.Shared.Services.Network;
using Integreat.Shared.Services.Persistence;
using ModernHttpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
                    }
                };

                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri("http://cms.integreat-app.de/wordpress/")
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
