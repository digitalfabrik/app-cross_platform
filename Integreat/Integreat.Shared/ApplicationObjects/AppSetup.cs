using Autofac;
using DLToolkit.Forms.Controls;
using Integreat.Shared;
using Integreat.Shared.Effects;
using Integreat.Shared.Firebase;
using Integreat.Shared.Pages;
using Integreat.Shared.Pages.Feedback;
using Integreat.Shared.Pages.General;
using Integreat.Shared.Pages.Main;
using Integreat.Shared.Pages.Search;
using Integreat.Shared.Pages.Settings;
using Integreat.Shared.Services;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewFactory;
using Integreat.Shared.ViewModels;
using Integreat.Shared.ViewModels.Events;
using Integreat.Utilities;
using Xamarin.Forms;

namespace Integreat.ApplicationObject
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
            var popupViewFactory = container.Resolve<IPopupViewFactory>();

            RegisterViews(viewFactory, popupViewFactory);

            ConfigureApplication(container);
        }

        private static void RegisterViews(IViewFactory viewFactory, IPopupViewFactory popupViewFactory)
        {
            viewFactory.Register<LanguagesViewModel, LanguagesPage>();
            viewFactory.Register<LocationsViewModel, LocationsPage>();
            viewFactory.Register<SearchViewModel, SearchListPage>();

            popupViewFactory.Register<FeedbackDialogViewModel, FeedbackDialogView>();

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
            viewFactory.Register<GeneralContentPageViewModel, GeneralContentPage>();
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
            cb.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
            cb.RegisterType<PopupViewFactory>().As<IPopupViewFactory>().SingleInstance();
            cb.RegisterType<Navigator>().As<INavigator>().SingleInstance();

            cb.RegisterType<FirebaseHelper>().SingleInstance();
            cb.RegisterType<PushNotificationHandler>().As<IPushNotificationHandler>().SingleInstance();

            cb.RegisterType<FeedbackFactory>().SingleInstance();

            // Current PageProxy
            cb.RegisterType<PageProxy>().As<IPage>().SingleInstance();
            cb.RegisterModule<IntegreatModule>();
        }
    }
}
