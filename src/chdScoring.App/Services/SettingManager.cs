using Blazored.LocalStorage;
using chd.UI.Base.Client.Implementations.Services.Base;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace chdScoring.App.Services
{
    public class SettingManager : BaseClientSettingManager<int, int>, ISettingManager
    {
        private string _mainUrl;
        private int? _judge;
        private bool? _isControlCenter;

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
        public Task<int> Judge => Task.Run(async () =>
       {
           if (!this._judge.HasValue)
           {
               this._judge = await this.GetSettingLocal<int>(SettingConstants.Judge);
           }
           return this._judge.Value;
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
        public async Task UpdateJudge(int judge)
        {
            this._judge = judge;
            await this.StoreSettingLocal<int>(SettingConstants.Judge, judge);
        }

        public async Task UpdateControlCenter(bool isControlCenter)
        {
            this._isControlCenter = isControlCenter;
            await this.StoreSettingLocal<bool>(SettingConstants.ControlCenter, isControlCenter);
        }
    }
    public interface ISettingManager
    {
        Task<string> MainUrl { get; }
        Task UpdateMainUrl(string url);
        Task UpdateJudge(int judge);
        Task<int> Judge { get; }
        Task<bool> IsControlCenter { get; }

        Task UpdateControlCenter(bool isControlCenter);
    }
}
