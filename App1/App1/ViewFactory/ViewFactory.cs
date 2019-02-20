using System;
using System.Collections.Generic;
using App1.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace App1.ViewFactory
{
    /// <summary>
    /// Factory class for page viewmodels
    /// </summary>
    /// <inheritdoc />
    public class ViewFactory : IViewFactory
    {
        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
        private readonly IComponentContext _componentContext;

        public ViewFactory(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Page
        {
            _map[typeof(TViewModel)] = typeof(TView);
        }

        public Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            var viewType = _map[typeof(TViewModel)];
            var viewModel = _componentContext.Resolve<TViewModel>();

            var resolved = _componentContext.Resolve(viewType);
            var view = resolved as Page;

            setStateAction?.Invoke(viewModel);

            if (view == null) { return null; }
            view.BindingContext = viewModel;
            return view;
        }

        public Page Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var type = viewModel.GetType();
            var viewType = _map[type];
            if (!(_componentContext.Resolve(viewType) is Page view)) { return null; }
            view.BindingContext = viewModel;
            return view;
        }
    }
}