using System.Collections.Generic;
using Autofac;
using Integreat.Models;
using Integreat.Shared.Services.Persistance;
using NUnit.Framework;

namespace Integreat.Shared.Test.Services
{
    [TestFixture]
    internal class PersistanceServiceTest
    {
        private IContainer _container;
        private Language _language;
        private Location _location;
        private PersistanceService _persistanceService;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _container = Platform.Setup.CreateContainer();
            Assert.True(_container.TryResolve(out _persistanceService), "PersistanceService not found");
           _persistanceService.Init();
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
            var location = await _persistanceService.Get<Location>(_location.Id);
            Assert.Null(location, "location is not null");
            await _persistanceService.Insert(_location);
            location = await _persistanceService.Get<Location>(_location.Id);
            Assert.NotNull(location, "location is null");
            Assert.AreEqual(_location.Id, location.Id, "location id is not set");
            await _persistanceService.Delete(location);
            location = await _persistanceService.Get<Location>(_location.Id);
            Assert.Null(location, "location is not null");
        }
        
        [Test]
        public async void InsertAndGetLanguage()
        {
            Assert.AreEqual(0, _language.PrimaryKey);
            var language = await _persistanceService.Get<Language>(_language.PrimaryKey);
            Assert.Null(language, "language is not null");
            await _persistanceService.Insert(_language);
            Assert.AreNotEqual(0, _language.PrimaryKey);
            language = await _persistanceService.Get<Language>(_language.PrimaryKey);
            Assert.NotNull(language, "Language is null");
            Assert.Null(language.Location, "Location is not null");
            Assert.AreEqual(_language.Id, language.Id, "language id is not set");
            Assert.AreEqual(_language.ShortName, language.ShortName);
            Assert.AreEqual(_language.Name, language.Name);
            Assert.AreEqual(_language.IconPath, language.IconPath);
            await _persistanceService.Delete(language);
            language = await _persistanceService.Get<Language>(_language.PrimaryKey);
            Assert.Null(language, "language is not null");
        }

        [Test]
        public async void InsertAndGetLanguageWithLocation()
        {
            _location.Languages = new List<Language> {_language};
            var language = await _persistanceService.Get<Language>(_language.PrimaryKey);
            Assert.Null(language, "language is not null");

            var location = await _persistanceService.Get<Location>(_location.Id);
            Assert.Null(location, "location is not null");

            await _persistanceService.Insert(_location);
            language = await _persistanceService.Get<Language>(_language.PrimaryKey);
            Assert.NotNull(language, "Language is null");
            Assert.NotNull(language.Location, "Location is null");

            await _persistanceService.Delete(language);
            language = await _persistanceService.Get<Language>(_language.PrimaryKey);
            Assert.Null(language, "language is not null");

            await _persistanceService.Delete(_location);
            location = await _persistanceService.Get<Location>(_location.Id);
            Assert.Null(location, "location is not null");
        }

        [Test]
        public async void InsertAndGetPage()
        {
            var expected = Mocks.Page;
            Assert.AreEqual(0, expected.PrimaryKey);
            var page = await _persistanceService.Get<Page>(expected.PrimaryKey);
            Assert.Null(page, "page is not null");

            await _persistanceService.Insert(expected);
            Assert.AreNotEqual(0, expected.PrimaryKey);
            page = await _persistanceService.Get<Page>(expected.PrimaryKey);
            AssertionHelper.AssertPage(expected, page);

            await _persistanceService.Delete(page);
            page = await _persistanceService.Get<Page>(expected.PrimaryKey);
            Assert.Null(page, "page should have been removed");
        }

        [Test]
        public async void InsertAndGetEventPage()
        {
            var expected = Mocks.EventPage;
            Assert.AreEqual(0, expected.PrimaryKey);
            var page = await _persistanceService.Get<EventPage>(expected.PrimaryKey);
            Assert.Null(page, "page is not null");

            await _persistanceService.Insert(expected);
            Assert.AreNotEqual(0, expected.PrimaryKey);
            page = await _persistanceService.Get<EventPage>(expected.PrimaryKey);
            AssertionHelper.AssertEventPage(expected, page);

            await _persistanceService.Delete(page);
            page = await _persistanceService.Get<EventPage>(expected.PrimaryKey);
            Assert.Null(page, "page is not null");
        }

        [Test]
        public async void InsertAndGetMultiplePages()
        {
            var length = 10;
            for (var i = 0; i < length; i++)
            {
                //Mocks.Identifier = i;
                var expected = Mocks.Page;
                await _persistanceService.Insert(expected);
                var actual = await _persistanceService.Get<Page>(expected.PrimaryKey);
                AssertionHelper.AssertPage(expected, actual);
            }

            // check whether or not the "desired" entry exists
            var count = await _persistanceService.Connection.Table<Page>().CountAsync();
            Assert.AreEqual(length, count);

            await _persistanceService.Connection.DeleteAllAsync<Page>();
            count = await _persistanceService.Connection.Table<Page>().CountAsync();
            Assert.AreEqual(0, count);
        }

        [Test]
        public async void InsertAndGetMultipleEventPages()
        {
            var length = 10;
            for (var i = 0; i < length; i++)
            {
                //Mocks.Identifier = i;
                var expected = Mocks.EventPage;
                await _persistanceService.Insert(expected);
                var actual = await _persistanceService.Get<EventPage>(expected.PrimaryKey);
                AssertionHelper.AssertEventPage(expected, actual);
            }

            // check whether or not the "desired" entry exists
            var count = await _persistanceService.Connection.Table<EventPage>().CountAsync();
            Assert.AreEqual(length, count);

            await _persistanceService.Connection.DeleteAllAsync<EventPage>();
            count = await _persistanceService.Connection.Table<EventPage>().CountAsync();
            Assert.AreEqual(0, count);
        }
    }
}