using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Integreat.Shared.Test.Models
{
    // data from http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/wp-json/extensions/v0/modified_content/events?since=2015-01-01T13%3A04%3A39-0700
    [TestFixture]
    internal class EventPageSerializationTest : PageSerializationTest
    {
        private string locationId = "1";
        private string locationName = "Caf\u00e9 T\u00fcr an T\u00fcr";
        private string locationAddress = "Wertachstr. 29";
        private string locationTown = "Augsburg";
        private string locationState = "Bayern";
        private string locationPostcode = "86159 ";
        private string locationRegion = null;
        private string locationCountry = "DE";
        private string locationLatitude = "48.378101";
        private string locationLongitude = "10.887950";

        private string tagId = "1";
        private string tagName = "Essen";

        private string categoryId = "53";
        private string categoryName = "Kochen";

        private int eventId = 1;
        private string startDate = "2015-10-31";
        private string endDate = "2015-10-31";
        private string allDay = "0";
        private string startTime = "16:00:00";
        private string endTime = "22:00:00";

        private string _serializedPage;

        [SetUp]
        public new void Before()
        {
            var _event = new Dictionary<string, object>
            {
                {"id", eventId},
                {"start_date", startDate},
                {"end_date", endDate},
                {"all_day", allDay},
                {"start_time", startTime},
                {"end_time", endTime}
            };

            Dictionary<string, object>[] categories =
            {
                new Dictionary<string, object>
                {
                    {"id", categoryId},
                    {"name", categoryName}
                }
            };

            Dictionary<string, object>[] tags =
            {
                new Dictionary<string, object>
                {
                    {"id",tagId},
                    {"name", tagName}
                }
            };

            var location = new Dictionary<string, object>
            {
                {"id", locationId},
                {"name", locationName},
                {"address", locationAddress},
                {"town", locationTown},
                {"state", locationState},
                {"postcode", locationPostcode},
                {"region", locationRegion},
                {"country", locationCountry},
                {"latitude", locationLatitude},
                {"longitude", locationLongitude}
            };

            var eventDictionary = new Dictionary<string, object>
            {
                {"categories", categories},
                {"tags", tags},
                {"location", location}
            };
            eventDictionary.AddRange(_event);
            var pageDictionary = PageDictionary();
            pageDictionary.Add("event", eventDictionary);
            _serializedPage = JsonConvert.SerializeObject(pageDictionary);
        }

        [Test]
        public new void DeserializationTest()
        {
            var page = JsonConvert.DeserializeObject<EventPage>(_serializedPage);
            PageDeserializationTest(page);
            EventTest(page.Event);
            EventLocationTest(page.Location);
            EventTagsTest(page.Tags);
            EventCategoriesTest(page.Categories);
        }

        private void EventCategoriesTest(Collection<EventCategory> categories)
        {
            Assert.True(1 == categories.Count);
            Assert.AreEqual(categoryId, categories[0].Id);
            Assert.AreEqual(categoryName, categories[0].Name);
        }

        private void EventTagsTest(Collection<EventTag> tags)
        {
            Assert.True(1 == tags.Count);
            Assert.AreEqual(tagName, tags[0].Name);
        }

        private void EventLocationTest(EventLocation location)
        {
            Assert.AreEqual(locationId, location.Id);
            Assert.AreEqual(locationName, location.Name);
            Assert.AreEqual(locationAddress, location.Address);
            Assert.AreEqual(locationTown, location.Town);
            Assert.AreEqual(locationState, location.State);
            Assert.AreEqual(locationPostcode, location.Postcode);
            Assert.AreEqual(locationRegion, location.Region);
            Assert.AreEqual(locationCountry, location.Country);
            Assert.AreEqual(locationLatitude, location.Latitude);
            Assert.AreEqual(locationLongitude, location.Longitude);
        }

        private void EventTest(Event mEvent)
        {
            Assert.AreEqual(eventId, mEvent.Id);
            Assert.AreEqual(int.Parse(allDay), mEvent.AllDay ? 1 : 0);
            Assert.AreEqual("2015-10-31 16:00:00".DateTimeFromRestString().Ticks, mEvent.StartTime);
            Assert.AreEqual("2015-10-31 22:00:00".DateTimeFromRestString().Ticks, mEvent.EndTime);
        }
        
    }
}
