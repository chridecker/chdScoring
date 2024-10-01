using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IJudgeService
    {
        Task<CurrentFlight> GetCurrentFlight(CancellationToken cancellationToken = default);

        Task<IEnumerable<JudgeDto>> GetJudges(CancellationToken cancellationToken = default);
    }
}
