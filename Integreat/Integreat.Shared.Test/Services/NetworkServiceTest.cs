using Autofac;
using NUnit.Framework;

namespace Integreat.Shared.Test.Services
{
    [TestFixture]
    internal class NetworkServiceTest
    {
        private IContainer _container;
        private INetworkService _networkService;
        private Language _language;
        private Location _location;
        private UpdateTime _updateTime;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _container = new AppSetup().CreateContainer();
            Assert.True(_container.TryResolve(out _networkService)); //TODO use different url
            _language = new Language{ ShortName = "de" };
            _location = new Location {Path= "/wordpress/augsburg/" };
            _updateTime = new UpdateTime(0);
        }

        [SetUp]
        public void Setup()
        {
        }


        [TearDown]
        public void Tear()
        {
        }

        [Test]
        public async void IsAlive()
        {
            var result = await _networkService.IsServerAlive();
            Assert.NotNull(result);
        }

        [Test]
        public async void LoadLocations()
        {
            var locations = await _networkService.GetLocations();
            Assert.NotNull(locations);
            Assert.True(locations.Count > 0);
           //TODO check size
        }

        [Test]
        public async void LoadLanguages()
        {
            var languages = await _networkService.GetLanguages(_location);
            Assert.NotNull(languages);
            Assert.True(languages.Count > 0);
            //TODO check size
        }

        [Test]
        public async void LoadPages()
        {
            var pages = await _networkService.GetPages(_language, _location, _updateTime);
            Assert.NotNull(pages);
            Assert.True(pages.Count > 0);
            //TODO check size
        }

        [Test]
        public async void LoadEventPages()
        {
            var pages = await _networkService.GetEventPages(_language, _location, _updateTime);
            Assert.NotNull(pages);
            Assert.True(pages.Count > 0);
            //TODO check size
        }


        [Test]
        [Ignore("TODO")]
        public void LoadLocationsEmptyResult()
        {
            Assert.True(false);
        }

        [Test]
        [Ignore("TODO")]
        public void LoadLanguagesEmptyResult()
        {
            Assert.True(false);
        }

        [Test]
        [Ignore("TODO")]
        public void LoadEventPagesEmptyResult()
        {
            Assert.True(false);
        }

        [Test]
        [Ignore("TODO")]
        public void LoadPagesEmptyResult()
        {
            Assert.True(false);
        }

        [Test]
        [Ignore("TODO")]
        public void LoadLocationsNoConnection()
        {
            Assert.True(false);
        }

        [Test]
        [Ignore("TODO")]
        public void LoadLanguagesNoConnection()
        {
            Assert.True(false);
        }

        [Test]
        [Ignore("TODO")]
        public void LoadEventPagesNoConnection()
        {
            Assert.True(false);
        }

        [Test]
        [Ignore("TODO")]
        public void LoadPagesNoConnection()
        {
            Assert.True(false);
        }

        [Test]
        [Ignore("TODO")]
        public async void IsAliveNoConnection()
        {
            Assert.True(false);
        }
    }
}
