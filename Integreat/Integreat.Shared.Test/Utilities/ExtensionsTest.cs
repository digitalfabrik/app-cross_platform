using System;
using NUnit.Framework;

namespace Integreat.Shared.Test.Utilities
{
    [TestFixture]
    internal class ExtensionsTest
    {
        private DateTime _dt;
        private string _dtStringTo;
        private string _dtStringFrom;

        [TestFixtureSetUp]
        public void Before()
        {
            _dt = new DateTime(1991, 4, 15, 16, 30, 42);
            _dtStringTo = "1991-04-15T16:30:42-7";
            _dtStringFrom = "1991-04-15 16:30:42";
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual(_dt.ToRestAcceptableString(), _dtStringTo);
        }

        [Test]
        public void FromStringTest()
        {
            Assert.AreEqual(_dtStringFrom.DateTimeFromRestString(), _dt);
        }
    }
}
