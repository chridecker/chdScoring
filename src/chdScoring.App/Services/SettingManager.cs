using chd.UI.Base.Client.Implementations.Services.Base;
using chd.UI.Base.Contracts.Interfaces.Services;
using chd.UI.Base.Contracts.Interfaces.Services.Base;
using chdScoring.App.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace chdScoring.App.Services
{
    public class SettingManager : BaseClientSettingManager<int, int>, ISettingManager
    {
        private string _mainUrl;
        private int? _judge;
        private bool? _isControlCenter;

        public event EventHandler<string> AutoRedirectToChanged;


        public SettingManager(ILogger<SettingManager> logger,
            IProtecedLocalStorageHandler protecedLocalStorageHandler,
            NavigationManager navigationManager) : base(logger, protecedLocalStorageHandler, navigationManager)
        {
        }
        public Task<string> MainUrl => Task.Run(async () =>
        {
            if (string.IsNullOrWhiteSpace(this._mainUrl))
            {
                this._mainUrl = await this.GetSettingLocal<string>(SettingConstants.BaseAddress);
            }
            return this._mainUrl;
        });

        public Task<bool> IsControlCenter => Task.Run(async () =>
       {
           if (!this._isControlCenter.HasValue)
           {
               this._isControlCenter = await this.GetSettingLocal<bool>(SettingConstants.ControlCenter);
           }
           return this._isControlCenter.Value;
       });

        public async Task UpdateMainUrl(string url)
        {
            this._mainUrl = url;
            await this.StoreSettingLocal<string>(SettingConstants.BaseAddress, url);
        }

        public async Task UpdateControlCenter(bool isControlCenter)
        {
            this._isControlCenter = isControlCenter;
            await this.StoreSettingLocal<bool>(SettingConstants.ControlCenter, isControlCenter);
        }

        public Task<string> GetAutoRedirectTo() => this.GetSettingLocal(SettingConstants.AutoRedirectTo);

        public async Task SetAutoRedirectTo(string value)
        {
            await this.StoreSettingLocal(SettingConstants.AutoRedirectTo, value);
            this.AutoRedirectToChanged?.Invoke(this, value);
        }

    }
    public interface ISettingManager : IBaseClientSettingManager
    {
        Task<string> MainUrl { get; }
        Task UpdateMainUrl(string url);
        Task<bool> IsControlCenter { get; }

        Task UpdateControlCenter(bool isControlCenter);

        event EventHandler<string> AutoRedirectToChanged;

        Task<string> GetAutoRedirectTo();
        Task SetAutoRedirectTo(string value);
    }
}
