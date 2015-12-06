using System;
using System.Threading;
using Integreat.Models;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using NUnit.Framework;

namespace Integreat.Shared.Test.Utilities
{
    [TestFixture]
    internal class PreferencesTest
    {
        private Language _language;
        private Location _location;

        [TestFixtureSetUp]
        public void Before()
        {
            _language = Mocks.Language;
            _location = Mocks.Location;
            Preferences.RemovePage<EventPage>(_language, _location);
            Preferences.RemovePage<Page>(_language, _location);
            Preferences.RemovePage<Disclaimer>(_language, _location);
            Preferences.RemoveLocation();
            Preferences.RemoveLanguage(_location);
        }

        public void SavePageUpdateTime<T> () where T : Page
        {
            Assert.AreEqual(new DateTime(0), Preferences.LastPageUpdateTime<T>(_language, _location));

            var now = DateTime.Now;
            Preferences.SetLastPageUpdateTime<T>(_language, _location);
            var updatedTime = Preferences.LastPageUpdateTime<T>(_language, _location);
            Assert.True(now < updatedTime);

            Thread.Sleep(10);
            Preferences.SetLastPageUpdateTime<T>(_language, _location);
            var newUpdatedTime = Preferences.LastPageUpdateTime<T>(_language, _location);
            Assert.True(updatedTime < newUpdatedTime);

            Preferences.RemovePage<T>(_language, _location);
            Assert.AreEqual(new DateTime(0), Preferences.LastPageUpdateTime<T>(_language, _location));
        }

        [Test]
        public void SaveEventPageUpdateTime()
        {
            SavePageUpdateTime<EventPage>();
        }

        [Test]
        public void SavePageUpdateTime()
        {
            SavePageUpdateTime<Page>();
        }

        [Test]
        public void SavePageDisclaimerTime()
        {
            SavePageUpdateTime<Disclaimer>();
        }

        [Test]
        public void SaveLocationUpdateTime()
        {
            Assert.AreEqual(new DateTime(0), Preferences.LastLocationUpdateTime());

            var now = DateTime.Now;
            Preferences.SetLastLocationUpdateTime();
            var updatedTime = Preferences.LastLocationUpdateTime();
            Assert.True(now < updatedTime);

            Thread.Sleep(10);
            Preferences.SetLastLocationUpdateTime();
            var newUpdatedTime = Preferences.LastLocationUpdateTime();
            Assert.True(updatedTime < newUpdatedTime);

            Preferences.RemoveLocation();
            Assert.AreEqual(new DateTime(0), Preferences.LastLocationUpdateTime());
        }

        [Test]
        public void SaveLanguageUpdateTime()
        {
            Assert.AreEqual(new DateTime(0), Preferences.LastLanguageUpdateTime(_location));

            var now = DateTime.Now;
            Preferences.SetLastLanguageUpdateTime(_location);
            var updatedTime = Preferences.LastLanguageUpdateTime(_location);
            Assert.True(now < updatedTime);

            Thread.Sleep(10);
            Preferences.SetLastLanguageUpdateTime(_location);
            var newUpdatedTime = Preferences.LastLanguageUpdateTime(_location);
            Assert.True(updatedTime < newUpdatedTime);

            Preferences.RemoveLanguage(_location);
            Assert.AreEqual(new DateTime(0), Preferences.LastLanguageUpdateTime(_location));
        }
    }
}
