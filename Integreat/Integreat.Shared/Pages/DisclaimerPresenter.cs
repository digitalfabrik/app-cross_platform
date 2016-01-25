using System;
using Integreat.Shared.Services.Loader;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using Integreat.Shared.Models;

namespace Integreat.Shared
{
	public class DisclaimerPresenter
	{
		public DisclaimerLoader DisclaimerLoader;
		// TODO: necessary? relict from copying stuff from a ContentPage
		private bool IsBusy;
		private List<Disclaimer> _pages;
		private string _content;

		public async Task LoadDisclaimer ()
		{
			Console.WriteLine ("Load Disclaimer called");
			if (IsBusy) {
				return;
			}
			IsBusy = true;
			LoadDisclaimerCommand.ChangeCanExecute ();
			try {
				_pages = await DisclaimerLoader.Load ();
				Console.WriteLine ("Disclaimer received:" + _pages.Count);
				_content = prepare (_pages);
			} catch (Exception e) {
				var page = new ContentPage ();
				await page.DisplayAlert ("Error", "Unable to load disclaimer: " + e.Message, "OK");
			} finally {
				IsBusy = false;
				LoadDisclaimerCommand.ChangeCanExecute ();
			}
			Console.WriteLine ("LoadDisclaimer stopped");
		}

		public async void Show ()
		{
			var page = new ContentPage ();
			await page.DisplayAlert ("Disclaimer", _content, "OK");
		}

		string prepare (List<Disclaimer> _pages)
		{
			string content = "";
			foreach (Disclaimer page in _pages) {
				content += page.Title + "\n" + page.Content + "\n\n";
			}
			if (string.IsNullOrEmpty (content)) {
				content = "-";
			}
			return content;
		}

		private Command _loadDisclaimerCommand;

		public Command LoadDisclaimerCommand {
			get {
				return _loadDisclaimerCommand ??
				(_loadDisclaimerCommand = new Command (async () => {
					await LoadDisclaimer ();
				}, () => !IsBusy));
			}
		}
	}
}

