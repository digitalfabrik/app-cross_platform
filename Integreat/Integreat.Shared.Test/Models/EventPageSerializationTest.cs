using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Integreat.Shared.Test.Models
{
    // data from http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/wp-json/extensions/v0/modified_content/events?since=2015-01-01T13%3A04%3A39-0700
    [TestFixture]
    internal class EventPageSerializationTest : PageSerializationTest
    {
        
        private string _serializedPage;

        [SetUp]
        public new void Before()
        {
            var mEventPage = Mocks.EventPage;
            var mEvent = mEventPage.Event;

            var start = new DateTime(mEvent.StartTime).ToRestAcceptableString().Split(' ');
            var end = new DateTime(mEvent.EndTime).ToRestAcceptableString().Split(' ');
            var _event = new Dictionary<string, object>
            {
                {"id", mEvent.Id},
                {"start_date", start[0]},
                {"end_date", end[0]},
                {"all_day", mEvent.AllDay ? "1" : "0"},
                {"start_time", start[1]},
                {"end_time", end[1]}
            };

            var categories = mEventPage.Categories.Select(category => new Dictionary<string, object>
            {
                {"id", category.Id},
                { "name", category.Name}
            }).ToArray();


            var tags = mEventPage.Tags.Select(tag => new Dictionary<string, object>
            {
                {"id", tag.Id},
                { "name", tag.Name}
            }).ToArray();

            var mLocation = mEventPage.Location;
            var location = new Dictionary<string, object>
            {
                {"id", mLocation.Id},
                {"name", mLocation.Name},
                {"address", mLocation.Address},
                {"town", mLocation.Town},
                {"state", mLocation.State},
                {"postcode", mLocation.Postcode},
                {"region", mLocation.Region},
                {"country", mLocation.Country},
                {"latitude", mLocation.Latitude},
                {"longitude", mLocation.Longitude}
            };

            var eventDictionary = new Dictionary<string, object>
            {
                {"categories", categories},
                {"tags", tags},
                {"location", location}
            };
            var pageDictionary = PageDictionary;
            pageDictionary.Add("event", _event);
            pageDictionary.AddRange(eventDictionary);
            _serializedPage = JsonConvert.SerializeObject(pageDictionary);
        }

        [Test]
        public new void DeserializationTest()
        {
            var page = JsonConvert.DeserializeObject<EventPage>(_serializedPage);
            PageDeserializationTest(page);
        }

        [Test]
        public void EventCategoriesTest()
        {
            var expectedCategories = Mocks.Categories;
            var page = JsonConvert.DeserializeObject<EventPage>(_serializedPage);
            var categories = page.Categories;
            Assert.NotNull(categories);
            Assert.True(expectedCategories.Count == categories.Count);
            for (var i = 0; i < expectedCategories.Count; i++)
            {
                Assert.AreEqual(expectedCategories[i].Id, categories[i].Id);
                Assert.AreEqual(expectedCategories[i].Name, categories[i].Name);
            }
        }

        [Test]
        public void EventTagsTest()
        {
            var expectedTags = Mocks.Tags;
            var page = JsonConvert.DeserializeObject<EventPage>(_serializedPage);
            var tags = page.Tags;
            Assert.NotNull(tags);
            Assert.True(expectedTags.Count == tags.Count);
            for (var i = 0; i < expectedTags.Count; i++)
            {
                Assert.AreEqual(expectedTags[i].Id, tags[i].Id);
                Assert.AreEqual(expectedTags[i].Name, tags[i].Name);
            }
        }

        [Test]
        public void EventLocationTest()
        {
            var expectedLocation = Mocks.EventLocation;
            var page = JsonConvert.DeserializeObject<EventPage>(_serializedPage);
            var location = page.Location;
            Assert.NotNull(location);
            Assert.AreEqual(expectedLocation.Id,  location.Id);
            Assert.AreEqual(expectedLocation.Name, location.Name);
            Assert.AreEqual(expectedLocation.Address, location.Address);
            Assert.AreEqual(expectedLocation.Town, location.Town);
            Assert.AreEqual(expectedLocation.State, location.State);
            Assert.AreEqual(expectedLocation.Postcode, location.Postcode);
            Assert.AreEqual(expectedLocation.Region, location.Region);
            Assert.AreEqual(expectedLocation.Country, location.Country);
            Assert.AreEqual(expectedLocation.Latitude, location.Latitude);
            Assert.AreEqual(expectedLocation.Longitude, location.Longitude);
        }

        [Test]
        public void EventTest()
        {
            var expectedEvent = Mocks.Event;
            var page = JsonConvert.DeserializeObject<EventPage>(_serializedPage);
            var mEvent = page.Event;
            Assert.NotNull(mEvent);
            Assert.AreEqual(expectedEvent.Id, mEvent.Id);
            Assert.AreEqual(expectedEvent.AllDay, mEvent.AllDay);
            Assert.AreEqual(expectedEvent.StartTime, mEvent.StartTime);
            Assert.AreEqual(expectedEvent.EndTime, mEvent.EndTime);
        }

    }
}
