using Autofac;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XamarinTSP.Abstractions;

namespace XamarinTSP.Utilities
{
    public class ViewFactory : IViewFactory
    {
        private readonly IDictionary<Type, Type> map = new Dictionary<Type, Type>();
        private readonly IComponentContext componentContext;

        public ViewFactory(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }

        public void Register<TViewModel, TView>() where TViewModel : class, IViewModel where TView : Page
        {
            map[typeof(TViewModel)] = typeof(TView);
        }

        public Page Resolve<TViewModel>() where TViewModel : class, IViewModel
        {
            var viewModel = componentContext.Resolve<TViewModel>();
            var viewType = map[typeof(TViewModel)];
            var view = componentContext.Resolve(viewType) as Page;

            view.BindingContext = viewModel;
            return view;
        }
    }
}
