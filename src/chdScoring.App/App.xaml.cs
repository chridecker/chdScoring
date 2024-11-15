using chd.UI.Base.Contracts.Enum;
using chd.UI.Base.Contracts.Interfaces.Services;
#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif
using Platform = Microsoft.Maui.ApplicationModel.Platform;

namespace chdScoring.App
{
    public partial class App : Application
    {
        private readonly IAppInfoService _appInfoService;

        public App(IAppInfoService appInfoService)
        {
            InitializeComponent();

            this.MainPage = new MainPage();
            this._appInfoService = appInfoService;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {

        var col = Color.FromRgba("#181B1F");
            col.ToRgba(out var r, out var g,out var b, out var a);
#if ANDROID
        Platform.CurrentActivity.Window.SetNavigationBarColor(Android.Graphics.Color.Argb(a,r,g,b));
#endif

            var mainWindow = base.CreateWindow(activationState);
#if WINDOWS
                 mainWindow.Width = 370;
                mainWindow.Height = 825;
#endif
        mainWindow.Deactivated += (sender, args) => this._appInfoService.AppLifeCycleChanged?.Invoke(this, EAppLifeCycle.OnSleep);
        mainWindow.Resumed += (sender, args) => this._appInfoService.AppLifeCycleChanged?.Invoke(this, EAppLifeCycle.OnResume);

            return mainWindow;
        }
}
}