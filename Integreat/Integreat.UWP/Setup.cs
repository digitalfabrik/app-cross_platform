using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Integreat.ApplicationObject;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;
using Autofac;
using SQLite.Net.Platform.WinRT;

namespace Integreat.UWP
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