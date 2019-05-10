using Autofac;
using Xamarin.Forms;
using XamarinTSP.Common.Abstractions;
using XamarinTSP.GoogleMapsApi;
using XamarinTSP.GoogleMapsApi.Abstractions;
using XamarinTSP.TSP;
using XamarinTSP.TSP.Abstractions;
using XamarinTSP.UI.Abstractions;
using XamarinTSP.UI.Models;
using XamarinTSP.UI.Utilities;
using XamarinTSP.UI.ViewModels;
using XamarinTSP.UI.Views;

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


            builder.RegisterType<PMXCrossover>().AsImplementedInterfaces().InstancePerDependency();

            builder.RegisterType<InversionMutation>().AsImplementedInterfaces().InstancePerDependency();

            builder.RegisterType<RouletteSelection>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<TournamentSelection>().AsImplementedInterfaces().InstancePerDependency();


            builder.RegisterType<GoogleMapsService>().As<IGoogleMapsService>().SingleInstance();

            builder.RegisterType<BasicGeneticAlgorithmConfiguration>().As<IBasicGeneticAlgorithmConfiguration>().SingleInstance();
            builder.RegisterType<DistanceMatrixRequestConfiguration>().As<IDistanceMatrixRequestConfiguration>().SingleInstance();

            builder.RegisterType<DurationFitnessFunction>().As<IDurationFitnessFunction>().SingleInstance();
            builder.RegisterType<DistanceFitnessFunction>().As<IDistanceFitnessFunction>().SingleInstance();

            builder.RegisterType<RouteGeneticAlgorithm>().As<IRouteGeneticAlgorithm>().SingleInstance();
            builder.RegisterType<BasicGeneticAlgorithm>().As<IBasicGeneticAlgorithm>().SingleInstance();
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
