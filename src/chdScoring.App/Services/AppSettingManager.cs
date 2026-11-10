using chd.Api.Base.Client.Extensions;
using chd.UI.Base.Client.Implementations.Services.Base;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.UI.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace chdScoring.App.Services
{
    public abstract class AppSettingManager(
        ILogger<AppSettingManager> logger,
        IBatteryService batteryService,
        IConfiguration configuration, 
        IProtecedLocalStorageHandler protecedLocalStorageHandler,
        NavigationManager navigationManager)
        : BaseSettingManager(logger, configuration, protecedLocalStorageHandler, navigationManager)
    {
        public override T? GetNativSetting<T>(string key) where T : class
        {
            if (Preferences.ContainsKey(key))
            {
                return Preferences.Default.Get<T>(key, default(T));
            }
            return default(T);
        }
        public override void CloseApp() => Application.Current.Quit();
        public override Task ShowToast(string message, CancellationToken cancellationToken = default) => Toast.Make(message).Show();
        public override void SetNativSetting<T>(string key, T value) where T : class
        {
            Preferences.Default.Set<T>(key, value);
        }

        public override async Task UpdateMainUrl(string url)
        {
            if (this._mainUrl != url)
            {
                await base.UpdateMainUrl(url);
                var snack = Snackbar.Make("Main URL changed. You need to RESTART the app!",
                    async ()=>await batteryService.ExitApplication(true),
                    "Restart",
                    TimeSpan.FromSeconds(5));
                await snack.Show();
            }
        }


    }

}
