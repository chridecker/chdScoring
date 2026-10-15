using chd.Hub.Base.Client;
using chd.Hub.Base.Contracts.Interfaces;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.App.UI.Interfaces
{
    public interface IJudgeHubClient : IBaseHubClient<IFlightHub>
    {
        event EventHandler<CurrentFlight> DataReceived;
        Task Register(int judge, CancellationToken cancellationToken = default);
    }
}
