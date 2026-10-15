using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.Contracts.Dtos;

namespace chdScoring.Contracts.Interfaces
{
    public interface IDeviceService
    {
        Task<List<DeviceStatusDto>> GetAllAsync(CancellationToken cancellationToken);
        Task NavigateTo(DeviceStatusDto dto, string redirect, CancellationToken cancellationToken);
        Task SetAutoRedirect(DeviceStatusDto dto, string redirect, CancellationToken cancellationToken);
        Task SetApiKey(DeviceStatusDto dto, string apiKey, CancellationToken cancellationToken);
        Task SetZoom(DeviceStatusDto dto, int zoom, CancellationToken cancellationToken);
        Task ShutDown(DeviceStatusDto dto, bool restart, CancellationToken cancellationToken);
        Task Identify(DeviceStatusDto dto, CancellationToken cancellationToken);
        Task ToggleTheme(DeviceStatusDto dto, CancellationToken cancellationToken);
    }
}
