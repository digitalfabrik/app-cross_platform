using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Integreat.Shared.ViewFactory
{
    /// <summary>
    /// IPopupViewFactory interface.
    /// </summary>
    public interface IPopupViewFactory
    {
        void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : PopupPage;

        PopupPage Resolve<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        PopupPage Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;
    }
}