using chd.Hub.Base.Client;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Interfaces
{
    public interface IJudgeHubClient : IBaseHubClient<IFlightHub>
    {
        event EventHandler<CurrentFlight> DataReceived;
        Task Register(int judge, CancellationToken cancellationToken = default);
        Task RegisterControlCenter(CancellationToken cancellationToken = default);
    }
}
