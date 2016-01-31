using System;
using System.Collections.Generic;
using DLToolkit.Forms.Controls;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
	public partial class LanguagesPage : ContentPage
	{
        private Location location;

        public LanguagesPage(Location location)
        {
            this.location = location;
            Title = "Select Language";
		}
	}
}
