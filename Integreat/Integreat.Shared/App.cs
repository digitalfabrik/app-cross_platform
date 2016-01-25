using Autofac;
using Integreat.ApplicationObject;
using Integreat.Shared;
using Integreat.Shared.Pages;
using Integreat.Shared.Services.Persistance;
using Xamarin.Forms;

namespace Integreat
{
	public class App : Application
    {
        public App (AppSetup setup)
        {
            AppContainer.Container = setup.CreateContainer();
            AppContainer.Container.Resolve<PersistenceService>().Init();
            MainPage = new NavigationPage(new SelectLocationPage());
            //MainPage = new RootPage();
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
