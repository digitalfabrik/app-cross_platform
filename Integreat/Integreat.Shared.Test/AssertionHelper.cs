using System.Collections.Generic;
using Integreat.Models;
using NUnit.Framework;

namespace Integreat.Shared.Test
{
    public class AssertionHelper
    {
        public static void AssertNullOrNotNull(object a, object b)
        {
            Assert.True(a == null == (b == null), "Null Check not passed.");
        }

        public static void AssertLanguage(Language expected, Language actual)
        {
            AssertNullOrNotNull(expected, actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.ShortName, actual.ShortName);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.IconPath, actual.IconPath);
        }

        public static void AssertLocation(Location expected, Location actual)
        {
            AssertNullOrNotNull(expected, actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Icon, actual.Icon);
            Assert.AreEqual(expected.CityImage, actual.CityImage);
            Assert.AreEqual(expected.Color, actual.Color);
            Assert.AreEqual(expected.Path, actual.Path);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Debug, actual.Debug);
        }

        public static void AssertEventPage(EventPage expected, EventPage actual)
        {
            AssertNullOrNotNull(expected, actual);
            AssertPage(expected, actual);
            AssertEvent(expected.Event, actual.Event);
            AssertEventCategories(expected.Categories, actual.Categories);
            AssertEventTags(expected.Tags, actual.Tags);
            AssertEventLocation(expected.Location, actual.Location);
        }

        public static void AssertPage(Page expected, Page actual)
        {
            AssertNullOrNotNull(expected, actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.Status, actual.Status);
            Assert.AreEqual(expected.Modified, actual.Modified);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Content, actual.Content);
            Assert.AreEqual(expected.ParentId, actual.ParentId);
            Assert.AreEqual(expected.Order, actual.Order);
            Assert.AreEqual(expected.Thumbnail, actual.Thumbnail);
            Assert.AreEqual(expected.AutoTranslated, actual.AutoTranslated);
            AssertAvailableLanguage(expected.AvailableLanguages, actual.AvailableLanguages);
            AssertAuthor(expected.Author, actual.Author);
        }

        public static void AssertAvailableLanguage(List<AvailableLanguage> expected, List<AvailableLanguage> actual)
        {
            AssertNullOrNotNull(expected, actual);
            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Language, actual[i].Language);
                Assert.AreEqual(expected[i].OtherPageId, actual[i].OtherPageId);
                Assert.AreEqual(expected[i].OwnPageId, actual[i].OwnPageId);
            }
        }

        public static void AssertAuthor(Author expected, Author actual)
        {
            AssertNullOrNotNull(expected, actual);
            Assert.AreEqual(expected.Login, actual.Login);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.LastName, actual.LastName);
        }

        public static void AssertEvent(Event expected, Event actual)
        {
            AssertNullOrNotNull(expected, actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.AllDay, actual.AllDay);
            Assert.AreEqual(expected.StartTime, actual.StartTime);
            Assert.AreEqual(expected.EndTime, actual.EndTime);
        }

        public static void AssertEventLocation(EventLocation expected, EventLocation actual)
        {
            AssertNullOrNotNull(expected, actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Address, actual.Address);
            Assert.AreEqual(expected.Town, actual.Town);
            Assert.AreEqual(expected.State, actual.State);
            Assert.AreEqual(expected.Postcode, actual.Postcode);
            Assert.AreEqual(expected.Region, actual.Region);
            Assert.AreEqual(expected.Country, actual.Country);
            Assert.AreEqual(expected.Latitude, actual.Latitude);
            Assert.AreEqual(expected.Longitude, actual.Longitude);
        }

        public static void AssertEventTags(List<EventTag> expected, List<EventTag> actual)
        {
            AssertNullOrNotNull(expected, actual);
            Assert.True(expected.Count == actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
                Assert.AreEqual(expected[i].Name, actual[i].Name);
            }
        }

        public static void AssertEventCategories(List<EventCategory> expected, List<EventCategory> actual)
        {
            AssertNullOrNotNull(expected, actual);
            Assert.True(expected.Count == actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
                Assert.AreEqual(expected[i].Name, actual[i].Name);
            }
        }
    }
}
