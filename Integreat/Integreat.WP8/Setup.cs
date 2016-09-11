
using Integreat.ApplicationObject;
using Integreat.Shared.Services.Persistence;

namespace Integreat.WP8
{
    public class Setup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder cb)
        {
            base.RegisterDependencies(cb);
            cb.Register(c => new PersistenceService(new SQLitePlatformWinRT())).As<PersistenceService>();
        }
    }
}