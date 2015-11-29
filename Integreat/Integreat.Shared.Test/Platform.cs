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
        public static AppSetup Setup => new Setup();
    }
}
