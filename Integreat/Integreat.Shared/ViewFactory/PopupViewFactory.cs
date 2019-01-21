using Autofac;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Integreat.Shared.ViewFactory
{
    /// <summary>
    /// Factory class for popuppage viewmodels
    /// </summary>
    /// <inheritdoc />
    public class PopupViewFactory : IPopupViewFactory
    {
        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
        private readonly IComponentContext _componentContext;

        public PopupViewFactory(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : PopupPage
        {
            _map[typeof(TViewModel)] = typeof(TView);
        }

        public PopupPage Resolve<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            var viewType = _map[typeof(TViewModel)];
            var viewModel = _componentContext.Resolve<TViewModel>();

            var resolved = _componentContext.Resolve(viewType);
            var view = resolved as PopupPage;

            setStateAction?.Invoke(viewModel);

            if (view == null) { return null; }
            view.BindingContext = viewModel;
            return view;
        }

        public PopupPage Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var type = viewModel.GetType();
            var viewType = _map[type];
            if (!(_componentContext.Resolve(viewType) is PopupPage view)) { return null; }
            view.BindingContext = viewModel;
            return view;
        }
    }
}