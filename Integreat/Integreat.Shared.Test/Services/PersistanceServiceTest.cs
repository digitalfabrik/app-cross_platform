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
        private PersistanceService _persistanceService;
        private int _id = 1;
        private string _shortName = "de";
        private string _name = "Deutsch";
        private string _iconPath = "iconPath";

        private Location _location;
        private int locationId = 1;
        private string locationName = "Augsburg";
        private string locationIcon = "Icon";
        private string locationPath = "Path";
        private string locationDescription = "AugsburgDesc";
        private bool locationGlobal = true;
        private string locationColor = "5678";
        private string locationCityImage = "My Image";
        private float locationLatitude = 13.37f;
        private float locationLongitude = 42.0f;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _container = Platform.Setup.CreateContainer();
            Assert.True(_container.TryResolve(out _persistanceService), "PersistanceService not found");
            _persistanceService.Init(true);
        }

        [SetUp]
        public void Setup()
        {
            _language = new Language (_id, _shortName, _name, _iconPath);
            _location = new Location(locationId, locationName, locationIcon, locationPath, locationDescription, locationGlobal, locationColor, locationCityImage, locationLatitude, locationLongitude);
        }


        [TearDown]
        public void Tear()
        {
        }

        [Test]
        public async void SaveAndRestoreLocation()
        {
            var location = await _persistanceService.GetLocation(locationId);
            Assert.Null(location, "location is not null");
            await _persistanceService.Insert(_location);
            location = await _persistanceService.GetLocation(_id);
            Assert.NotNull(location, "location is null");
            Assert.AreEqual(_id, location.Id, "location id is not set");
            //TODO
        }
        
        [Test]
        public async void SaveAndRestoreLanguage() //TODO seperate file so that we can preload location..
        {
            var language = await _persistanceService.GetLanguage(_id);
            Assert.Null(language, "language is not null");
            await _persistanceService.Insert(_language);
            language = await _persistanceService.GetLanguage(_id);
            Assert.NotNull(language, "Language is null");
            Assert.AreEqual(_id, language.Id, "language id is not set");
            Assert.AreEqual(_shortName, language.ShortName);
            Assert.AreEqual(_name, language.Name);
            Assert.AreEqual(_iconPath, language.IconPath);
        }

        public async void SaveAndRestoreLanguageWithLocation()
        {
            
        }
    }
}