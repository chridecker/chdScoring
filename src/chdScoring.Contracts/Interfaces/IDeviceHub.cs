using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IDeviceHub
    {
        Task RegisterDeviceAsync(DeviceStatusDto dto);
        Task NavigateTo(DeviceStatusDto dto, string redirect);
        Task SetAutoRedirect(DeviceStatusDto dto, string redirect);
        Task SetApiKey(DeviceStatusDto dto, string apiKey);
        Task SetZoom(DeviceStatusDto dto, int zoom);
        Task ShutDown(DeviceStatusDto dto, bool restart);
        Task Identify(DeviceStatusDto dto);
        Task ToggleTheme(DeviceStatusDto dto);
    }
}
