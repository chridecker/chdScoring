using Android.App;
using Android.Runtime;

namespace chdScoring.Client
{
    [Application(UsesCleartextTraffic=true)]
    [assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}