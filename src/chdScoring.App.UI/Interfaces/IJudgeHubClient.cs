using chd.Hub.Base.Client;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.App.UI.Interfaces
{
    public interface IJudgeHubClient : IBaseHubClient<IFlightHub>
    {
        event EventHandler<CurrentFlight> DataReceived;
        Task Register(int judge, CancellationToken cancellationToken = default);
        Task RegisterControlCenter(CancellationToken cancellationToken = default);
    }
}
