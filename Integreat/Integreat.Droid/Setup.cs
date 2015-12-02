using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autofac;
using Integreat.ApplicationObject;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;
using Refit;
using SQLite.Net.Interop;
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