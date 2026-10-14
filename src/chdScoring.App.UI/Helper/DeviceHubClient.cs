using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;
using chd.Api.Base.Contracts.Interfaces;
using chd.Hub.Base.Client;
using chd.UI.Base.Contracts.Interfaces.Update;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

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

        public async Task Update(CancellationToken cancellationToken)
        {
            this._currentDeviceStatus.CurrentSite = navigationManager.ToBaseRelativePath(navigationManager.Uri);
            await SendDto(cancellationToken);
        }

        protected override void SpecificReinitialize(HubConnection connection)
        {
            profileService.UserChanged -= ProfileService_UserChanged;
        }

        protected override Task<bool> ShouldInitialize(CancellationToken cancellationToken) => Task.FromResult(true);

        protected override void HookIncomingCalls(HubConnection connection)
        {
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

        protected override Uri LoadUri()
        {
            //var baseAddress = settingManager.MainUrl.Result;
            return new UriBuilder($"http://localhost:8081/chdscoring/device-hub").Uri;
            //return new UriBuilder($"{baseAddress}chdscoring/device-hub").Uri;
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
