using System;

namespace Integreat
{
    public static class Konstanten
    {
        public const string BaseUri = "http://vmkrcmar21.informatik.tu-muenchen.de";
        public const string isServerAliveUri = "/alive";
        public const string getAvailableLocationsUri = "/wordpress/wp-json/extensions/v0/multisites/";
        public const string getAvailableLanguagesUri = "{0}de/wp-json/extensions/v0/languages/wpml";
        public const string subscribePushUri = "/{0}";
        public const string unsubscribePushUri = "/{0}";
        public const string getEventPagesUri = "{0}{1}/wp-json/extensions/v0/modified_content/events";
        public const string getPagesUri = "{0}{1}/wp-json/extensions/v0/modified_content/pages";
    }
}

