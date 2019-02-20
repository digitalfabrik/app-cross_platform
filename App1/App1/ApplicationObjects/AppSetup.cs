using App1.Dialogs;
using App1.Effects;
using App1.Firebase;
using App1.Navigator;
using App1.Pages;
using App1.Pages.Extras;
using App1.Pages.General;
using App1.Pages.Main;
using App1.Pages.Search;
using App1.Pages.Settings;
using App1.Utilities;
using App1.ViewFactory;
using App1.ViewModels;
using App1.ViewModels.Events;
using App1.ViewModels.Extras;
using App1.ViewModels.General;
using App1.ViewModels.Main;
using App1.ViewModels.Search;
using App1.ViewModels.Settings;
using Autofac;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;

namespace App1.ApplicationObjects
{
    /// <summary>
    /// The application configuration and view registration is done on this place
    /// </summary>
    public class AppSetup
    {
        private readonly Application _application;
        private readonly ContainerBuilder _cb;

        //
        public AppSetup(Application application, ContainerBuilder cb)
        {
            _application = application;
            _cb = cb;
            //Initializes ListView derivative to present lists of data
            FlowListView.Init();
        }

        public void Run()
        {
            ConfigureContainer(_cb);
            var container = _cb.Build();
            IntegreatApp.Container = container;

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

            // events
            viewFactory.Register<EventsSingleItemDetailViewModel, GeneralWebViewPage>();

            // extras
            viewFactory.Register<SprungbrettViewModel, JobOffersPage>();
            viewFactory.Register<RaumfreiViewModel, RaumfreiOffersPage>();
            viewFactory.Register<RaumfreiDetailViewModel, RaumfreiOfferDetailPage>();

            // general
            viewFactory.Register<GeneralWebViewPageViewModel, GeneralWebViewPage>();
            viewFactory.Register<PdfWebViewPageViewModel, PdfWebViewPage>();
            viewFactory.Register<ImagePageViewModel, ImageViewPage>();
            // settings
            viewFactory.Register<SettingsPageViewModel, SettingsPage>();
            viewFactory.Register<FcmSettingsPageViewModel, FCMSettingsPage>();
            viewFactory.Register<FcmTopicsSettingsPageViewModel, FCMTopicsSettingsPage>();
        }

        private void ConfigureApplication(IComponentContext container)
        {
            var viewFactory = container.Resolve<IViewFactory>();
            // reset HTML raw view
            Preferences.SetHtmlRawView(false);

            Helpers.Platform.GetCurrentMainPage(viewFactory);

            if (Device.RuntimePlatform == Device.iOS)
            {
                SetStatusBarAndAddToMainPage();
            }
        }

        private void SetStatusBarAndAddToMainPage()
        {
            StatusBarEffect.SetBackgroundColor((Color)_application.Resources["StatusBarColor"]);
            //add effect to main page
            _application.MainPage.Effects.Add(new StatusBarEffect());
        }

        private static void ConfigureContainer(ContainerBuilder cb)
        {
            // service registration
            cb.RegisterType<DialogService>().As<IDialogProvider>().SingleInstance();
            cb.RegisterType<ViewFactory.ViewFactory>().As<IViewFactory>().SingleInstance();
            cb.RegisterType<Navigator.Navigator>().As<INavigator>().SingleInstance();

            cb.RegisterType<FirebaseHelper>().SingleInstance();
            cb.RegisterType<PushNotificationHandler>().As<IPushNotificationHandler>().SingleInstance();

            // Current PageProxy
            cb.RegisterType<PageProxy>().As<IPage>().SingleInstance();
            cb.RegisterModule<IntegreatModule>();
        }
    }
}
