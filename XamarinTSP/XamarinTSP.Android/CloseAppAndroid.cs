using Xamarin.Forms;
using XamarinTSP.Abstractions;

[assembly: Dependency(typeof(XamarinTSP.Droid.CloseApplication))]
namespace XamarinTSP.Droid
{
    public class CloseApplication : ICloseApp
    {
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}