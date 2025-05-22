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
            this._appInfoService = appInfoService;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var mainWindow = new Window(new MainPage());
            mainWindow.Deactivated += (sender, args) => this._appInfoService.AppLifeCycleChanged?.Invoke(this, EAppLifeCycle.OnSleep);
            mainWindow.Resumed += (sender, args) => this._appInfoService.AppLifeCycleChanged?.Invoke(this, EAppLifeCycle.OnResume);

            return mainWindow;
        }
    }
}