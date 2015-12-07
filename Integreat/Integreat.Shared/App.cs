using Autofac;
using Integreat.ApplicationObject;
using Integreat.Shared.Services.Persistance;
using Integreat.Shared.Views;
using Xamarin.Forms;

namespace Integreat
{
	public class App : Application
    {
        public App (AppSetup setup)
        {
            AppContainer.Container = setup.CreateContainer();
            AppContainer.Container.Resolve<PersistenceService>().Init();
            MainPage = new RootPage();
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
