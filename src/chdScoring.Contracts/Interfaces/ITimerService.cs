using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace chdScoring.Contracts.Interfaces
{
    public interface ITimerService
    {
        Task<bool> HandleOperation(TimerOperationDto dto, CancellationToken cancellationToken);
        Task<bool> SaveRound(SaveRoundDto dto, CancellationToken cancellation);
    }
}
