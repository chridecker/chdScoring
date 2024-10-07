using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.DAL.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.DAL
{
    public interface ITimerDAL : IBaseDAL
    {
        Task<bool> HandleStart(TimerOperationDto dto, CancellationToken cancellationToken);
        Task<bool>HandleStop(TimerOperationDto dto, CancellationToken cancellationToken);
        Task<bool> SaveRound(SaveRoundDto dto, CancellationToken cancellationToken);
    }
}
