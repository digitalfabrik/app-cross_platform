using Autofac;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.ViewModels;
using System;
using Integreat.Shared.Models;
using Integreat.Shared.Pages;
using Integreat.Shared.Services.Network;
using Newtonsoft.Json;
using Refit;
using Xamarin.Forms;
using NavigationPage = Xamarin.Forms.NavigationPage;
using Page = Xamarin.Forms.Page;

namespace Integreat.Shared.ApplicationObjects
{
    public class IntegreatModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // service registration
            var networkServiceSettings = new RefitSettings
            {
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            };
            // var networkService = RestService.For<INetworkService> ("http://vmkrcmar21.informatik.tu-muenchen.de/", networkServiceSettings);
            var networkService = new NetworkServiceMock();
            builder.Register(c => new SafeNetworkService(networkService)).As<INetworkService>();

            // register loader
            builder.RegisterType<PageLoader>();
            builder.RegisterType<LocationsLoader>().SingleInstance();
            builder.RegisterType<LanguagesLoader>();
            builder.RegisterType<EventPageLoader>();
            builder.RegisterType<DisclaimerLoader>();

            // register view models
            builder.RegisterType<PagesViewModel>().SingleInstance();
            builder.RegisterType<PageViewModel>();

            builder.RegisterType<EventPagesViewModel>().SingleInstance();
            builder.RegisterType<EventPageViewModel>();
            
            builder.RegisterType<DisclaimerViewModel>();

            builder.RegisterType<LocationsViewModel>().SingleInstance();
            builder.RegisterType<LanguagesViewModel>(); // can have multiple instances

            builder.RegisterType<NavigationViewModel>().SingleInstance();
            builder.RegisterType<TabViewModel>().SingleInstance();
            builder.RegisterType<MainPageViewModel>().SingleInstance();

            builder.RegisterType<SearchViewModel>();

            // register views
            builder.RegisterType<EventPageDetailView>();
            builder.RegisterType<EventsOverviewPage>();
            builder.RegisterType<InformationOverviewPage>();
            builder.RegisterType<DisclaimerPagesView>();
            builder.RegisterType<LanguagesPage>();
            builder.RegisterType<LocationsPage>();
            builder.RegisterType<MainPage>();
            builder.RegisterType<Pages.NavigationPage>();
            builder.RegisterType<PageDetailView>();
            builder.RegisterType<PageSearchList>();
            builder.RegisterType<TabPage>();

            // current page resolver
            builder.RegisterInstance<Func<Page>>(() => ((NavigationPage)Application.Current.MainPage).CurrentPage);
        }
    }
}
