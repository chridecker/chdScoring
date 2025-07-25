using chdScoring.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace chdScoring.Contracts.Interfaces
{
    public interface IFlightCacheService
    {
        Task Update(CancellationToken cancellationToken);
        CurrentFlight GetCurrentFlight(DateTime currentDateTime);
        Task UpdateRoundResults(CancellationToken cancellationToken);
        List<RoundResultDto> GetCurrentRoundResults();
    }
}
