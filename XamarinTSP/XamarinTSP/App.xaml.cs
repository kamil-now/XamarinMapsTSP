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
            Helper.InvokeOnMainThreadAsync(async () =>
            {
                var permissionGranted = await CheckPermissions(Permission.Location);
                if (!permissionGranted)
                    CloseApp();
            });
            ApiKey = apiKey;
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            var bootstrapper = new Bootstrapper(this);
            bootstrapper.Run();
        }
        public void CloseApp()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        private async Task<bool> CheckPermissions(Permission permission)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission))
                    {
                        return false;
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);

                    if (results.ContainsKey(permission))
                        status = results[permission];
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
