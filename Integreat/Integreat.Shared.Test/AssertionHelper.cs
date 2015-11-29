using System.Collections.Generic;
using Integreat.Models;
using NUnit.Framework;

namespace Integreat.Shared.Test
{
    public class AssertionHelper
    {
        public static void AssertLanguage(Language expected, Language actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.ShortName, actual.ShortName);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.IconPath, actual.IconPath);
        }

        public static void AssertLocation(Location expected, Location actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Icon, actual.Icon);
            Assert.AreEqual(expected.CityImage, actual.CityImage);
            Assert.AreEqual(expected.Color, actual.Color);
            Assert.AreEqual(expected.Path, actual.Path);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Global, actual.Global);
        }

        public static void AssertEventPage(EventPage expected, EventPage page)
        {
            AssertPage(expected, page);
            AssertEvent(expected.Event, page.Event);
            AssertEventCategories(expected.Categories, page.Categories);
            AssertEventTags(expected.Tags, page.Tags);
            AssertEventLocation(expected.Location, page.Location);
        }

        public static void AssertPage(Page exepected, Page actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(exepected.Id, actual.Id);
            Assert.AreEqual(exepected.Title, actual.Title);
            Assert.AreEqual(exepected.Type, actual.Type);
            Assert.AreEqual(exepected.Status, actual.Status);
            Assert.AreEqual(exepected.Modified, actual.Modified);
            Assert.AreEqual(exepected.Description, actual.Description);
            Assert.AreEqual(exepected.Content, actual.Content);
            Assert.AreEqual(exepected.ParentId, actual.ParentId);
            Assert.AreEqual(exepected.Order, actual.Order);
            Assert.AreEqual(exepected.Thumbnail, actual.Thumbnail);
            AssertAvailableLanguage(exepected.AvailableLanguages, actual.AvailableLanguages);
            AssertAuthor(exepected.Author, actual.Author);
        }

        public static void AssertAvailableLanguage(List<AvailableLanguage> expected, List<AvailableLanguage> actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Language, actual[i].Language);
                Assert.AreEqual(expected[i].OtherPageId, actual[i].OtherPageId);
            }
        }

        public static void AssertAuthor(Author expected, Author actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Login, actual.Login);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.LastName, actual.LastName);
        }

        public static void AssertEvent(Event expected, Event actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.AllDay, actual.AllDay);
            Assert.AreEqual(expected.StartTime, actual.StartTime);
            Assert.AreEqual(expected.EndTime, actual.EndTime);
        }

        public static void AssertEventLocation(EventLocation expected, EventLocation actual)
        {
            Assert.NotNull(actual);
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
            Assert.NotNull(actual);
            Assert.True(expected.Count == actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
                Assert.AreEqual(expected[i].Name, actual[i].Name);
            }
        }

        public static void AssertEventCategories(List<EventCategory> expected, List<EventCategory> actual)
        {
            Assert.NotNull(actual);
            Assert.True(expected.Count == actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
                Assert.AreEqual(expected[i].Name, actual[i].Name);
            }
        }
    }
}
