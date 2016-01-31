using Autofac;
using Integreat.Shared.Services;
using Xamarin.Forms;
using System;
using Integreat.Shared.Pages;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.ViewModels;
using Integreat.Shared;
using Integreat.Shared.ViewFactory;

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
            var mainPage = viewFactory.Resolve<MainPageViewModel>();
            var navigationPage = new Xamarin.Forms.NavigationPage(mainPage);

            var backgroundColor = (Color) _application.Resources["backgroundColor"];
            var textColor = (Color) _application.Resources["textColor"];

            navigationPage.BarBackgroundColor = backgroundColor;
            navigationPage.BarTextColor = textColor;
            navigationPage.BackgroundColor = backgroundColor;

            _application.MainPage = navigationPage;
        }

        protected virtual void ConfigureContainer(ContainerBuilder cb)
        {
            // service registration
            cb.RegisterType<DialogService>().As<IDialogProvider>().SingleInstance();
            cb.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
            cb.RegisterType<Navigator>().As<INavigator>().SingleInstance();

            cb.RegisterInstance<Func<Page>>(() =>
            {
                // Check if we are using MasterDetailPage
                var masterDetailPage = Application.Current.MainPage as MasterDetailPage;
                var page = masterDetailPage != null ? masterDetailPage.Detail : Application.Current.MainPage;

                // Check if page is a NavigationPage
                var navigationPage = page as IPageContainer<Page>;
                return navigationPage != null ? navigationPage.CurrentPage : page;
            });

            // Current PageProxy
            cb.RegisterType<PageProxy>().As<IPage>().SingleInstance();
            cb.RegisterModule<IntegreatModule>(); 
        }
    }
}
