using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Integreat.iOS
{
    public class Setup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder cb)
        {
            base.RegisterDependencies(cb);

            //cb.RegisterType<TouchHelloFormsService>().As<IHelloFormsService>();
        }
    }
}
