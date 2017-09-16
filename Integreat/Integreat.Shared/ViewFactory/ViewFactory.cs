using Autofac;
using Integreat.Shared.ApplicationObjects;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Integreat.Shared.ViewFactory
{
    /// <inheritdoc />
    public class ViewFactory : IViewFactory
    {
        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
        private readonly IComponentContext _componentContext;

        public ViewFactory(IComponentContext componentContext)
        {
            _componentContext = componentContext;
            Instance = this;
        }

        public static IViewFactory Instance { get; private set; }


        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Page
        {
            _map[typeof(TViewModel)] = typeof(TView);
        }

        public Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            return Resolve(out viewModel, setStateAction);
        }


        public Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            var viewType = _map[typeof(TViewModel)];
            viewModel = _componentContext.Resolve<TViewModel>();

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
            var view = _componentContext.Resolve(viewType) as Page;
            if (view == null) { return null; }
            view.BindingContext = viewModel;
            return view;
        }
    }
}