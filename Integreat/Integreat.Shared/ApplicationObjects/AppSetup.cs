using Autofac;
using DLToolkit.Forms.Controls;
using Integreat.Shared;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Factories;
using Integreat.Shared.Pages;
using Integreat.Shared.Pages.General;
using Integreat.Shared.Pages.Main;
using Integreat.Shared.Pages.Search;
using Integreat.Shared.Pages.Settings;
using Integreat.Shared.Services;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using Integreat.Shared.ViewModels.Events;
using Integreat.Shared.ViewModels.General;
using Integreat.Shared.ViewModels.Search;
using Integreat.Shared.ViewModels.Settings;
using Xamarin.Forms;
using Integreat.Shared.Effects;

namespace Integreat.ApplicationObject
{
    /// <summary>
    /// The application configuration and view registration is done on this place
    /// </summary>
    public class AppSetup
    {
        //Initializes Application instance that represents a cross-platform mobile application.
        public readonly Application ApplicationInstance;
        //Inizializes a ContainerBuilder which is needed to create instances of IContainer.
        private readonly ContainerBuilder _cb;

        //Current Containter object (Actually just for deepLinking purposes)

        public IContainer Container { get; private set; }


        /// <summary> Initializes a new instance of the <see cref="AppSetup"/> class. </summary>
        /// <param name="application">The application.</param>
        /// <param name="cb">The cb.</param>
        public AppSetup(Application application, ContainerBuilder cb)
        {
            ApplicationInstance = application;
            _cb = cb;
            //Initializes ListView derivative to present lists of data[TODO: Which data?]
            FlowListView.Init();
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            ConfigureContainer(_cb);
            Container = _cb.Build();

            var viewFactory = Container.Resolve<IViewFactory>();
            RegisterViews(viewFactory);
            ConfigureApplication(viewFactory);
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

            // events
            viewFactory.Register<EventsSingleItemDetailViewModel, GeneralWebViewPage>();

            // extras
            viewFactory.Register<SprungbrettViewModel, JobOffersPage>();

            // general
            viewFactory.Register<GeneralWebViewPageViewModel, GeneralWebViewPage>();
            viewFactory.Register<PdfWebViewPageViewModel, PdfWebViewPage>();
            viewFactory.Register<ImagePageViewModel, ImageViewPage>();
            // settings
            viewFactory.Register<SettingsPageViewModel, SettingsPage>();
        }

        private void ConfigureApplication(IViewFactory viewFactory)
        {
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

            // reset HTML raw view
            Preferences.SetHtmlRawView(false);

            var mainPage = new NavigationPage(viewFactory.Resolve<ContentContainerViewModel>()) { BarTextColor = (Color)Application.Current.Resources["TextColor"], BackgroundColor = (Color)Application.Current.Resources["HighlightColor"] };

            //--------------------------------------------------------------------------------
            // mainPage = new NavigationPage(viewFactory.Resolve<ContentContainerViewModel>());
            //  }

            ApplicationInstance.MainPage = mainPage;    

            //set statusbar backgroundcolor
            StatusBarEffect.SetBackgroundColor((Color)_application.Resources["StatusBarColor"]);

            //add effect to mainpage
            _application.MainPage.Effects.Add(new StatusBarEffect());

        }

        protected virtual void ConfigureContainer(ContainerBuilder cb)
        {
            cb.RegisterModule<IntegreatModule>();
            // service registration
            cb.RegisterType<DialogService>().As<IDialogProvider>().SingleInstance();
            cb.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
            cb.RegisterType<Navigator>().As<INavigator>().SingleInstance();
            cb.RegisterType<DeepLinkService>().As<IDeepLinkService>().SingleInstance();
            cb.Register(c => new ShortnameParser(DataLoadServiceFactory.Create())).AsImplementedInterfaces();
            // Current PageProxy
            cb.RegisterType<PageProxy>().As<IPage>().SingleInstance();         
        }
    }
}
