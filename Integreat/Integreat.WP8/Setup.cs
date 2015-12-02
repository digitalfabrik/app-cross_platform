using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autofac;
using Integreat.ApplicationObject;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;
using SQLite.Net.Platform.WinRT;

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