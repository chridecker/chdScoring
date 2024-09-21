using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace chdScoring.Contracts.Interfaces
{
    public interface IFlightCacheService
    {
        Task Update(CancellationToken cancellationToken);
        CurrentFlight GetCurrentFlight();
    }
}
