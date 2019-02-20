using System;
using App1.ViewModels;
using Xamarin.Forms;

namespace App1.ViewFactory
{
    /// <summary>
    /// IViewFactory interface.
    /// </summary>
    public interface IViewFactory
    {
        void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Page;

        Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Page Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;
    }
}