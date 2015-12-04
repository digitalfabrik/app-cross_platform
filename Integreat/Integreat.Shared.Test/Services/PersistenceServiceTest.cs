using System.Collections.Generic;
using Autofac;
using Integreat.Models;
using Integreat.Shared.Services.Persistance;
using NUnit.Framework;

namespace Integreat.Shared.Test.Services
{
    [TestFixture]
    internal class PersistenceServiceTest
    {
        private IContainer _container;
        private Language _language;
        private Location _location;
        private PersistenceService _persistenceService;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _container = Platform.Setup.CreateContainer();
            Assert.True(_container.TryResolve(out _persistenceService), "PersistenceService not found");
           _persistenceService.Init();
        }

        [SetUp]
        public void Setup()
        {
            _language = Mocks.Language;
            _location = Mocks.Location;
        }


        [TearDown]
        public void Tear()
        {
            Mocks.Identifier = 42;
        }

        [Test]
        public async void InsertAndGetLocation()
        {
            var location = await _persistenceService.Get<Location>(_location.Id);
            Assert.Null(location, "location is not null");

            await _persistenceService.Insert(_location);
            location = await _persistenceService.Get<Location>(_location.Id);
            AssertionHelper.AssertLocation(_location, location);

            await _persistenceService.Delete(location);
            location = await _persistenceService.Get<Location>(_location.Id);
            Assert.Null(location, "location is not null");
        }
        
        [Test]
        public async void InsertAndGetLanguage()
        {
            Assert.AreEqual(0, _language.PrimaryKey);
            var language = await _persistenceService.Get<Language>(_language.PrimaryKey);
            Assert.Null(language, "language is not null");

            await _persistenceService.Insert(_language);
            Assert.AreNotEqual(0, _language.PrimaryKey);
            language = await _persistenceService.Get<Language>(_language.PrimaryKey);
            AssertionHelper.AssertLanguage(_language, language);
            Assert.Null(language.Location, "Location is not null");

            await _persistenceService.Delete(language);
            language = await _persistenceService.Get<Language>(_language.PrimaryKey);
            Assert.Null(language, "language is not null");
        }

        [Test]
        public async void InsertAndGetLanguageWithLocation()
        {
            var language = await _persistenceService.Get<Language>(_language.PrimaryKey);
            Assert.Null(language, "language is not null");
            var location = await _persistenceService.Get<Location>(_location.Id);
            Assert.Null(location, "location is not null");

            _location.Languages = new List<Language> { _language };
            await _persistenceService.Insert(_location);
            language = await _persistenceService.Get<Language>(_language.PrimaryKey);
            AssertionHelper.AssertLanguage(_language, language);
            AssertionHelper.AssertLocation(_location, language.Location);

            await _persistenceService.Delete(language);
            language = await _persistenceService.Get<Language>(_language.PrimaryKey);
            Assert.Null(language, "language is not null");
            await _persistenceService.Delete(_location);
            location = await _persistenceService.Get<Location>(_location.Id);
            Assert.Null(location, "location is not null");
        }

        [Test]
        public async void InsertAndGetPage()
        {
            var expected = Mocks.Page;
            Assert.AreEqual(0, expected.PrimaryKey);
            var page = await _persistenceService.Get<Page>(expected.PrimaryKey);
            Assert.Null(page, "page is not null");

            await _persistenceService.Insert(expected);
            Assert.AreNotEqual(0, expected.PrimaryKey);
            page = await _persistenceService.Get<Page>(expected.PrimaryKey);
            AssertionHelper.AssertPage(expected, page);

            await _persistenceService.Delete(page);
            page = await _persistenceService.Get<Page>(expected.PrimaryKey);
            Assert.Null(page, "page should have been removed");
        }

        [Test]
        public async void InsertAndGetEventPage()
        {
            var expected = Mocks.EventPage;
            Assert.AreEqual(0, expected.PrimaryKey);
            var page = await _persistenceService.Get<EventPage>(expected.PrimaryKey);
            Assert.Null(page, "page is not null");

            await _persistenceService.Insert(expected);
            Assert.AreNotEqual(0, expected.PrimaryKey);
            page = await _persistenceService.Get<EventPage>(expected.PrimaryKey);
            AssertionHelper.AssertEventPage(expected, page);

            await _persistenceService.Delete(page);
            page = await _persistenceService.Get<EventPage>(expected.PrimaryKey);
            Assert.Null(page, "page is not null");
        }

        [Test]
        public async void InsertAndGetMultiplePages()
        {
            var length = 10;
            for (var i = 1; i < length + 1; i++)
            {
                Mocks.Identifier = i;
                var expected = Mocks.Page;
                await _persistenceService.Insert(expected);
                var actual = await _persistenceService.Get<Page>(expected.PrimaryKey);
                AssertionHelper.AssertPage(expected, actual);
            }

            // check whether or not the "desired" entry exists
            var count = await _persistenceService.Connection.Table<Page>().CountAsync();
            Assert.AreEqual(length, count);

            await _persistenceService.Connection.DeleteAllAsync<Page>();
            count = await _persistenceService.Connection.Table<Page>().CountAsync();
            Assert.AreEqual(0, count);
        }

        [Test]
        public async void InsertAndGetMultipleEventPages()
        {
            var length = 10;
            for (var i = 0; i < length; i++)
            {
                Mocks.Identifier += i;
                var expected = Mocks.EventPage;
                await _persistenceService.Insert(expected);
                var actual = await _persistenceService.Get<EventPage>(expected.PrimaryKey);
                AssertionHelper.AssertEventPage(expected, actual);
            }

            // check whether or not the "desired" entry exists
            var count = await _persistenceService.Connection.Table<EventPage>().CountAsync();
            Assert.AreEqual(length, count);

            await _persistenceService.Connection.DeleteAllAsync<EventPage>();
            count = await _persistenceService.Connection.Table<EventPage>().CountAsync();
            Assert.AreEqual(0, count);
        }

        [Test]
        public async void InsertAndGetParentChildrenPages()
        {
            var page1 = Mocks.Page;
            Mocks.Identifier++;
            var page2 = Mocks.Page;
         

            page1.SubPages = new List<Page> { page2 };
            Assert.AreEqual(0, page1.PrimaryKey);
            Assert.AreEqual(0, page2.PrimaryKey);
            await _persistenceService.Insert(page1);
            Assert.AreNotEqual(0, page1.PrimaryKey);
            Assert.AreNotEqual(0, page2.PrimaryKey);

            var actual = await _persistenceService.Get<Page>(page1.PrimaryKey);
            Assert.AreEqual(1, actual.SubPages.Count);
            AssertionHelper.AssertPage(page2, actual.SubPages[0]);
        }

        [Test]
        public async void InsertAndUpdatePage()
        {
            var expected = Mocks.Page;
            Assert.AreEqual(0, expected.PrimaryKey);
            var page = await _persistenceService.Get<Page>(expected.PrimaryKey);
            Assert.Null(page, "page is not null");

            await _persistenceService.Insert(expected);
            Assert.AreNotEqual(0, expected.PrimaryKey);
            page = await _persistenceService.Get<Page>(expected.PrimaryKey);
            AssertionHelper.AssertPage(expected, page);
            string updatedDescription = "Updated description";
            page.Description = updatedDescription;
            await _persistenceService.Insert(page);
            page = await _persistenceService.Get<Page>(expected.PrimaryKey);
            Assert.AreEqual(updatedDescription, page.Description);

            await _persistenceService.Delete(page);
            page = await _persistenceService.Get<Page>(expected.PrimaryKey);
            Assert.Null(page, "page should have been removed");
        }
    }
}