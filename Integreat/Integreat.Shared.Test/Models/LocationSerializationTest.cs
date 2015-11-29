using System.Collections.Generic;
using Integreat.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Integreat.Shared.Test.Models
{
    // data from http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/wp-json/extensions/v0/multisites
    [TestFixture]
    internal class LocationSerializationTest
    {
        private string _serializedLocation;

        [SetUp]
        public void Before()
        {
            var location = Mocks.Location;
            var  locationDictionary = new Dictionary<string, object>
            {
                {"id", location.Id},
                {"name", location.Name},
                {"icon", location.Icon},
                {"cover_image", location.CityImage},
                {"color", location.Color},
                {"path", location.Path},
                {"description", location.Description},
                {"global", location.Global},
            };
            _serializedLocation = JsonConvert.SerializeObject(locationDictionary);
        }

        [Test]
        public void DeserializationTest()
        {
            AssertionHelper.AssertLocation(Mocks.Location, JsonConvert.DeserializeObject<Location>(_serializedLocation));
        }
    }
}
