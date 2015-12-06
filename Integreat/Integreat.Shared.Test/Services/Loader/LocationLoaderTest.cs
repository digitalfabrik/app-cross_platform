using System;
using Autofac;
using Integreat.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistance;
using Integreat.Shared.Utilities;
using NUnit.Framework;

namespace Integreat.Shared.Test.Services.Loader
{
    [TestFixture]
    internal class LocationLoaderTest
    {
        private LocationLoader _loader;
        private PersistenceService _persistenceService;
        private INetworkService _networkService;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            var container = Platform.Setup.CreateContainer();
            Assert.True(container.TryResolve(out _persistenceService), "PersistenceService not found");
            Assert.True(container.TryResolve(out _networkService), "NetworkService not found");
            _persistenceService.Init();
            _loader = new LocationLoader(_persistenceService, _networkService);
        }

        [SetUp]
        public void Setup()
        {
            Preferences.RemoveLocation();
        }

        [Test]
        public async void TestLoadingSavesInDatabase()
        {
            Assert.AreEqual(new DateTime(), Preferences.LastLocationUpdateTime());
            var locations = await _loader.Load();
            var updateTimeChanged = Preferences.LastLocationUpdateTime();
            Assert.AreNotEqual(new DateTime(), updateTimeChanged);
            Assert.NotNull(locations);

            var locations2 = await _loader.Load();
            var updateTimeNotChanged = Preferences.LastLocationUpdateTime();
            Assert.AreEqual(updateTimeChanged, updateTimeNotChanged);
            Assert.AreEqual(locations.Count, locations2.Count);
        }

        [TearDown]
        public void Tear()
        {
        }
    }
}