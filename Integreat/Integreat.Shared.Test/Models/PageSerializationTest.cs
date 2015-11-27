using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Integreat.Shared.Test.Models
{
    // data from http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/wp-json/extensions/v0/modified_content/pages?since=2015-01-01T13%3A04%3A39-0700
    [TestFixture]
    internal class PageSerializationTest
    {
        private string _id = "382";
        private string _title = "Wichtige Links";
        private string _type = "page";
        private string _status = "publish";
        private string _modified_gmt = "2015-10-10 11:42:51";

        private readonly string _excerpt =
            "Bundesamt f\u00fcr Migration: http:\\/\\/www.bamf.de\\/SharedDocs\\/Anlagen\\/DE\\/Publikationen\\/EMN\\/Glossary\\/emn-glossary.pdf?__blob=publicationFile Projekt \u00bbFirst Steps\u00ab http:\\/\\/www.first-steps-augsburg.de Stadt Augsburg mit Verwaltungswegweiser www.augsburg.de bitte gerne erg\u00e4nzen";

        private readonly string _content =
            "<p>Bundesamt f\u00fcr Migration: <a href=\"http:\\/\\/www.bamf.de\\/SharedDocs\\/Anlagen\\/DE\\/Publikationen\\/EMN\\/Glossary\\/emn-glossary.pdf?__blob=publicationFile\">http:\\/\\/www.bamf.de\\/SharedDocs\\/Anlagen\\/DE\\/Publikationen\\/EMN\\/Glossary\\/emn-glossary.pdf?__blob=publicationFile<\\/a><\\/p><p><\\/p><p>Projekt \u00bbFirst Steps\u00ab<\\/p><p><a href=\"http:\\/\\/www.first-steps-augsburg.de\">http:\\/\\/www.first-steps-augsburg.de<\\/a><\\/p><p><\\/p><p>Stadt Augsburg mit Verwaltungswegweiser<\\/p><p><a href=\"http:\\/\\/www.augsburg.de\">www.augsburg.de<\\/a><\\/p><p><\\/p><p><span style=\"color: #ff0000\"><strong>bitte gerne erg\u00e4nzen<\\/strong><\\/span><\\/p>";

        private string _parent = "0";
        private string _order = "62";

        private readonly Dictionary<string, int> _availableLanguages = new Dictionary<string, int>
        {
            {"en", 1052},
            {"fr", 1374},
            {"ar", 1289},
            {"fa", 1375}
        };

        private readonly string _thumbnail =
            "http:\\/\\/vmkrcmar21.informatik.tu-muenchen.de\\/wordpress\\/augsburg\\/wp-content\\/uploads\\/sites\\/2\\/2015\\/10\\/keyboard53.png";

        private readonly Dictionary<string, string> _author = new Dictionary<string, string>
        {
            {"login", "langerenken"},
            {"first_name", "Daniel"},
            {"last_name", "Langerenken"}
        };

        private string _serializedPage;

        public Dictionary<string, object> PageDictionary()
        {
            return new Dictionary<string, object>
            {
                {"id", _id},
                {"title", _title},
                {"type", _type},
                {"status", _status},
                {"modified_gmt", _modified_gmt},
                {"excerpt", _excerpt},
                {"content", _content},
                {"parent", _parent},
                {"order", _order},
                {"available_languages", _availableLanguages},
                {"thumbnail", _thumbnail},
                {"author", _author}
            };
        }

        [SetUp]
        public void Before()
        {
            _serializedPage = JsonConvert.SerializeObject(PageDictionary());
        }

        [Test]
        public void DeserializationTest()
        {
            PageDeserializationTest(JsonConvert.DeserializeObject<Page>(_serializedPage));
        }

        public void PageDeserializationTest(Page page)
        {
            Assert.AreEqual(_id, page.Id);
            Assert.AreEqual(_title, page.Title);
            Assert.AreEqual(_type, page.Type);
            Assert.AreEqual(_status, page.Status);
            Assert.AreEqual(_modified_gmt.DateTimeFromRestString().Ticks, page.Modified);
            Assert.AreEqual(_excerpt, page.Description);
            Assert.AreEqual(_content, page.Content);
            Assert.AreEqual(_parent, page.Parent);
            Assert.AreEqual(_order, page.Order);
            Assert.AreEqual(_thumbnail, page.Thumbnail);
            AvailableLanguagesTest(page.AvailableLanguages);
            AuthorTest(page.Author);
        }

        public void AvailableLanguagesTest(Collection<AvailableLanguage> languages)
        {
            foreach (var language in languages)
            {
                Assert.AreEqual(_availableLanguages[language.Language], language.OtherPageId);
            }
        }

        public void AuthorTest(Author author)
        {
            Assert.AreEqual(_author["login"], author.Login);
            Assert.AreEqual(_author["first_name"], author.FirstName);
            Assert.AreEqual(_author["last_name"], author.LastName);
        }
    }
}
