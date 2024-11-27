using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.DAL;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class PilotService : IPilotService
    {
        private readonly IPilotDAL _dal;
        private readonly ICurrentFlightDAL _cDal;
        private readonly IHubDataService _hubDataService;
        private readonly IFlightCacheService _flightCacheService;

        public PilotService(IPilotDAL dal, ICurrentFlightDAL cDal, IHubDataService hubDataService, IFlightCacheService flightCacheService)
        {
            this._dal = dal;
            this._cDal = cDal;
            this._hubDataService = hubDataService;
            this._flightCacheService = flightCacheService;
        }

        public Task<IEnumerable<OpenRoundDto>> GetOpenRound(int? round, CancellationToken cancellationToken)
        => this._dal.LoadOpenPilots(round, cancellationToken);

        public Task<IEnumerable<RoundResultDto>> GetRoundResult(int? round, CancellationToken cancellationToken)
       => this._dal.LoadRoundResults(round, cancellationToken);

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
        public async Task<bool> UnLoadPilot(LoadPilotDto dto, CancellationToken cancellationToken)
        {
            if (await this._dal.UnLoadPilot(dto, cancellationToken))
            {
                await this._flightCacheService.Update(cancellationToken);
                await this._hubDataService.SendAll(cancellationToken);
                return true;
            }
            return false;
        }

        public Task<IEnumerable<FinishedRoundDto>> GetFinishedFlights(CancellationToken cancellationToken = default) => this._dal.GetFinishedFlights(cancellationToken);

        public Task<RoundDataDto> GetRoundData(int pilot, int round, CancellationToken cancellationToken)
        => this._cDal.GetRoundData(pilot, round, cancellationToken);
    }
}
