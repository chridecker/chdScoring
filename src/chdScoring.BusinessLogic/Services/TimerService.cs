using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class TimerService : ITimerService
    {
        private readonly ITimerDAL _dAL;

        public TimerService(ITimerDAL dAL)
        {
            this._dAL = dAL;
        }
        public Task<bool> HandleOperation(TimerOperationDto dto, CancellationToken cancellationToken)
        => (dto.Operation) switch
        {
            ETimerOperation.Start => this._dAL.HandleStart(dto, cancellationToken),
            ETimerOperation.Stop => this._dAL.HandleStop(dto, cancellationToken),
            _=> Task.FromResult(false),
        };
    }
   
}
