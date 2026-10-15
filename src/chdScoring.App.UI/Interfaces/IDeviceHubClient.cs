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
        event EventHandler ToggleTheme;
        event EventHandler<int> ChangeZoom;
        event EventHandler<string> ChangeApiKey;
        Task Update(CancellationToken cancellationToken);
       
    }
}
