using chd.Api.Base.Contracts.Interfaces;
using chd.Hub.Base.Client;
using chd.UI.Base.Contracts.Interfaces.Update;
using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace chdScoring.App.UI.Helper
{
    public class DeviceHubClient(
        ILogger<BaseAuthenticationHubClient<IDeviceHub>> logger,
        NavigationManager navigationManager,
        IchdScoringProfileService profileService,
        IBatteryService batteryService,
        IUpdateService updateService,
        ISettingManager settingManager,
        IApiKeyProvider apiKeyProvider)
        : BaseAuthenticationHubClient<IDeviceHub>(logger, apiKeyProvider), IDeviceHubClient
    {
        private DeviceStatusDto _currentDeviceStatus;

        public event EventHandler<string> ChangeNavigation;
        public event EventHandler<string> ChangeAutoRedirect;
        public event EventHandler<int> ChangeZoom;
        public event EventHandler ToggleTheme;
        public event EventHandler<string> ChangeApiKey;

        public Task Update(CancellationToken cancellationToken) => SendDto(cancellationToken);

        protected override void SpecificReinitialize(HubConnection connection)
        {
            profileService.UserChanged -= ProfileService_UserChanged;

            connection?.Remove(nameof(IDeviceHub.NavigateTo));
            connection?.Remove(nameof(IDeviceHub.SetAutoRedirect));
            connection?.Remove(nameof(IDeviceHub.SetZoom));
            connection?.Remove(nameof(IDeviceHub.SetApiKey));
            connection?.Remove(nameof(IDeviceHub.ShutDown));
            connection?.Remove(nameof(IDeviceHub.Identify));
            connection?.Remove(nameof(IDeviceHub.ToggleTheme));
        }

        protected override Task<bool> ShouldInitialize(CancellationToken cancellationToken) => Task.FromResult(true);

        protected override void HookIncomingCalls(HubConnection connection)
        {
            connection.On<DeviceStatusDto, string>(nameof(IDeviceHub.NavigateTo), (_, redirect) =>
            {
                this.ChangeNavigation?.Invoke(this, redirect);
            });
            connection.On<DeviceStatusDto, string>(nameof(IDeviceHub.SetAutoRedirect), (_, redirect) =>
            {
                this.ChangeAutoRedirect?.Invoke(this, redirect);
            });
            connection.On<DeviceStatusDto, int>(nameof(IDeviceHub.SetZoom), (_, zoom) =>
            {
                this.ChangeZoom?.Invoke(this, zoom);
            });
            connection.On<DeviceStatusDto, string>(nameof(IDeviceHub.SetApiKey), (_, apiKey) =>
            {
                this.ChangeApiKey(this, apiKey);
            });
            connection.On<DeviceStatusDto, bool>(nameof(IDeviceHub.ShutDown), (_, restart) =>
            {
                batteryService.ExitApplication(restart);
            });
            connection.On<DeviceStatusDto>(nameof(IDeviceHub.Identify), (_) =>
            {
                batteryService.Identify();
            });
            connection.On<DeviceStatusDto>(nameof(IDeviceHub.ToggleTheme), (_) =>
            {
                ToggleTheme?.Invoke(this, EventArgs.Empty);
            });
        }

        protected override async Task DoInvokations(HubConnection connection, CancellationToken cancellationToken)
        {
            profileService.UserChanged += ProfileService_UserChanged;
            await SendDto(cancellationToken);
        }

        private async void ProfileService_UserChanged(object? sender, chd.UI.Base.Contracts.Dtos.Authentication.UserDto<int, int> e)
        {
            this._currentDeviceStatus.User = e?.FullName;
            await SendDto(CancellationToken.None);
        }

        protected override async Task<Uri> LoadUri(CancellationToken cancellationToken)
        {
            var baseAddress = await settingManager.MainUrl;
            //return new UriBuilder($"http://localhost:8081/chdscoring/device-hub").Uri;
            return new UriBuilder($"{baseAddress}chdscoring/device-hub").Uri;
        }

        private Task SendDto(CancellationToken cancellationToken)
        => this.SendAsync(async conn =>
            {
                await conn.SendAsync(nameof(IDeviceHub.RegisterDeviceAsync), await GetCurrent(cancellationToken), cancellationToken);
            });

        private async Task<DeviceStatusDto> GetCurrent(CancellationToken cancellationToken)
        {
            if (_currentDeviceStatus is null)
            {
                _currentDeviceStatus = new DeviceStatusDto()
                {
                    CurrentLocalTime = DateTime.Now,
                    User = profileService.CsUser?.FullName,
                    CurrentSite = navigationManager.ToBaseRelativePath(navigationManager.Uri),
                    CurrentVersion = await updateService.CurrentVersion(),
                    HostName = batteryService.DeviceName,
                    BatteryLevel = batteryService.BatteryLevel,
                    BatteryCharging = batteryService.Charging,
                    Platform = batteryService.Platform,
                };
            }
            else
            {
                _currentDeviceStatus.CurrentLocalTime = DateTime.Now;
                _currentDeviceStatus.User = profileService.CsUser?.FullName;
                _currentDeviceStatus.CurrentSite = navigationManager.ToBaseRelativePath(navigationManager.Uri);
                _currentDeviceStatus.BatteryLevel = batteryService.BatteryLevel;
                _currentDeviceStatus.BatteryCharging = batteryService.Charging;
            }

            return _currentDeviceStatus;
        }
    }
}
