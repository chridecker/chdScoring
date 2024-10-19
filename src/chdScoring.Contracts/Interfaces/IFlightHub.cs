using chdScoring.Contracts.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IFlightHub
    {
        Task ReceiveFlightData(CurrentFlight dto, CancellationToken cancellationToken = default);
        Task<bool> RegisterAsJudge(int judge);
        Task<bool> RegisterAsStatus();
        Task<bool> SendStatus(DeviceStatusDto dto);
        Task<bool> RegisterAsControlCenter();
        Task ReceiveNotification(NotificationDto dto, CancellationToken cancellationToken = default);
        Task ReceiveStatusRequest(CancellationToken cancellationToken = default);
    }
}
