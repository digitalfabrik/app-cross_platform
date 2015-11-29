using System.Collections.Generic;
using Integreat.Models;

namespace Integreat.Shared.Test
{
    public class Mocks
    {
        public static Location Location => new Location
        {
            Id = 2,
            Name = "Augsburg",
            Icon =
                "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/wp-content/uploads/sites/2/2015/10/cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg",
            CityImage =
                "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/wp-content/uploads/sites/2/2015/10/cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg",
            Color = "#FFA000",
            Path = "/wordpress/augsburg/",
            Description = "Augsburg",
            Global = false
        };

        public static Language Language => new Language
        {
            Id = 1,
            ShortName = "en",
            Name = "English",
            IconPath =
                "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/plugins/sitepress-multilingual-cms/res/flags/en.png"
        };

        public static Author Author => new Author
        {
            Login = "langerenken",
            FirstName = "Daniel",
            LastName = "Langerenken"
        };

        public static EventPage EventPage => new EventPage
        {
            Id = 382,
            Title = "Wichtige Links",
            Type = "page",
            Modified = "2015-10-10 11:42:51".DateTimeFromRestString(),
            Content =
                "<p>Bundesamt f\u00fcr Migration: <a href=\"http:\\/\\/www.bamf.de\\/SharedDocs\\/Anlagen\\/DE\\/Publikationen\\/EMN\\/Glossary\\/emn-glossary.pdf?__blob=publicationFile\">http:\\/\\/www.bamf.de\\/SharedDocs\\/Anlagen\\/DE\\/Publikationen\\/EMN\\/Glossary\\/emn-glossary.pdf?__blob=publicationFile<\\/a><\\/p><p><\\/p><p>Projekt \u00bbFirst Steps\u00ab<\\/p><p><a href=\"http:\\/\\/www.first-steps-augsburg.de\">http:\\/\\/www.first-steps-augsburg.de<\\/a><\\/p><p><\\/p><p>Stadt Augsburg mit Verwaltungswegweiser<\\/p><p><a href=\"http:\\/\\/www.augsburg.de\">www.augsburg.de<\\/a><\\/p><p><\\/p><p><span style=\"color: #ff0000\"><strong>bitte gerne erg\u00e4nzen<\\/strong><\\/span><\\/p>",
            Parent = null,
            Thumbnail = "Thumbnail",
            ParentId = 1,
            Order = 62,
            Author = Author,
            AvailableLanguages = AvailableLanguages,
            Event = Event,
            Categories = Categories,
            Tags = Tags,
            Location = EventLocation
        };

        public static Event Event => new Event
        {
            Id = 1,
            StartTime = "2015-10-31 16:00:00".DateTimeFromRestString().Ticks,
            EndTime = "2015-10-31 22:00:00".DateTimeFromRestString().Ticks,
            AllDay = false
        };

        public static EventLocation EventLocation => new EventLocation
        {
            Id = 1,
            Name = "Caf\u00e9 T\u00fcr an T\u00fcr",
            Address = "Wertachstr. 29",
            Town = "Augsburg",
            State = "Bayern",
            Postcode = 88048,
            Region = "Bayern",
            Country = "Deutschland",
            Latitude = 48.378101,
            Longitude = 10.887950
        };

        public static List<EventTag> Tags => new List<EventTag>
        {
            new EventTag
            {
                Id = 1,
                Name = "kochen"
            },
            new EventTag
            {
                Id = 2,
                Name = "esen"
            }
        };

        public static List<EventCategory> Categories => new List<EventCategory>
        {
            new EventCategory
            {
                Id = 1,
                Name = "Category: Kochen"
            },
            new EventCategory
            {
                Id = 2,
                Name = "Category: Essen"
            }
        };

        public static Page Page => new Page
        {
            Id = 382,
            Title = "Wichtige Links",
            Type = "page",
            Modified = "2015-10-10 11:42:51".DateTimeFromRestString(),
            Content =
                "<p>Bundesamt f\u00fcr Migration: <a href=\"http:\\/\\/www.bamf.de\\/SharedDocs\\/Anlagen\\/DE\\/Publikationen\\/EMN\\/Glossary\\/emn-glossary.pdf?__blob=publicationFile\">http:\\/\\/www.bamf.de\\/SharedDocs\\/Anlagen\\/DE\\/Publikationen\\/EMN\\/Glossary\\/emn-glossary.pdf?__blob=publicationFile<\\/a><\\/p><p><\\/p><p>Projekt \u00bbFirst Steps\u00ab<\\/p><p><a href=\"http:\\/\\/www.first-steps-augsburg.de\">http:\\/\\/www.first-steps-augsburg.de<\\/a><\\/p><p><\\/p><p>Stadt Augsburg mit Verwaltungswegweiser<\\/p><p><a href=\"http:\\/\\/www.augsburg.de\">www.augsburg.de<\\/a><\\/p><p><\\/p><p><span style=\"color: #ff0000\"><strong>bitte gerne erg\u00e4nzen<\\/strong><\\/span><\\/p>",
            Parent = null,
            Thumbnail = "Thumbnail",
            ParentId = 1,
            Order = 62,
            Author = Author,
            AvailableLanguages = AvailableLanguages
        };

        public static List<AvailableLanguage> AvailableLanguages => new List<AvailableLanguage>
        {
            new AvailableLanguage("en", 1052),
            new AvailableLanguage("fr", 1374)
        };
    }
}
