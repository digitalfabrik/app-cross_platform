﻿using Autofac;
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
using Integreat.Shared.Models;
using Integreat.Shared.Pages.Redesign;
using Integreat.Shared.Pages.Redesign.Events;
using Integreat.Shared.Pages.Redesign.General;
using Integreat.Shared.Pages.Redesign.Main;
using Integreat.Shared.Pages.Redesign.Settings;
using Integreat.Shared.ViewModels.Resdesign;
using Integreat.Shared.ViewModels.Resdesign.Events;
using Integreat.Shared.ViewModels.Resdesign.General;
using Integreat.Shared.ViewModels.Resdesign.Main;
using Integreat.Shared.ViewModels.Resdesign.Settings;

namespace Integreat.ApplicationObject
{
    public class AppSetup
    {
        //Initializes Application instance that represents a cross-platform mobile application.
        private readonly Application _application;
        //Inizializes a ContainerBuilder which is needed to create instances of IContainer.
        private readonly ContainerBuilder _cb;

        //
        public AppSetup(Application application, ContainerBuilder cb)
        {
            _application = application;
            _cb = cb;
            //Initializes ListView derivative to present lists of data[TODO: Which data?]
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

            viewFactory.Register<LanguagesViewModel, LanguagesPage>();
            viewFactory.Register<LocationsViewModel, LocationsPage>();
            viewFactory.Register<SearchViewModel, SearchListPage>();

            // redesign
            viewFactory.Register<ContentContainerViewModel, ContentContainerPage>();
            viewFactory.Register<MainContentPageViewModel, MainContentPage>();
            viewFactory.Register<ExtrasContentPageViewModel, ExtrasContentPage>();
            viewFactory.Register<EventsContentPageViewModel, EventsContentPage>();

            // main
            viewFactory.Register<MainTwoLevelViewModel, MainTwoLevelPage>();
            viewFactory.Register<MainSingleItemDetailViewModel, MainSingleItemDetailPage>();

            // events
            viewFactory.Register<EventsSingleItemDetailViewModel, EventsSingleItemDetailPage>();

            // extras
            viewFactory.Register<SprungbrettViewModel, JobOffersPage>();
            viewFactory.Register<Careers4RefugeesViewModel, JobOffersPage>();

            // general
            viewFactory.Register<GeneralWebViewPageViewModel, GeneralWebViewPage>();

            // settings
            viewFactory.Register<SettingsPageViewModel, SettingsPage>();
        }

        private void ConfigureApplication(IComponentContext container)
        {
            var viewFactory = container.Resolve<IViewFactory>();

            // THE CODE BELOW IS FOR DEBUGGING POURPOSE
            //------------------------------------------------------------------------------

            // check whether to start with MainPageViewModel or LocationsViewModel
            //var locationId = Preferences.Location();

            // clear language selection for testing
            //Preferences.SetLocation(new Location() { Id = -1 });

            // clear cache
            // Cache.ClearCachedResources();
            /*
 			if (locationId >= 0 && !Preferences.Language(locationId).IsNullOrEmpty())
 			{
 				mainPage = viewFactory.Resolve<MainPageViewModel>();
 			}
 			else
 			{*/

            //  mainPage = new NavigationPage(viewFactory.Resolve<LocationsViewModel>()) {BarTextColor = (Color)Application.Current.Resources["secondaryColor"] };
            //--------------------------------------------------------------------------------

            var mainPage = new NavigationPage(viewFactory.Resolve<ContentContainerViewModel>()) { BarTextColor = (Color)Application.Current.Resources["textColor"], BackgroundColor = (Color)Application.Current.Resources["highlightColor"] };
            
            //--------------------------------------------------------------------------------
            // mainPage = new NavigationPage(viewFactory.Resolve<ContentContainerViewModel>());
            //  }

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
