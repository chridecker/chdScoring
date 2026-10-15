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
        public event EventHandler<string> ChangeApiKey;

        public async Task Update(CancellationToken cancellationToken)
        {
            this._currentDeviceStatus.CurrentSite = navigationManager.ToBaseRelativePath(navigationManager.Uri);
            await SendDto(cancellationToken);
        }

        public Task NavigateTo(DeviceStatusDto dto, string redirect) => SendHubCommand(nameof(IDeviceHub.NavigateTo), dto, redirect);

        public Task SetAutoRedirect(DeviceStatusDto dto, string redirect) => SendHubCommand(nameof(IDeviceHub.SetAutoRedirect), dto, redirect);

        public Task SetApiKey(DeviceStatusDto dto, string apiKey) => SendHubCommand(nameof(IDeviceHub.SetApiKey), dto, apiKey);

        public Task SetZoom(DeviceStatusDto dto, int zoom) => SendHubCommand(nameof(IDeviceHub.SetZoom), dto, zoom);

        public Task ShutDown(DeviceStatusDto dto, bool restart) => SendHubCommand(nameof(IDeviceHub.ShutDown), dto, restart);

        public Task Identify(DeviceStatusDto dto) => SendHubCommand(nameof(IDeviceHub.Identify), dto);

        protected override void SpecificReinitialize(HubConnection connection)
        {
            profileService.UserChanged -= ProfileService_UserChanged;
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

        private async Task SendHubCommand(string method, DeviceStatusDto dto, object param = null, CancellationToken cancellationToken = default)
        {
            await this.SendAsync(async conn =>
            {
                if (param is null)
                {
                    await conn.SendAsync(method, dto, param, cancellationToken);
                }
                else
                {
                    await conn.SendAsync(method, dto, cancellationToken);
                }
            });
        }

        private async Task SendDto(CancellationToken cancellationToken)
        {
            await this.SendAsync(async conn =>
            {
                await conn.SendAsync(nameof(IDeviceHub.RegisterDeviceAsync), await GetCurrent(cancellationToken), cancellationToken);
            });
        }

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
                };
            }

            return _currentDeviceStatus;
        }
    }
}
