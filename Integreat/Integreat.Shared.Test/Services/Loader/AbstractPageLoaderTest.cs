using System;
using Autofac;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistance;
using Integreat.Shared.Utilities;
using NUnit.Framework;

namespace Integreat.Shared.Test.Services.Loader
{
    [TestFixture]
    internal abstract class AbstractPageLoaderTest<T> where T : Page
    {
        private AbstractPageLoader<T> _loader;
        private PersistenceService _persistenceService;
        private INetworkService _networkService;
        private Location _location;
        private Language _language;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            var container = Platform.Setup.CreateContainer();
            Assert.True(container.TryResolve(out _persistenceService), "PersistenceService not found");
            Assert.True(container.TryResolve(out _networkService), "SafeNetworkService not found");
            _location = Mocks.Location;
            _language = Mocks.Language;
            _persistenceService.Init();
            _loader = GetPageLoader(_language, _location, _persistenceService, _networkService);
        }

        public abstract AbstractPageLoader<T> GetPageLoader(Language language, Location location, PersistenceService persistenceService, INetworkService networkService);

        [SetUp]
        public void Setup()
        {
            Preferences.RemoveLocation();
            Preferences.RemoveLanguage(_location);
            Preferences.RemovePage<T>(_language, _location);
        }

        [Test]
        public async void TestLoadingSavesInDatabase()
        {
            await _persistenceService.Insert(_location);
            _language.LocationId = _location.Id;
            _language.Location = _location;
            await _persistenceService.Insert(_language);

            Assert.AreEqual(new DateTime(), Preferences.LastPageUpdateTime<T>(_language, _location));
            Assert.NotNull(_loader);
            var pages = await _loader.Load();
            var updateTimeChanged = Preferences.LastPageUpdateTime<T>(_language, _location);
            Assert.AreNotEqual(new DateTime(), updateTimeChanged);
            Assert.NotNull(pages);

            var pages2 = await _loader.Load();
            var updateTimeNotChanged = Preferences.LastPageUpdateTime<T>(_language, _location);
            Assert.AreEqual(updateTimeChanged, updateTimeNotChanged);
            Assert.AreEqual(pages.Count, pages2.Count);
        }

        [TearDown]
        public void Tear()
        {
        }
    }
}