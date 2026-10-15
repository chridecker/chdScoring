using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.BusinessLogic.Hubs;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Security;

namespace chdScoring.BusinessLogic.Services
{
    public class DeviceService(IDeviceCacheService deviceCacheService, IHubContext<DeviceHub,IDeviceHub> hub) : IDeviceService
    {
        public Task<List<DeviceStatusDto>> GetAllAsync(CancellationToken cancellationToken) => Task.FromResult(deviceCacheService.GetAll());

        public Task NavigateTo(DeviceStatusDto dto, string redirect, CancellationToken cancellationToken)
            => hub.Clients.Client(dto.ConnectionId).NavigateTo(dto, redirect);

        public Task SetAutoRedirect(DeviceStatusDto dto, string redirect, CancellationToken cancellationToken)
            => hub.Clients.Client(dto.ConnectionId).SetAutoRedirect(dto, redirect);

        public Task SetApiKey(DeviceStatusDto dto, string apiKey, CancellationToken cancellationToken)
            => hub.Clients.Client(dto.ConnectionId).SetApiKey(dto, apiKey);

        public Task SetZoom(DeviceStatusDto dto, int zoom, CancellationToken cancellationToken)
            => hub.Clients.Client(dto.ConnectionId).SetZoom(dto, zoom);

        public Task ShutDown(DeviceStatusDto dto, bool restart, CancellationToken cancellationToken)
            => hub.Clients.Client(dto.ConnectionId).ShutDown(dto, restart);

        public Task Identify(DeviceStatusDto dto, CancellationToken cancellationToken)
            => hub.Clients.Client(dto.ConnectionId).Identify(dto);
        public Task ToggleTheme(DeviceStatusDto dto, CancellationToken cancellationToken)
            => hub.Clients.Client(dto.ConnectionId).ToggleTheme(dto);

    }
}
