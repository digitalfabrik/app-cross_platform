using Autofac;
using Integreat.Shared.Services;
using Xamarin.Forms;
using Integreat.Shared.Pages;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.ViewModels;
using Integreat.Shared;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewFactory;
using Page = Xamarin.Forms.Page;
using DLToolkit.Forms.Controls;
using Integreat.Shared.Services.Persistence;

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
            FlowListView.Init();
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
            viewFactory.Register<DisclaimerViewModel, DisclaimerListPage>();

            viewFactory.Register<PagesViewModel, InformationOverviewPage>();
            viewFactory.Register<DetailedPagesViewModel, DetailedInformationPage>();
            viewFactory.Register<PageViewModel, DetailPage>();

            viewFactory.Register<EventPageViewModel, EventDetailPage>();
            viewFactory.Register<EventPagesViewModel, EventsOverviewPage>();

            viewFactory.Register<LanguagesViewModel, LanguagesPage>();
            viewFactory.Register<LocationsViewModel, LocationsPage>();

            viewFactory.Register<MainPageViewModel, MainPage>();
            viewFactory.Register<NavigationViewModel, NavigationDrawerPage>();
            viewFactory.Register<SearchViewModel, SearchListPage>();
            viewFactory.Register<TabViewModel, TabPage>();
        }

        private void ConfigureApplication(IComponentContext container)
        {
            var viewFactory = container.Resolve<IViewFactory>();

            // check whether to start with MainPageViewModel or LocationsViewMpdel
            Page mainPage;
            var locationId =  Preferences.Location();
            if (locationId >= 0 && !Preferences.Language(locationId).IsNullOrEmpty())
            {
                mainPage = viewFactory.Resolve<MainPageViewModel>();
            }
            else
            {
                mainPage = new NavigationPage(viewFactory.Resolve<LocationsViewModel>()) {BarTextColor = (Color)Application.Current.Resources["accentColor"] };
                
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
