using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTSP.Utilities;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinTSP
{
    public partial class App : Application
    {
        public string ApiKey { get; set; }
        public App(string apiKey)
        {
            Helper.InvokeOnMainThreadAsync(async () => { if (!await CheckPermissions()) System.Diagnostics.Process.GetCurrentProcess().Kill(); });

            ApiKey = apiKey;
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            var bootstrapper = new Bootstrapper(this);
            bootstrapper.Run();
        }
        private async Task<bool> CheckPermissions()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        return false;
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                return status == PermissionStatus.Granted;
            }
            catch
            {
                return false;
            }
        }
    }
}
