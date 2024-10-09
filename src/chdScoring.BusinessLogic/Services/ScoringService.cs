using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<bool> SaveScore(SaveScoreDto dto, CancellationToken cancellationToken)
        {
            if (await this._scoreDal.SaveScore(dto, cancellationToken))
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
