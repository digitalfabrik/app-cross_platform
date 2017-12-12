
using Integreat.Shared.Utilities;
using NUnit.Framework;

namespace Integreat.Shared.Test.Services.Loader
{
    [TestFixture]
    internal class LocationLoaderTest
    {

        [TestFixtureSetUp]
        public void BeforeAll()
        {
        }

        [SetUp]
        public void Setup()
        {
            Preferences.RemoveLocation();
        }

        [Test]
        public void SomeTestsHere()
        {
        }

        [TearDown]
        public void Tear()
        {
        }
    }
}