using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Integreat.Shared.Test.Utilities
{
    [TestFixture]
    internal class ExtensionsTest
    {
        private DateTime _dt;
        private string _dtString;

        [TestFixtureSetUp]
        public void Before()
        {
            _dt = new DateTime(1991, 4, 15, 16, 30, 42);
            _dtString = "1991-04-15 16:30:42";
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual(_dt.ToRestAcceptableString(), _dtString);
        }

        [Test]
        public void FromStringTest()
        {
            Assert.AreEqual(_dtString.DateTimeFromRestString(), _dt);
        }

        [Test]
        public void ToFromTest()
        {
            Assert.AreEqual(_dtString.DateTimeFromRestString().ToRestAcceptableString().DateTimeFromRestString().ToRestAcceptableString(), _dtString);
        }
    }
}
