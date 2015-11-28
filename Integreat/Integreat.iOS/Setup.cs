using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Integreat.ApplicationObject;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;
using SQLite.Net.Platform.XamarinIOS;

namespace Integreat.iOS
{
    public class Setup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder cb)
        {
            base.RegisterDependencies(cb);
            cb.Register(c => new PersistanceService(new SQLitePlatformIOS())).As<PersistanceService>();
        }
    }
}
