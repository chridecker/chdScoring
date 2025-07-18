using chd.UI.Base.Components.Base;
using chd.UI.Base.Components.General;
using chd.UI.Base.Contracts.Interfaces.Update;
using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Components;

namespace chdScoring.App.UI.Pages
{
    public partial class Settings : PageComponentBase<int, int>
    {
        [Inject] private ISettingManager _settingManager { get; set; }
        [Inject] private IUpdateService _updateService { get; set; }
        [Inject(Key = SettingConstants.AvailableLanguages)] private Task<Dictionary<string, string>> _availableLang { get; set; }

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private string _baseAddress = string.Empty;
        private Version _currentVersion;
        private bool _dropPanel = false;
        private bool _developerMode = false;
        private string _autoRedirect;
        private double _batteryLimit;
        private int _scoringZoom;
        private string _speechLanguage;
        private bool _useUix;
        private Dictionary<string, RenderFragment> _speechLanguages = new Dictionary<string, RenderFragment>();
        private Dictionary<string, RenderFragment> _redirectOptions = new Dictionary<string, RenderFragment>();

        private KeyValuePair<string, RenderFragment>? _selectedSpeechLanguage;
        private KeyValuePair<string, RenderFragment>? SelectedSpeechLanguage
        {
            get => this._selectedSpeechLanguage;
            set
            {
                this._selectedSpeechLanguage = value;
                this.SelectedSpeechLanguageChanged(value);
            }
        }


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
            this._dropPanel = await this._settingManager.GetSettingLocal<bool>(SettingConstants.DropPanel);
            this._developerMode = await this._settingManager.GetSettingLocal<bool>(SettingConstants.DeveloperMode);
            this._autoRedirect = await this._settingManager.GetSettingLocal(SettingConstants.AutoRedirectTo);
            this._batteryLimit = await this._settingManager.GetSettingLocal<double>(SettingConstants.BatteryWarningLimit);
            this._scoringZoom = await this._settingManager.GetSettingLocal<int>(SettingConstants.ScoringZoom);
            this._speechLanguage = await this._settingManager.GetSettingLocal(SettingConstants.SpeechLanguage);
            this._useUix = await this._settingManager.GetSettingLocal<bool>(SettingConstants.Use_UIX);

            await this.InitSpeechLanguages();
            this.InitSelection();

            await base.OnInitializedAsync();
        }

        private async Task InitSpeechLanguages()
        {
            this._speechLanguages.Add(string.Empty, this.CreateColorOption("None", "xmark"));
            foreach (var lang in await this._availableLang)
            {
                this._speechLanguages.Add(lang.Key, CreateColorOption(string.IsNullOrEmpty(lang.Value) ? lang.Key : lang.Value, "message"));
            }
            this._selectedSpeechLanguage = this._speechLanguages.FirstOrDefault(x => x.Key == this._speechLanguage);
        }


        private void InitSelection()
        {
            this._redirectOptions.Add("", CreateColorOption(PageTitleConstants.Scoring, "whistle"));
            this._redirectOptions.Add("controlcenter", CreateColorOption(PageTitleConstants.ControlCenter, "calculator"));
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
            if (e.Value is not string val) { return; }
            await this._settingManager.UpdateMainUrl(val);
            this._settingManager.SetNativSetting(SettingConstants.BaseAddress, val);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task UpdateBatteryLimit(ChangeEventArgs e)
        {
            if (e.Value is not string val || !double.TryParse(val, out var limit)) { return; }
            await this._settingManager.StoreSettingLocal<double>(SettingConstants.BatteryWarningLimit, limit);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task UpdateScoringZoom(ChangeEventArgs e)
        {
            if (e.Value is not string val || !int.TryParse(val, out var zoom))
            {
                this._scoringZoom = 100;
                return;
            }
            this._scoringZoom = zoom;
            await this._settingManager.StoreSettingLocal<int>(SettingConstants.ScoringZoom, this._scoringZoom);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task SelectedSpeechLanguageChanged(KeyValuePair<string, RenderFragment>? val)
        {
            this._selectedSpeechLanguage = val;
            if (val.HasValue)
            {
                await this._settingManager.StoreSettingLocal(SettingConstants.SpeechLanguage, val.Value.Key);
            }
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task UpdateDeveloperMode(ChangeEventArgs e)
        {
            await this._settingManager.StoreSettingLocal<bool>(SettingConstants.DeveloperMode, (bool)e.Value);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task UpdateDropPanel(ChangeEventArgs e)
        {
            await this._settingManager.StoreSettingLocal<bool>(SettingConstants.DropPanel, (bool)e.Value);
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
