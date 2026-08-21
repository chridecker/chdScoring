using chd.Api.Base.Client.Extensions;
using chd.UI.Base.Client.Implementations.Services.Base;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Services
{
    public abstract class BaseSettingManager : BaseClientSettingManager<int, int>, ISettingManager
    {
        protected string _mainUrl;
        protected string _apiKey;
        private readonly IConfiguration _configuration;

        public event EventHandler<string> AutoRedirectToChanged;

        public BaseSettingManager(ILogger<BaseSettingManager> logger, IConfiguration configuration,
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
        public Task<string> ApiKey => Task.Run(async () =>
        {
            if (string.IsNullOrWhiteSpace(this._apiKey))
            {
                this._apiKey = await this.GetSettingLocal<string>(SettingConstants.ApiKey) ??
                               this._configuration.GetSection("ApiKey").Value;
            }
            return this._apiKey;
        });

        public bool IsiOS => this._isiOS();

        protected abstract bool _isiOS();

        public async Task UpdateMainUrl(string url)
        {
            this._mainUrl = url;
            await this.StoreSettingLocal<string>(SettingConstants.BaseAddress, url);
        }
        public async Task UpdateApiKey(string url)
        {
            this._apiKey = url;
            await this.StoreSettingLocal<string>(SettingConstants.ApiKey, url);
        }


        public Task<string> GetAutoRedirectTo() => this.GetSettingLocal(SettingConstants.AutoRedirectTo);

        public async Task SetAutoRedirectTo(string value)
        {
            await this.StoreSettingLocal(SettingConstants.AutoRedirectTo, value);
            this.AutoRedirectToChanged?.Invoke(this, value);
        }

        public abstract T? GetNativSetting<T>(string key) where T : class;


        public abstract void SetNativSetting<T>(string key, T value) where T : class;

        public Task<int> GetScoringZoom() => this.GetSettingLocal<int>(SettingConstants.ScoringZoom);
        public Task<bool> GetUseJudgeConfirmQuestion() => this.GetSettingLocal<bool>(SettingConstants.Use_JudgeConfirm_Question);

        public abstract Task ShowToast(string message, CancellationToken cancellationToken = default);

        public abstract void CloseApp();
    }
}
