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
        private readonly IHubDataService _hubDataService;
        private readonly IFlightCacheService _flightCacheService;

        public TimerService(ITimerDAL dAL, IHubDataService hubDataService, IFlightCacheService flightCacheService)
        {
            this._dAL = dAL;
            this._hubDataService = hubDataService;
            this._flightCacheService = flightCacheService;
        }
        public Task<bool> HandleOperation(TimerOperationDto dto, CancellationToken cancellationToken)
        => (dto.Operation) switch
        {
            ETimerOperation.Start => this.Start(dto, cancellationToken),
            ETimerOperation.Stop => this.Stop(dto, cancellationToken),
            _ => Task.FromResult(false),
        };

        public async Task<bool> SaveRound(SaveRoundDto dto, CancellationToken cancellation)
        {
            var returnVal = await this._dAL.SaveRound(dto, cancellation);
            if (dto.StopTimer)
            {
                return returnVal && await this.Stop(new TimerOperationDto
                {
                    Airfield = 1,
                    Operation = ETimerOperation.Stop
                }, cancellation);
            }
            return returnVal;
        }

        private async Task<bool> Start(TimerOperationDto dto, CancellationToken cancellationToken)
        {
            if (await this._dAL.HandleStart(dto, cancellationToken))
            {
                await Task.Delay(500, cancellationToken);
                await this._flightCacheService.Update(cancellationToken);
                await this._hubDataService.SendAll(cancellationToken);
                return true;
            }
            return false;
        }
        private async Task<bool> Stop(TimerOperationDto dto, CancellationToken cancellationToken)
        {
            if (await this._dAL.HandleStop(dto, cancellationToken))
            {
                await this._flightCacheService.Update(cancellationToken);
                await this._hubDataService.SendAll(cancellationToken);
                return true;
            }
            return false;
        }
    }

}
