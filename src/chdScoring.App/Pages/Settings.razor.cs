using Microsoft.AspNetCore.Components;
using chdScoring.App.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.App.Constants;
using chd.UI.Base.Components.Base;
using chd.UI.Base.Components.General;
using chd.UI.Base.Contracts.Extensions;

namespace chdScoring.App.Pages
{
    public partial class Settings : PageComponentBase<int, int>
    {
        [Inject] private ISettingManager _settingManager { get; set; }

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private string _baseAddress = string.Empty;
        private bool _autocollapseNav = false;
        private string _autoRedirect;
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

            this._autocollapseNav = await this._settingManager.GetSettingLocal<bool>(SettingConstants.AutoCollapseNavbar_Key); ;
            this._autoRedirect = await this._settingManager.GetSettingLocal(SettingConstants.AutoRedirectTo);

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
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task UpdateAutoCollapeseNavBar(ChangeEventArgs e)
        {
            await this._settingManager.StoreSettingLocal<bool>(SettingConstants.AutoCollapseNavbar_Key, (bool)e.Value);
            await this.InvokeAsync(this.StateHasChanged);
        }


        private async Task SelectedAutoRedirectChanged(KeyValuePair<string, RenderFragment>? val)
        {
            this._selectedAutoRedirect = val;
            await this.InvokeAsync(this.StateHasChanged);
        }

    }
}
