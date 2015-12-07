using Autofac;
using Integreat.ApplicationObject;
using Integreat.Shared.Services.Persistance;
using SQLite.Net.Platform.XamarinAndroid;

namespace Integreat.Droid
{
    public class Setup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder cb)
        {
            base.RegisterDependencies(cb);
            cb.Register(c => new PersistenceService(new SQLitePlatformAndroid())).As<PersistenceService>();
            //cb.RegisterType<DroidHelloFormsService>().As<IHelloFormsService>();
        }
    }
}