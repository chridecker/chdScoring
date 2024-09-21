using Blazored.LocalStorage;
using chdScoring.App.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace chdScoring.App.Services
{
    public class SettingManager : ISettingManager
    {
        private readonly ILocalStorageService _localStorageService;
        private string _mainUrl;
        private int? _judge;
        private bool? _isControlCenter;


        public SettingManager(ILocalStorageService localStorageService)
        {
            this._localStorageService = localStorageService;
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



        public async Task<T> GetSettingLocal<T>(string key)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(await this.GetItemAsync(key), SerializationConstants.JsonOptions);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
        public async Task<string> GetSettingLocal(string key)
        {
            try { return await this.GetItemAsync(key); }
            catch { return string.Empty; }
        }
        public async Task StoreSettingLocal(string key, string value) => await this.SetItemAsync(key, value);
        public async Task StoreSettingLocal<T>(string key, T value) => await this.SetItemAsync(key, JsonSerializer.Serialize<T>(value, SerializationConstants.JsonOptions));

        private ValueTask SetItemAsync(string key, string value) => this._localStorageService.SetItemAsStringAsync(key, value);
        private ValueTask SetItemObject<T>(string key, T value) => this._localStorageService.SetItemAsync(key, value);
        private ValueTask<T> GetItemObject<T>(string key) => this._localStorageService.GetItemAsync<T>(key);
        private ValueTask<string> GetItemAsync(string key) => this._localStorageService.GetItemAsStringAsync(key);
        private ValueTask<IEnumerable<string>> GetKeys() => this._localStorageService.KeysAsync();
    }
    public interface ISettingManager
    {
        Task<string> MainUrl { get; }
        Task UpdateMainUrl(string url);
        Task UpdateJudge(int judge);
        Task<int> Judge { get; }
        Task<bool> IsControlCenter { get; }

        Task StoreSettingLocal(string key, string value);
        Task StoreSettingLocal<T>(string key, T value);
        Task UpdateControlCenter(bool isControlCenter);
    }
}
