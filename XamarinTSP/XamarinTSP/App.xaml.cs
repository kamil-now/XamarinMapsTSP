using Autofac;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTSP.Common.Abstractions;

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
            InitializeComponent();

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
        public static Task InvokeOnMainThreadAsync(Action action, int delay = 0)
        {
            var task = new TaskCompletionSource<object>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (delay > 0)
                    {
                        await Task.Delay(delay);
                    }
                    action();
                    task.SetResult(null);
                }
                catch (Exception ex)
                {
                    task.SetException(ex);
                }
            }); return task.Task;
        }
    }
}
