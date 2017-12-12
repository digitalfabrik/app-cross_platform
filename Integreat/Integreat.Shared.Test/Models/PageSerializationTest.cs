using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Integreat.Shared.Test.Models
{
	// data from http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/wp-json/extensions/v0/modified_content/Pages?since=2015-01-01T13%3A04%3A39-0700
	[TestFixture]
	internal class PageSerializationTest
	{
		private string _serializedPage;

		public Dictionary<string, string> Author {
			get {
				var author = Mocks.Page.Author;
				return new Dictionary<string, string> {
					{ "login", author.Login },
					{ "first_name", author.FirstName },
					{ "last_name", author.LastName }
				};
			}
		}

		public Dictionary<string, object> PageDictionary {
			get {
				var page = Mocks.Page;
				return new Dictionary<string, object> {
					{ "id", page.Id },
					{ "type", page.Type },
					{ "status", page.Status },
					{ "modified_gmt", page.Modified.ToRestAcceptableString () },
					{ "title", page.Title },
					{ "excerpt", page.Description },
					{ "content", page.Content },
					{ "parent", page.ParentId },
					{ "order", page.Order }, {
                        "available_languages", page.AvailableLanguages.ToDictionary(availableLanguage =>
                            availableLanguage.LanguageId, availableLanguage => availableLanguage.OtherPageId)
                    },
                    {"thumbnail", page.Thumbnail},
                    {"author", Author}
                };
            }
        }


        [SetUp]
        public void Before()
        {
            _serializedPage = JsonConvert.SerializeObject(PageDictionary);
        }

        [Test]
        public void DeserializationTest()
        {
            AssertionHelper.AssertPage(Mocks.Page, JsonConvert.DeserializeObject<Page>(_serializedPage));
        }

       
    }
}
