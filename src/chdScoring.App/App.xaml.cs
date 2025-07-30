using chd.UI.Base.Contracts.Enum;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.UI.Interfaces;

#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif
using Platform = Microsoft.Maui.ApplicationModel.Platform;

namespace chdScoring.App
{
    public partial class App : Application
    {
        private readonly IAppInfoService _appInfoService;
        private readonly IDeviceDisplayService _deviceDisplay;

        public App(IAppInfoService appInfoService, IDeviceDisplayService deviceDisplay)
        {
            InitializeComponent();
            this._appInfoService = appInfoService;
            this._deviceDisplay = deviceDisplay;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var mainWindow = new Window(new MainPage(this._deviceDisplay));
            mainWindow.Deactivated += (sender, args) => this._appInfoService.AppLifeCycleChanged?.Invoke(this, EAppLifeCycle.OnSleep);
            mainWindow.Resumed += (sender, args) => this._appInfoService.AppLifeCycleChanged?.Invoke(this, EAppLifeCycle.OnResume);

            return mainWindow;
        }
    }
}