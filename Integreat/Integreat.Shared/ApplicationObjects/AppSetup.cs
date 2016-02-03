using Autofac;
using Integreat.Shared.Services;
using Xamarin.Forms;
using System;
using Integreat.Shared.Pages;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.ViewModels;
using Integreat.Shared;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewFactory;
using Page = Xamarin.Forms.Page;

namespace Integreat.ApplicationObject
{
    public class AppSetup
    {
        private readonly Application _application;
        private readonly ContainerBuilder _cb;

        public AppSetup(Application application, ContainerBuilder cb)
        {
            _application = application;
            _cb = cb;
        }

        public void Run()
        {
            ConfigureContainer(_cb);
            var container = _cb.Build();

            var viewFactory = container.Resolve<IViewFactory>();
            RegisterViews(viewFactory);

            ConfigureApplication(container);
        }

        private static void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<DisclaimerViewModel, DisclaimerPagesView>();

            viewFactory.Register<PagesViewModel, InformationOverviewPage>();
            viewFactory.Register<PageViewModel, PageDetailView>();

            viewFactory.Register<EventPageViewModel, EventPageDetailView>();
            viewFactory.Register<EventPagesViewModel, EventsOverviewPage>();

            viewFactory.Register<LanguagesViewModel, LanguagesPage>();
            viewFactory.Register<LocationsViewModel, LocationsPage>();

            viewFactory.Register<MainPageViewModel, MainPage>();
            viewFactory.Register<NavigationViewModel, Shared.Pages.NavigationPage>();
            viewFactory.Register<SearchViewModel, PageSearchList>();
            viewFactory.Register<TabViewModel, TabPage>();
        }

        private void ConfigureApplication(IComponentContext container)
        {
            var viewFactory = container.Resolve<IViewFactory>();

            // check whether to start with MainPageViewModel or LocationsViewMpdel
            Page mainPage;
            var locationId =  Preferences.Location();
            if (locationId > 0 && Preferences.Language(locationId) > 0)
            {
                mainPage = viewFactory.Resolve<MainPageViewModel>();
            }
            else
            {
                mainPage = viewFactory.Resolve<LocationsViewModel>();
            }
            _application.MainPage = mainPage;
        }

        protected virtual void ConfigureContainer(ContainerBuilder cb)
        {
            // service registration
            cb.RegisterType<DialogService>().As<IDialogProvider>().SingleInstance();
            cb.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
            cb.RegisterType<Navigator>().As<INavigator>().SingleInstance();
            
            // Current PageProxy
            cb.RegisterType<PageProxy>().As<IPage>().SingleInstance();
            cb.RegisterModule<IntegreatModule>(); 
        }
    }
}
