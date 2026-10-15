using chd.Hub.Base.Contracts.Interfaces;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.App.UI.Interfaces
{
    public interface IDeviceHubClient : IBaseHubClient<IDeviceHub>
    {
        event EventHandler<string> ChangeNavigation;
        event EventHandler<string> ChangeAutoRedirect;
        event EventHandler<int> ChangeZoom;
        event EventHandler<string> ChangeApiKey;
        Task Update(CancellationToken cancellationToken);
        event EventHandler<List<DeviceStatusDto>> OnDeviceLst;

        Task RegisterForDevices(CancellationToken cancellationToken);
        Task UnRegisterForDevices(CancellationToken cancellationToken);
        Task NavigateTo(DeviceStatusDto dto, string redirect, CancellationToken cancellationToken);
        Task SetAutoRedirect(DeviceStatusDto dto, string redirect, CancellationToken cancellationToken);
        Task SetApiKey(DeviceStatusDto dto, string apiKey, CancellationToken cancellationToken);
        Task SetZoom(DeviceStatusDto dto, int zoom, CancellationToken cancellationToken);
        Task ShutDown(DeviceStatusDto dto, bool restart, CancellationToken cancellationToken);
        Task Identify(DeviceStatusDto dto, CancellationToken cancellationToken);
    }
}
