using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chd.UI.Base.Contracts.Interfaces.Authentication;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Interfaces;
using Microsoft.AspNetCore.Components;

namespace chdScoring.App.UI.Shared
{
    public partial class NavMenu : ComponentBase, IDisposable
    {
        [Inject] private IchdScoringProfileService _profileService { get; set; }
        [Inject] private ITimeoutHandler _timeoutHandler { get; set; }
        [Inject] private ISettingManager _settingManager { get; set; }
        [Inject] private IToastService _toastService { get; set; }
        [Inject] private IBaseUIComponentHandler _uiHandler { get; set; }
        [Inject] private NavigationManager _navManager { get; set; }
        [Inject] private INavigationHandler _navigationHandler { get; set; }
        [Inject] private IServiceProvider _serviceProvider { get; set; }

        [Parameter] public bool Visible { get; set; } = true;


        private bool _small = true;

        private bool collapseNavMenu = true;
        private bool _useUix;

        private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        protected override async Task OnInitializedAsync()
        {
            this._useUix = await this._settingManager.GetSettingLocal<bool>(SettingConstants.Use_UIX);
            this.RegisterEvents();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool first)
        {
            await this.CheckWindowSize();

            await base.OnAfterRenderAsync(first);
        }

        private void GoHome()
        {
            this._navigationHandler.NavigateToRoot();
        }

        private void RegisterEvents()
        {
            this._profileService.UserChanged += this.UserChanged;
            this._timeoutHandler.TimeoutExpires += this._timeoutHandler_TimeoutExpires;
        }
        private void DeregisterEvents()
        {
            this._profileService.UserChanged -= this.UserChanged;
            this._timeoutHandler.TimeoutExpires -= this._timeoutHandler_TimeoutExpires;
        }

        private async void UserChanged(object? sender, UserDto<int, int> user) => await this.InvokeAsync(this.StateHasChanged);

        private async void _timeoutHandler_TimeoutExpires(object? sender, int left)
        {
            if (left > 0)
            {
                this._toastService.ShowError($"Automatische Abmeldung erfolgt in {left} Sekunden! Hier klicken um das Timeout zurückzusetzen", settings =>
                {
                    settings.OnClick = () => { this._timeoutHandler.LastAction = DateTime.Now; };
                    settings.IconType = IconType.None;
                });
            }
            else
            {
                await this._profileService.LogoutAsync();
            }
        }

        private async Task CheckWindowSize()
        {
            var dimension = await this._uiHandler.GetWindowDimensions();
            this._small = dimension.Width > 641;
        }

        public void Dispose() => this.DeregisterEvents();
    }
}