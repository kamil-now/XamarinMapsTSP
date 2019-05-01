using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinTSP.UI.Abstractions;

namespace XamarinTSP.UI.Utilities
{
    public class Navigator : INavigator
    {
        private readonly Lazy<INavigation> navigation;
        private readonly IViewFactory viewFactory;
        private INavigation Navigation => navigation.Value;

        public Navigator(Lazy<INavigation> navigation, IViewFactory viewFactory)
        {
            this.navigation = navigation;
            this.viewFactory = viewFactory;
        }

        public async Task PopAsync() => await Navigation.PopAsync();

        public async Task PopToRootAsync() => await Navigation.PopToRootAsync();

        public async Task PushAsync<TViewModel>() where TViewModel : class, IViewModel
        {
            var view = viewFactory.Resolve<TViewModel>();
            await Navigation.PushAsync(view);
        }

        public async Task PushModalAsync<TViewModel>() where TViewModel : class, IViewModel
        {
            var view = viewFactory.Resolve<TViewModel>();
            await Navigation.PushModalAsync(view);
        }
    }
}
