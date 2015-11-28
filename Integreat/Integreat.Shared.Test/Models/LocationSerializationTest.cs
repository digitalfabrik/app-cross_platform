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
        private int _id = 2;
        private string _name = "Augsburg";
        private string _icon = "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/wp-content/uploads/sites/2/2015/10/cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg";
        private string _cover_image = "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/wp-content/uploads/sites/2/2015/10/cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg";
        private string _color = "#FFA000";
        private string _path = "/wordpress/augsburg/";
        private string _description = "Augsburg";
        private bool _global = false;

        private string _serializedLocation;

        [SetUp]
        public void Before()
        {
            var  locationDictionary = new Dictionary<string, object>
            {
                {"id", _id},
                {"name", _name},
                {"icon", _icon},
                {"cover_image", _cover_image},
                {"color", _color},
                {"path", _path},
                {"description", _description},
                {"global", _global},
            };
            _serializedLocation = JsonConvert.SerializeObject(locationDictionary);
        }

        [Test]
        public void DeserializationTest()
        {
            var location = JsonConvert.DeserializeObject<Location>(_serializedLocation);
            Assert.AreEqual(_id, location.Id);
            Assert.AreEqual(_name, location.Name);
            Assert.AreEqual(_icon, location.Icon);
            Assert.AreEqual(_cover_image, location.CityImage);
            Assert.AreEqual(_color, location.Color);
            Assert.AreEqual(_path, location.Path);
            Assert.AreEqual(_description, location.Description);
            Assert.AreEqual(_global, location.Global);
        }
    }
}
