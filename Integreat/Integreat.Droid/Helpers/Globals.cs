using Android.Views;

namespace Integreat.Droid.Helpers
{
    ////<summary>
    ////This is global static class, for accessing important information
    ////</summary>
    public class Globals
    {
        private static Window _window;

        public static Window Window { get => _window; set => _window = value; }
    }
}
