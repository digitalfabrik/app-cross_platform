using System;
using System.Collections.Generic;
using Integreat.Models;

namespace Integreat.Shared.Test
{
    public class Mocks
    {
        public static int Identifier { get; set; }
        
        public static Location Location => new Location
        {
            Id = 2 + Identifier,
            Name = "Augsburg" + Identifier,
            Icon =
                "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/wp-content/uploads/sites/2/2015/10/cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg" + Identifier,
            CityImage =
                "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/wp-content/uploads/sites/2/2015/10/cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg" + Identifier,
            Color = "#FFA000" + Identifier,
            Path = "/wordpress/augsburg/" + Identifier,
            Description = "Augsburg" + Identifier,
            Global = false
        };

        public static Language Language => new Language
        {
            Id = 1 + Identifier,
            ShortName = "en" + Identifier,
            Name = "English" + Identifier,
            IconPath =
                "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/plugins/sitepress-multilingual-cms/res/flags/en.png" + Identifier
        };

        public static Author Author => new Author
        {
            Login = "langerenken" + Identifier,
            FirstName = "Daniel" + Identifier,
            LastName = "Langerenken" + Identifier
        };

        public static EventPage EventPage => new EventPage
        {
            Id = 1 + Identifier,
            Title = "Wichtige Links" + Identifier,
            Type = "page" + Identifier,
            Modified = "2015-10-10 11:42:51".DateTimeFromRestString().AddHours(Identifier),
            Content =
                "<p>Bundesamt f\u00fcr Migration: <a href=\"http:\\/\\/www.bamf.de\\/SharedDocs\\/Anlagen\\/DE\\/Publikationen\\/EMN\\/Glossary\\/emn-glossary.pdf?__blob=publicationFile\">http:\\/\\/www.bamf.de\\/SharedDocs\\/Anlagen\\/DE\\/Publikationen\\/EMN\\/Glossary\\/emn-glossary.pdf?__blob=publicationFile<\\/a><\\/p><p><\\/p><p>Projekt \u00bbFirst Steps\u00ab<\\/p><p><a href=\"http:\\/\\/www.first-steps-augsburg.de\">http:\\/\\/www.first-steps-augsburg.de<\\/a><\\/p><p><\\/p><p>Stadt Augsburg mit Verwaltungswegweiser<\\/p><p><a href=\"http:\\/\\/www.augsburg.de\">www.augsburg.de<\\/a><\\/p><p><\\/p><p><span style=\"color: #ff0000\"><strong>bitte gerne erg\u00e4nzen<\\/strong><\\/span><\\/p>" + Identifier,
            Parent = null,
            Thumbnail = "Thumbnail" +Identifier,
            ParentId = 1 + Identifier,
            Order = 62 + Identifier,
            Author = Author,
            AvailableLanguages = AvailableLanguages,
            Event = Event,
            Categories = Categories,
            Tags = Tags,
            Location = EventLocation
        };

        public static Event Event => new Event
        {
            Id = 1 + Identifier,
            StartTime = "2015-10-31 16:00:00".DateTimeFromRestString().Ticks + Identifier,
            EndTime = "2015-10-31 22:00:00".DateTimeFromRestString().Ticks + Identifier,
            AllDay = false
        };

        public static EventLocation EventLocation => new EventLocation
        {
            Id = 1 + Identifier,
            Name = "Caf\u00e9 T\u00fcr an T\u00fcr" + Identifier,
            Address = "Wertachstr. 29" + Identifier,
            Town = "Augsburg" + Identifier,
            State = "Bayern" + Identifier,
            Postcode = 88048 + Identifier,
            Region = "Bayern" + Identifier,
            Country = "Deutschland" + Identifier,
            Latitude = 48.378101 + Identifier,
            Longitude = 10.887950 + Identifier
        };

        public static List<EventTag> Tags => new List<EventTag>
        {
            new EventTag
            {
                Id = 1+ Identifier,
                Name = "kochen"+ Identifier
            },
            new EventTag
            {
                Id = 2+ Identifier,
                Name = "esen"+ Identifier
            }
        };

        public static List<EventCategory> Categories => new List<EventCategory>
        {
            new EventCategory
            {
                Id = 1+ Identifier,
                Name = "Category: Kochen"+ Identifier
            },
            new EventCategory
            {
                Id = 2+ Identifier,
                Name = "Category: Essen"+ Identifier
            }
        };

        public static Page Page => new Page
        {
            Id = 1 + Identifier,
            Title = "Wichtige Links" + Identifier,
            Type = "page" + Identifier,
            Modified = "2015-10-10 11:42:51".DateTimeFromRestString().AddHours(Identifier),
            Content =
                "<p>Bundesamt f\u00fcr Migration: <a href=\"http:\\/\\/www.bamf.de\\/SharedDocs\\/Anlagen\\/DE\\/Publikationen\\/EMN\\/Glossary\\/emn-glossary.pdf?__blob=publicationFile\">http:\\/\\/www.bamf.de\\/SharedDocs\\/Anlagen\\/DE\\/Publikationen\\/EMN\\/Glossary\\/emn-glossary.pdf?__blob=publicationFile<\\/a><\\/p><p><\\/p><p>Projekt \u00bbFirst Steps\u00ab<\\/p><p><a href=\"http:\\/\\/www.first-steps-augsburg.de\">http:\\/\\/www.first-steps-augsburg.de<\\/a><\\/p><p><\\/p><p>Stadt Augsburg mit Verwaltungswegweiser<\\/p><p><a href=\"http:\\/\\/www.augsburg.de\">www.augsburg.de<\\/a><\\/p><p><\\/p><p><span style=\"color: #ff0000\"><strong>bitte gerne erg\u00e4nzen<\\/strong><\\/span><\\/p>" + Identifier,
            Parent = null,
            Thumbnail = "Thumbnail" + Identifier,
            ParentId = 1 + Identifier,
            Order = 62 + Identifier,
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
