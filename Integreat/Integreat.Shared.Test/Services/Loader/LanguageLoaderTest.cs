using System;
using Autofac;
using Integreat.Shared.Models;
using Integreat.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistance;
using Integreat.Shared.Utilities;
using NUnit.Framework;

namespace Integreat.Shared.Test.Services.Loader
{
	[TestFixture]
	internal class LanguageLoaderTest
	{
		private LanguagesLoader _loader;
		private PersistenceService _persistenceService;
		private INetworkService _networkService;
		private Location _location;

		[TestFixtureSetUp]
		public void BeforeAll ()
		{
			var container = Platform.Setup.CreateContainer ();
			Assert.True (container.TryResolve (out _persistenceService), "PersistenceService not found");
			Assert.True (container.TryResolve (out _networkService), "SafeNetworkService not found");
			_location = Mocks.Location;
			_persistenceService.Init ();
			_loader = new LanguagesLoader (_location, _persistenceService, _networkService);
		}

		[SetUp]
		public void Setup ()
		{
			Preferences.RemoveLocation ();
			Preferences.RemoveLanguage (_location);
		}

		[Test]
		public async void TestLoadingSavesInDatabase ()
		{
			await _persistenceService.Insert (_location);
			Assert.AreEqual (new DateTime (), Preferences.LastLanguageUpdateTime (_location));
			Assert.NotNull (_loader);
			var languages = await _loader.Load ();
			var updateTimeChanged = Preferences.LastLanguageUpdateTime (_location);
			Assert.AreNotEqual (new DateTime (), updateTimeChanged);
			Assert.NotNull (languages);

			var languages2 = await _loader.Load ();
			var updateTimeNotChanged = Preferences.LastLanguageUpdateTime (_location);
			Assert.AreEqual (updateTimeChanged, updateTimeNotChanged);
			Assert.AreEqual (languages.Count, languages2.Count);
		}

		[TearDown]
		public void Tear ()
		{
		}
	}
}