using Autofac;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTSP.Abstractions;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinTSP
{
    public partial class App : Application
    {
        private Bootstrapper _bootstrapper;
        public string ApiKey { get; set; }
        public IContainer Container => _bootstrapper?.Container;
        public App(string apiKey)
        {
            ApiKey = apiKey;
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            _bootstrapper = new Bootstrapper(this);
            _bootstrapper.Run();

        }
        public void CloseApp()
        {
            DependencyService.Get<ICloseApp>().CloseApp();
        }

    }
}
