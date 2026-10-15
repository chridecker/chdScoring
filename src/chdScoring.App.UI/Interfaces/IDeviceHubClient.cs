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

        Task NavigateTo(DeviceStatusDto dto, string redirect);
        Task SetAutoRedirect(DeviceStatusDto dto, string redirect);
        Task SetApiKey(DeviceStatusDto dto, string apiKey);
        Task SetZoom(DeviceStatusDto dto, int zoom);
        Task ShutDown(DeviceStatusDto dto, bool restart);
        Task Identify(DeviceStatusDto dto);


    }
}
