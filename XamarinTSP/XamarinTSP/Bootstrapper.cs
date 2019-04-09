using Autofac;
using Xamarin.Forms;
using XamarinTSP.Abstractions;
using XamarinTSP.UI.ViewModels;
using XamarinTSP.UI.Views;
using XamarinTSP.Utilities;

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

            builder.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
            builder.RegisterType<Navigator>().As<INavigator>().SingleInstance();

            builder.RegisterType<MainPage>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();

            builder.RegisterType<MapViewModel>().SingleInstance();
            builder.RegisterType<LocationListViewModel>().SingleInstance();


            builder.RegisterType<GoogleMapsService>().SingleInstance();

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
        }

    }
}
