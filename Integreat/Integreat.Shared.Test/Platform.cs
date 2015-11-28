using System;
using System.Collections.Generic;
using System.Text;
using Integreat.ApplicationObject;
#if __ANDROID__
using Integreat.Droid;
#else
using Integreat.iOS;
#endif

namespace Integreat.Shared.Test
{
    public class Platform
    {
        public static AppSetup Setup
        {
            get
            {
#if NETFX_CORE
#else

#if SILVERLIGHT
#else

#if __ANDROID__
                return new Setup();
#else

                return new Setup();
#endif
#endif
#endif
            }

        }
    }
}
