using System;
using System.Collections.Generic;
using System.Text;
using chd.Hub.Base.Contracts.Interfaces;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.App.UI.Interfaces
{
    public interface IDeviceHubClient : IBaseHubClient<IDeviceHub>
    {
        Task Update(CancellationToken cancellationToken);
    }
}
