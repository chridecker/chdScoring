using chdScoring.App.Services;
using Microsoft.AspNetCore.Components;
using chdScoring.Contracts.Interfaces;
using chd.Api.Base.Contracts.Interfaces;
using chd.Api.Base.Client.Extensions;
using chd.UI.Base.Contracts.Interfaces.Authentication;
using chd.UI.Base.Contracts.Interfaces.Services;

namespace chdScoring.App
{
    public partial class Main
    {
        [Inject] IProfileService<int, int> _profileService { get; set; }
        [Inject] ISettingManager _settingManager { get; set; }
        [Inject] IBaseUIComponentHandler _baseUIComponentHandler { get; set; }
        [Inject] NavigationManager _navManager { get; set; }

        private string _autoRedirect;

        protected override async Task OnInitializedAsync()
        {
            this._settingManager.AutoRedirectToChanged += this.AutoRedirectToChanged;
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var darkMode = await this._baseUIComponentHandler.GetDarkMode();
                await this._baseUIComponentHandler.SetDarkMode(darkMode);

                await this.ReloadAutoRedirect();
                if (this._navManager.Uri == this._navManager.BaseUri && !this._navManager.Uri.Contains(this._autoRedirect))
                {
                    this._navManager.NavigateTo($"{this._navManager.BaseUri}{this._autoRedirect}");
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async void AutoRedirectToChanged(object? sender, string e) => await this.ReloadAutoRedirect();

        private async Task ReloadAutoRedirect()
        {
            await Task.Delay(500);
            this._autoRedirect = await this._settingManager.GetAutoRedirectTo();
            if (string.IsNullOrWhiteSpace(this._autoRedirect))
            {
                this._autoRedirect = "";
            }
        }

        public void Dispose()
        {
            this._settingManager.AutoRedirectToChanged -= this.AutoRedirectToChanged;
        }
    }
}