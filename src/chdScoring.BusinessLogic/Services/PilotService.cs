using chdScoring.Contracts.Dtos;
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
    public class PilotService : IPilotService
    {
        private readonly IPilotDAL _dal;
        private readonly IHubDataService _hubDataService;
        private readonly IFlightCacheService _flightCacheService;

        public PilotService(IPilotDAL dal, IHubDataService hubDataService, IFlightCacheService flightCacheService)
        {
            this._dal = dal;
            this._hubDataService = hubDataService;
            this._flightCacheService = flightCacheService;
        }

        public Task<IEnumerable<OpenRoundDto>> GetOpenRound(int? round, CancellationToken cancellationToken)
        => this._dal.LoadOpenPilots(round, cancellationToken);

        public async Task<bool> SetPilotActive(LoadPilotDto dto, CancellationToken cancellationToken)
        {
            if (await this._dal.SetPilotActive(dto, cancellationToken))
            {
                await this._flightCacheService.Update(cancellationToken);
                await this._hubDataService.SendAll(cancellationToken);
                return true;
            }
            return false;
        }
    }
}
