using chd.Api.Base.Client.Extensions;
using chd.UI.Base.Client.Implementations.Services.Base;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.UI.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Services;

namespace chdScoring.App.Services
{
    public class SettingManager : BaseSettingManager
    {
        private string _mainUrl;
        private int? _judge;
        private readonly IConfiguration _configuration;

        public event EventHandler<string> AutoRedirectToChanged;

        public SettingManager(ILogger<SettingManager> logger, IConfiguration configuration,
            IProtecedLocalStorageHandler protecedLocalStorageHandler,
            NavigationManager navigationManager) : base(logger, configuration, protecedLocalStorageHandler, navigationManager)
        {
            this._configuration = configuration;
        }

        public override T? GetLocalSetting<T>(string key) where T : class
        {
            if (Preferences.ContainsKey(key))
            {
                return Preferences.Default.Get<T>(key, default(T));
            }
            return default(T);
        }

        public override void SetLocalSetting<T>(string key, T value) where T : class
        {
            Preferences.Default.Set<T>(key, value);
        }

    }

}
