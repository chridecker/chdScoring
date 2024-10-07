    using Microsoft.AspNetCore.Components;
using chdScoring.App.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.App.Constants;
using chd.UI.Base.Components.Base;

namespace chdScoring.App.Pages
{
    public partial class Settings : PageComponentBase<int, int>
    {
        [Inject] private ISettingManager _settingManager { get; set; }

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private string _baseAddress = string.Empty;
        private bool _autocollapseNav = false;
        private string _autoRedirect;

        private Dictionary<string, string> _redirectOptions = new()
        {
            {"", PageTitleConstants.Index},
            {"controlcenter", PageTitleConstants.ControlCenter },
            {"scoring", PageTitleConstants.Scoring },
            {"competitionmanagement", PageTitleConstants.CompetitionManagement },
        };

        protected override async Task OnInitializedAsync()
        {
            this.Title = PageTitleConstants.Settings;

            this._baseAddress = await this._settingManager.MainUrl;

            this._autocollapseNav = await this._settingManager.GetSettingLocal<bool>(SettingConstants.AutoCollapseNavbar_Key); ;
            this._autoRedirect = await this._settingManager.GetSettingLocal(SettingConstants.AutoRedirectTo);

            await base.OnInitializedAsync();
        }
        private async Task UpdateMainUrl(ChangeEventArgs e)
        {
            await this._settingManager.UpdateMainUrl((string)e.Value);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task UpdateAutoCollapeseNavBar(ChangeEventArgs e)
        {
            await this._settingManager.StoreSettingLocal<bool>(SettingConstants.AutoCollapseNavbar_Key, (bool)e.Value);
            await this.InvokeAsync(this.StateHasChanged);
        }


        private async Task OnAutoRedirectChanged(ChangeEventArgs e)
        {
            if (e.Value is string autoRedirect && autoRedirect != this._autoRedirect)
            {
                await this._settingManager.SetAutoRedirectTo(autoRedirect);
                this._autoRedirect = autoRedirect;
            }
        }

    }
}
