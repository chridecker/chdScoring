using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface ICompetitionService
    {
        Task ClearDataAsync(CancellationToken cancellationToken);
        Task StartAsync(CancellationToken cancellationToken);
    }
}
