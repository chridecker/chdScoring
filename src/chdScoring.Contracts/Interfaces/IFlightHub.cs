using chdScoring.Contracts.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IFlightHub
    {
        Task ReceiveFlightData(CurrentFlight dto, CancellationToken cancellationToken = default);
        Task<bool> RegisterAsJudge(int judge);
        Task<bool> RegisterAsControlCenter();
        Task ReceiveNotification(NotificationDto dto, CancellationToken cancellationToken = default);
    }
}
