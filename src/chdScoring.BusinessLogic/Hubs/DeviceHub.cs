using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chd.Hub.Base.Server;
using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace chdScoring.BusinessLogic.Hubs
{
    public class DeviceHub(ILogger<DeviceHub> logger, IDeviceCacheService deviceCacheService, IApiLogger apiLogger)
        : BaseAuthenticationHub<IDeviceHub>(logger), IDeviceHub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await apiLogger.Log($"{Context.GetHttpContext().Request.Host} connected ID:{Context.ConnectionId}, API-Key:{GetApiKey()}");
            deviceCacheService.AddConnection(Context.ConnectionId, GetApiKey());
            await SendDevicesToGroup();
        }

        public Task RegisterDeviceAsync(DeviceStatusDto dto)
        {
            dto.ApiKey = GetApiKey();
            dto.LastUpdate = DateTime.Now;
            dto.ConnectionId = Context.ConnectionId;
            deviceCacheService.SetDevice(Context.ConnectionId, dto);
            return SendDevicesToGroup();
        }

        public async Task RegisterForDevices()
        {
            await this.Groups.AddToGroupAsync(Context.ConnectionId, HubContants.Device_Group, Context.ConnectionAborted);
            await this.Clients.Caller.GetDevices(deviceCacheService.GetAll());
        }

        public Task UnRegisterForDevices()
            =>this.Groups.RemoveFromGroupAsync(Context.ConnectionId, HubContants.Device_Group, Context.ConnectionAborted);
        

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

        public Task GetDevices(List<DeviceStatusDto> devices) => Task.CompletedTask;

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            deviceCacheService.RemoveConnection(Context.ConnectionId);
            await SendDevicesToGroup();
            await base.OnDisconnectedAsync(exception);
        }

        private Task SendDevicesToGroup()
            => this.Clients.Group(HubContants.Device_Group).GetDevices(deviceCacheService.GetAll());
    }
}
