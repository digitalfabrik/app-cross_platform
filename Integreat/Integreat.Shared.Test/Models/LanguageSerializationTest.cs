using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Integreat.Shared.Test.Models
{
    // data from http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/wp-json/extensions/v0/languages/wpml
    [TestFixture]
    internal class LanguageSerializationTest
    {
        private readonly int _id = 1;
        private readonly string _code = "en";
        private readonly string _native_name = "English";
        private readonly string _country_flag_url = "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/plugins/sitepress-multilingual-cms/res/flags/en.png";

        private string _serializedLanguage;

        [SetUp]
        public void Before()
        {
            var languageDictionary = new Dictionary<string, object>
            {
                {"id", _id},
                {"code", _code},
                {"native_name", _native_name},
                {"country_flag_url", _country_flag_url}
            };
            _serializedLanguage = JsonConvert.SerializeObject(languageDictionary);
        }

        [Test]
        public void DeserializationTest()
        {
            var language = JsonConvert.DeserializeObject<Language>(_serializedLanguage);
            Assert.AreEqual(_id, language.Id);
            Assert.AreEqual(_code, language.ShortName);
            Assert.AreEqual(_native_name, language.Name);
            Assert.AreEqual(_country_flag_url, language.IconPath);
        }
    }
}
