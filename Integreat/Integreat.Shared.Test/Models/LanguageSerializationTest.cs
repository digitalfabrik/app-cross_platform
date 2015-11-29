using System.Collections.Generic;
using Integreat.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Integreat.Shared.Test.Models
{
    // data from http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/wp-json/extensions/v0/languages/wpml
    [TestFixture]
    internal class LanguageSerializationTest
    {

        private string _serializedLanguage;

        [SetUp]
        public void Before()
        {
            var language = Mocks.Language;
            var languageDictionary = new Dictionary<string, object>
            {
                {"id", language.Id},
                {"code", language.ShortName},
                {"native_name", language.Name},
                {"country_flag_url", language.IconPath}
            };
            _serializedLanguage = JsonConvert.SerializeObject(languageDictionary);
        }

        [Test]
        public void DeserializationTest()
        {
            AssertionHelper.AssertLanguage(Mocks.Language, JsonConvert.DeserializeObject<Language>(_serializedLanguage));
        }
    }
}
