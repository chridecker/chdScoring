using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.DAL;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class ScoringService : IScoringService
    {
        private readonly ILogger<ScoringService> _logger;
        private readonly IScoreDAL _scoreDal;
        private readonly IFlightCacheService _flightCacheService;
        private readonly IHubDataService _hubDataService;

        public ScoringService(ILogger<ScoringService> logger, IScoreDAL scoreDal, IFlightCacheService flightCacheService, IHubDataService hubDataService)
        {
            this._logger = logger;
            this._scoreDal = scoreDal;
            this._flightCacheService = flightCacheService;
            this._hubDataService = hubDataService;
        }

        public async Task<bool> ConfirmScores(ConfirmScoresDto saveScoreDto, CancellationToken cancellationToken)
        {
            var res = await this._scoreDal.ConfirmScores(saveScoreDto, cancellationToken);
            await this._flightCacheService.Update(cancellationToken);
            await this._hubDataService.SendJudge(saveScoreDto.Judge, cancellationToken);
            return res;
        }
        public async Task<bool> UnConfirmScores(ConfirmScoresDto saveScoreDto, CancellationToken cancellationToken)
        {
            var res = await this._scoreDal.UnConfirmScores(saveScoreDto, cancellationToken);
            await this._flightCacheService.Update(cancellationToken);
            await this._hubDataService.SendJudge(saveScoreDto.Judge, cancellationToken);
            return res;
        }

        public async Task<bool> SaveScore(SaveScoreDto dto, CancellationToken cancellationToken)
        {
            if (dto.Value == -99)
            {
                if (!await this._scoreDal.TryHandleNotObserved(dto, cancellationToken))
                {
                    return false;
                }
                return await this._scoreDal.SaveScore(dto, cancellationToken);
            }
            else if (await this._scoreDal.SaveScore(dto, cancellationToken))
            {
                if (dto.Value < 1 && dto.Value >= 0)
                {
                    await this._hubDataService.NotifyZero(await this._scoreDal.CreateZeroNotification(dto), cancellationToken);
                }
                await this._flightCacheService.Update(cancellationToken);
                await this._hubDataService.SendJudge(dto.Judge, cancellationToken);
            }
            return false;
        }

        public async Task<bool> UpdateScore(SaveScoreDto dto, CancellationToken cancellationToken)
        {
            if (await this._scoreDal.UpdateScore(dto, cancellationToken))
            {
                if (dto.Value < 1)
                {
                    await this._hubDataService.NotifyZero(await this._scoreDal.CreateZeroNotification(dto), cancellationToken);
                }

                await this._flightCacheService.Update(cancellationToken);
                await this._hubDataService.SendJudge(dto.Judge, cancellationToken);
            }
            return false;
        }
    }
}
