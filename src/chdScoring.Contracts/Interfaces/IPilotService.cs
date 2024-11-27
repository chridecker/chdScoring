using chdScoring.Contracts.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IPilotService
    {
        Task<IEnumerable<FinishedRoundDto>> GetFinishedFlights(CancellationToken cancellationToken = default);
        Task<IEnumerable<OpenRoundDto>> GetOpenRound(int? round, CancellationToken cancellationToken);
        Task<IEnumerable<RoundResultDto>> GetRoundResult(int? round, CancellationToken cancellationToken);
        Task<bool> SetPilotActive(LoadPilotDto dto, CancellationToken cancellationToken);
        Task<bool> UnLoadPilot(LoadPilotDto dto, CancellationToken cancellationToken);
        Task<RoundDataDto> GetRoundData(int pilot, int round, CancellationToken cancellationToken);
    }
}
