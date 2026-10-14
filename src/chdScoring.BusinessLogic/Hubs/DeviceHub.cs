using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chd.Hub.Base.Server;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;

namespace chdScoring.BusinessLogic.Hubs
{
    public class DeviceHub(ILogger<DeviceHub> logger, IDeviceCacheService deviceCacheService)
        : BaseAuthenticationHub<IDeviceHub>(logger), IDeviceHub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            deviceCacheService.AddConnection(Context.ConnectionId, _apiKey);
        }

        public Task RegisterDeviceAsync(DeviceStatusDto dto)
        {
            dto.ApiKey = _apiKey;
            dto.LastUpdate = DateTime.Now;
            dto.ConnectionId = Context.ConnectionId;
            deviceCacheService.SetDevice(Context.ConnectionId, dto);
            return Task.CompletedTask;
        }

        public async Task NavigateTo(DeviceStatusDto dto, string redirect)
        {
            await this.Clients.Client(dto.ConnectionId).NavigateTo(dto, redirect);
        }

        public async Task SetAutoRedirect(DeviceStatusDto dto, string redirect)
        {
            await this.Clients.Client(dto.ConnectionId).SetAutoRedirect(dto, redirect);
        }

        public async Task SetZoom(DeviceStatusDto dto, int zoom)
        {
            await this.Clients.Client(dto.ConnectionId).SetZoom(dto, zoom);
        }
        public async Task SetApiKey(DeviceStatusDto dto, string apiKey)
        {
            await this.Clients.Client(dto.ConnectionId).SetApiKey(dto, apiKey);
        }
        public async Task ShutDown(DeviceStatusDto dto, bool restart)
        {
            await this.Clients.Client(dto.ConnectionId).ShutDown(dto, restart);
        }

        public async Task Identify(DeviceStatusDto dto)
        {
            await this.Clients.Client(dto.ConnectionId).Identify(dto);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            deviceCacheService.RemoveConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
