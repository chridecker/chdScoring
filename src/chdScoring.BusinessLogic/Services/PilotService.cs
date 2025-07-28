using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Domain;
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


        public Task<bool> UpdatePilotData(PilotDto dto, CancellationToken cancellationToken = default) => this._dal.UpdatePilotData(dto, cancellationToken);

        public Task<bool> SetStartnumber(SetStartNumberDto dto, CancellationToken cancellationToken = default)
            => this._dal.ChangeStartNumber(dto.Pilot, dto.NewStartId, cancellationToken);


        public Task<IEnumerable<PilotDto>> GetAllPilots(CancellationToken cancellationToken = default)
        => this._dal.GetAllPilots(cancellationToken);

        public Task<IEnumerable<OpenRoundDto>> GetOpenRound(int? round, CancellationToken cancellationToken)
        => this._dal.LoadOpenPilots(round, cancellationToken);

        public Task<IEnumerable<RoundResultDto>> GetRoundResult(int? round, CancellationToken cancellationToken)
       => this._dal.LoadRoundResults(round, cancellationToken);


        public Task<bool> ReflightRound(ReflightRoundDto dto, CancellationToken cancellationToken = default) => this._dal.DeleteRoundScoring(dto.Pilot, dto.Round, cancellationToken);

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

        public async Task<ImageDto> GetCountryImage(int id, CancellationToken cancellationToken = default)
        {
            var c = await this._dal.GetCountryImage(id, cancellationToken);
            return new ImageDto
            {
                Data = c.Img_Data,
                Type = c.Img_Type
            };
        }
    }
}
