﻿using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
	public partial class MainPage : MasterDetailPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
            var vm = BindingContext as BaseViewModel;
	        vm?.OnAppearingCommand.Execute(null);
	    }
	}
}