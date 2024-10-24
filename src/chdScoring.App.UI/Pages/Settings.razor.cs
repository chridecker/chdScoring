using Microsoft.AspNetCore.Components;
using chdScoring.App.UI.Constants;
using chd.UI.Base.Components.Base;
using chd.UI.Base.Components.General;
using chdScoring.App.UI.Interfaces;
using chd.UI.Base.Contracts.Interfaces.Update;

namespace chdScoring.App.UI.Pages
{
    public partial class Settings : PageComponentBase<int, int>
    {
        [Inject] private ISettingManager _settingManager { get; set; }
        [Inject] private IUpdateService _updateService { get; set; }
        [Inject] private IWifiService _wifiService { get; set; }

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private string _baseAddress = string.Empty;
        private Version _currentVersion;
        private bool _autocollapseNav = false;
        private bool _developerMode = false;
        private string _autoRedirect;
        private double _batteryLimit;
        private int _scoringZoom;
        private bool _useUix;
        private Dictionary<string, RenderFragment> _redirectOptions = new Dictionary<string, RenderFragment>();

        private KeyValuePair<string, RenderFragment>? _selectedAutoRedirect;
        private KeyValuePair<string, RenderFragment>? SelectedAutoRedirect
        {
            get => this._selectedAutoRedirect;
            set
            {
                this._selectedAutoRedirect = value;
                this.SelectedAutoRedirectChanged(value);
            }
        }



        protected override async Task OnInitializedAsync()
        {
            this.Title = PageTitleConstants.Settings;

            this._baseAddress = await this._settingManager.MainUrl;
            this._currentVersion = await this._updateService.CurrentVersion();
            this._autocollapseNav = await this._settingManager.GetSettingLocal<bool>(SettingConstants.AutoCollapseNavbar_Key);
            this._developerMode = await this._settingManager.GetSettingLocal<bool>(SettingConstants.DeveloperMode);
            this._autoRedirect = await this._settingManager.GetSettingLocal(SettingConstants.AutoRedirectTo);
            this._batteryLimit = await this._settingManager.GetSettingLocal<double>(SettingConstants.BatteryWarningLimit);
            this._scoringZoom = await this._settingManager.GetSettingLocal<int>(SettingConstants.ScoringZoom);
            this._useUix = await this._settingManager.GetSettingLocal<bool>(SettingConstants.Use_UIX);

            await this.InitSelection();

            await base.OnInitializedAsync();
        }
        private async Task InitSelection()
        {
            this._redirectOptions.Add("", CreateColorOption(PageTitleConstants.Index, "house"));
            this._redirectOptions.Add("controlcenter", CreateColorOption(PageTitleConstants.ControlCenter, "calculator"));
            this._redirectOptions.Add("scoring", CreateColorOption(PageTitleConstants.Scoring, "whistle"));
            this._redirectOptions.Add("competitionmanagement", CreateColorOption(PageTitleConstants.CompetitionManagement, "stopwatch"));
            this._selectedAutoRedirect = this._redirectOptions.FirstOrDefault(x => x.Key == this._autoRedirect);
        }

        private RenderFragment CreateColorOption(string text, string icon = "circle-check") => builder =>
                {
                    builder.OpenComponent(0, typeof(SelectionOption));
                    builder.AddAttribute(1, nameof(SelectionOption.FAClass), icon);
                    builder.AddAttribute(2, nameof(SelectionOption.Text), text);
                    builder.CloseComponent();
                };

        private async Task UpdateMainUrl(ChangeEventArgs e)
        {
            await this._settingManager.UpdateMainUrl((string)e.Value);
            this._settingManager.SetNativSetting(SettingConstants.BaseAddress, (string)e.Value);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task UpdateBatteryLimit(ChangeEventArgs e)
        {
            await this._settingManager.StoreSettingLocal<double>(SettingConstants.BatteryWarningLimit, double.Parse(e.Value.ToString()));
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task UpdateAutoCollapeseNavBar(ChangeEventArgs e)
        {
            await this._settingManager.StoreSettingLocal<bool>(SettingConstants.AutoCollapseNavbar_Key, (bool)e.Value);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task UpdateScoringZoom(ChangeEventArgs e)
        {
            this._scoringZoom = int.TryParse(e.Value.ToString(), out var val) ? val : 100;
            await this._settingManager.StoreSettingLocal<int>(SettingConstants.ScoringZoom, this._scoringZoom);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task UpdateDeveloperMode(ChangeEventArgs e)
        {
            await this._settingManager.StoreSettingLocal<bool>(SettingConstants.DeveloperMode, (bool)e.Value);
            await this.InvokeAsync(this.StateHasChanged);
        }
        
        private async Task UpdateUIX(ChangeEventArgs e)
        {
            await this._settingManager.StoreSettingLocal<bool>(SettingConstants.Use_UIX, (bool)e.Value);
            await this.InvokeAsync(this.StateHasChanged);
        }


        private async Task SelectedAutoRedirectChanged(KeyValuePair<string, RenderFragment>? val)
        {
            this._selectedAutoRedirect = val;
            if (val.HasValue)
            {
                await this._settingManager.StoreSettingLocal(SettingConstants.AutoRedirectTo, val.Value.Key);
            }
            await this.InvokeAsync(this.StateHasChanged);
        }

    }
}
