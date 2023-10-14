using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IFlightHub
    {
        Task ReceiveFlightData(CurrentFlight dto);
        Task<bool> RegisterAsJudge(int judge, CancellationToken cancellationToken = default);
    }
}
