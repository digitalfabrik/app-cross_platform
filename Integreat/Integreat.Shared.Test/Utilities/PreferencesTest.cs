using System;
using System.Threading;
using Integreat.Models;
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
            Preferences.RemoveEventPage(_language, _location);
            Preferences.RemovePage(_language, _location);
            Preferences.RemoveDisclaimer(_language, _location);
            Preferences.RemoveLocation();
            Preferences.RemoveLanguage(_location);
        }

        [Test]
        public void SaveEventPageUpdateTime()
        {
            Assert.AreEqual(new DateTime(0), Preferences.LastEventPageUpdateTime(_language, _location));

            var now = DateTime.Now;
            Preferences.SetLastEventPageUpdateTime(_language, _location);
            var updatedTime = Preferences.LastEventPageUpdateTime(_language, _location);
            Assert.True(now < updatedTime);
            
            Thread.Sleep(10);
            Preferences.SetLastEventPageUpdateTime(_language, _location);
            var newUpdatedTime = Preferences.LastEventPageUpdateTime(_language, _location);
            Assert.True(updatedTime < newUpdatedTime);

            Preferences.RemoveEventPage(_language, _location);
            Assert.AreEqual(new DateTime(0), Preferences.LastEventPageUpdateTime(_language, _location));
        }

        [Test]
        public void SavePageUpdateTime()
        {
            Assert.AreEqual(new DateTime(0), Preferences.LastPageUpdateTime(_language, _location));

            var now = DateTime.Now;
            Preferences.SetLastPageUpdateTime(_language, _location);
            var updatedTime = Preferences.LastPageUpdateTime(_language, _location);
            Assert.True(now < updatedTime);

            Thread.Sleep(10);
            Preferences.SetLastPageUpdateTime(_language, _location);
            var newUpdatedTime = Preferences.LastPageUpdateTime(_language, _location);
            Assert.True(updatedTime < newUpdatedTime);

            Preferences.RemovePage(_language, _location);
            Assert.AreEqual(new DateTime(0), Preferences.LastPageUpdateTime(_language, _location));
        }

        [Test]
        public void SavePageDisclaimerTime()
        {
            Assert.AreEqual(new DateTime(0), Preferences.LastPageDisclaimerUpdateTime(_language, _location));

            var now = DateTime.Now;
            Preferences.SetLastPageDisclaimerUpdateTime(_language, _location);
            var updatedTime = Preferences.LastPageDisclaimerUpdateTime(_language, _location);
            Assert.True(now < updatedTime);

            Thread.Sleep(10);
            Preferences.SetLastPageDisclaimerUpdateTime(_language, _location);
            var newUpdatedTime = Preferences.LastPageDisclaimerUpdateTime(_language, _location);
            Assert.True(updatedTime < newUpdatedTime);

            Preferences.RemoveDisclaimer(_language, _location);
            Assert.AreEqual(new DateTime(0), Preferences.LastPageDisclaimerUpdateTime(_language, _location));
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
