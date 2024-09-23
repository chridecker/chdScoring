using Microsoft.AspNetCore.Components;
using chdScoring.App.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.App.Constants;

namespace chdScoring.App.Pages
{
    public partial class Settings
    {
        [Inject] private ISettingManager _settingManager { get; set; }
        [Inject] private IMainService _mainService { get; set; }

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private string _baseAddress = string.Empty;
        private bool _autocollapseNav = false;
        private bool _isControlCenter = false;


        protected override async Task OnInitializedAsync()
        {
            this._baseAddress = await this._settingManager.MainUrl;

            this._autocollapseNav = await this._settingManager.GetSettingLocal<bool>(SettingConstants.AutoCollapseNavbar_Key); ;
            this._isControlCenter = await this._settingManager.IsControlCenter;

            await base.OnInitializedAsync();
        }
        private Task UpdateMainUrl(ChangeEventArgs e) => this._settingManager.UpdateMainUrl((string)e.Value);

        private Task UpdateAutoCollapeseNavBar(ChangeEventArgs e) => this._settingManager.StoreSettingLocal<bool>(SettingConstants.AutoCollapseNavbar_Key, (bool)e.Value);
        private Task UpdateIsControlCenter(ChangeEventArgs e) => this._settingManager.UpdateControlCenter((bool)e.Value);
    }
}
