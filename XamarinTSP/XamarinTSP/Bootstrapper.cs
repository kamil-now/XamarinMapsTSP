using Autofac;
using Xamarin.Forms;
using XamarinTSP.Common.Abstractions;
using XamarinTSP.GoogleMapsApi;
using XamarinTSP.TSP;
using XamarinTSP.TSP.Common.Abstractions;
using XamarinTSP.UI;
using XamarinTSP.UI.Models;
using XamarinTSP.UI.CustomControls;
using XamarinTSP.UI.ViewModels;
using XamarinTSP.UI.Views;
using XamarinTSP.UI.Abstractions;
using XamarinTSP.UI.Utilities;

namespace XamarinTSP
{
    public class Bootstrapper
    {
        private readonly App app;
        public IContainer Container { get; private set; }
        public Bootstrapper(App app)
        {
            this.app = app;
        }

        public void Run()
        {
            Container = BuildContainer();

            ConfigureApplication(Container);
        }
        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => Application.Current.MainPage.Navigation).SingleInstance();

            builder.Register(geolocation => DependencyService.Get<IGeolocationService>()).As<IGeolocationService>().SingleInstance();
            builder.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
            builder.RegisterType<Navigator>().As<INavigator>().SingleInstance();

            builder.RegisterType<MainPage>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();

            builder.RegisterType<LocationListPage>().SingleInstance();
            builder.RegisterType<LocationListViewModel>().SingleInstance();

            builder.RegisterType<ConfigurationPage>().SingleInstance();
            builder.RegisterType<ConfigurationViewModel>().SingleInstance();

            builder.RegisterType<MapViewModel>().AsSelf().SingleInstance();

            builder.RegisterType<LocationList>().AsSelf().SingleInstance();
            builder.RegisterType<GoogleMapsService>().AsSelf().SingleInstance();

            builder.RegisterType<TSPConfiguration>().As<ITSPConfiguration>().SingleInstance();
            builder.RegisterType<TSPAlgorithm>().As<ITSPAlgorithm>().SingleInstance();

            return builder.Build();
        }

        private void ConfigureApplication(IContainer container)
        {
            var viewFactory = container.Resolve<IViewFactory>();
            MapViews(viewFactory);

            var mainPage = viewFactory.Resolve<MainViewModel>();
            var navPage = new NavigationPage(mainPage);

            app.MainPage = navPage;
        }
        private void MapViews(IViewFactory viewFactory)
        {
            viewFactory.Register<MainViewModel, MainPage>();
            viewFactory.Register<LocationListViewModel, LocationListPage>();
            viewFactory.Register<ConfigurationViewModel, ConfigurationPage>();
        }

    }
}
