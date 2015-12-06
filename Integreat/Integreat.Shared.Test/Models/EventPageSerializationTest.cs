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

            var start = new DateTime(mEvent.StartTime).ToRestAcceptableString().Split('T');
            var end = new DateTime(mEvent.EndTime).ToRestAcceptableString().Split('T');
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
            AssertionHelper.AssertPage(Mocks.Page, page);
        }

        [Test]
        public void EventCategoriesTest()
        {
            var page = JsonConvert.DeserializeObject<EventPage>(_serializedPage);
            AssertionHelper.AssertEventCategories(Mocks.Categories, page.Categories);
        }

        [Test]
        public void EventTagsTest()
        {
            var page = JsonConvert.DeserializeObject<EventPage>(_serializedPage);
            AssertionHelper.AssertEventTags(Mocks.Tags, page.Tags);
        }

        [Test]
        public void EventLocationTest()
        {
            var page = JsonConvert.DeserializeObject<EventPage>(_serializedPage);
            AssertionHelper.AssertEventLocation(Mocks.EventLocation, page.Location);
        }

        [Test]
        public void EventTest()
        {
            var page = JsonConvert.DeserializeObject<EventPage>(_serializedPage);
            AssertionHelper.AssertEvent(Mocks.Event, page.Event);
        }

    }
}
