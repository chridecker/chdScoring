using chd.Api.Base.Client.Extensions;
using chd.UI.Base.Client.Implementations.Services.Base;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using chdScoring.App.Interfaces;

namespace chdScoring.App.Services
{
    public class SettingManager : BaseClientSettingManager<int, int>, ISettingManager
    {
        private string _mainUrl;
        private int? _judge;
        private readonly IConfiguration _configuration;

        public event EventHandler<string> AutoRedirectToChanged;


        public SettingManager(ILogger<SettingManager> logger, IConfiguration configuration,
            IProtecedLocalStorageHandler protecedLocalStorageHandler,
            NavigationManager navigationManager) : base(logger, protecedLocalStorageHandler, navigationManager)
        {
            this._configuration = configuration;
        }
        public Task<string> MainUrl => Task.Run(async () =>
        {
            if (string.IsNullOrWhiteSpace(this._mainUrl))
            {
                this._mainUrl = await this.GetSettingLocal<string>(SettingConstants.BaseAddress) ?? 
                this._configuration.GetApiKey("chdScoringApi").ToString();
            }
            return this._mainUrl;
        });



        public async Task UpdateMainUrl(string url)
        {
            this._mainUrl = url;
            await this.StoreSettingLocal<string>(SettingConstants.BaseAddress, url);
        }

        public Task<string> GetAutoRedirectTo() => this.GetSettingLocal(SettingConstants.AutoRedirectTo);

        public async Task SetAutoRedirectTo(string value)
        {
            await this.StoreSettingLocal(SettingConstants.AutoRedirectTo, value);
            this.AutoRedirectToChanged?.Invoke(this, value);
        }

    }
   
}
