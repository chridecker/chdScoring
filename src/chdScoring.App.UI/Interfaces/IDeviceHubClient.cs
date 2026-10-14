using System;
using System.Collections.Generic;
using System.Text;
using chd.Hub.Base.Contracts.Interfaces;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.App.UI.Interfaces
{
    public interface IDeviceHubClient : IBaseHubClient<IDeviceHub>
    {
        event EventHandler<string> ChangeNavigation;
        event EventHandler<string> ChangeAutoRedirect;
        event EventHandler<int> ChangeZoom;
        event EventHandler<string> ChangeApiKey;
        Task Update(CancellationToken cancellationToken);
    }
}
