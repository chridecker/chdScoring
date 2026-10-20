using chd.Hub.Base.Server;
using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Hubs
{
    public class DeviceHub(ILogger<DeviceHub> logger, IDeviceCacheService deviceCacheService, IApiLogger apiLogger)
        : BaseAuthenticationHub<IDeviceHub>(logger), IDeviceHub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var context = Context.GetHttpContext();
            var ip = context.Connection.RemoteIpAddress;
            var hostName = string.Empty;
            if (ip != null)
            {
                var hostEntry = await Dns.GetHostEntryAsync(ip);
                hostName = hostEntry.HostName;
            }

            await apiLogger.Log($"{(!string.IsNullOrWhiteSpace(hostName) ? hostName : ip)}({GetApiKey()}) connected ID: {Context.ConnectionId}");
            deviceCacheService.AddConnection(Context.ConnectionId, GetApiKey());
        }

        public Task RegisterDeviceAsync(DeviceStatusDto dto)
        {
            dto.ApiKey = GetApiKey();
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
        public async Task ToggleTheme(DeviceStatusDto dto)
        {
            await this.Clients.Client(dto.ConnectionId).ToggleTheme(dto);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            deviceCacheService.RemoveConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

    }
}
