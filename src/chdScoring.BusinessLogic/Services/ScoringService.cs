using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
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
        private readonly IWertungRepository _wertungRepository;
        private readonly IFlightCacheService _flightCacheService;
        private readonly IHubDataService _hubDataService;

        public ScoringService(ILogger<ScoringService> logger, IWertungRepository wertungRepository, IFlightCacheService flightCacheService, IHubDataService hubDataService)
        {
            this._logger = logger;
            this._wertungRepository = wertungRepository;
            this._flightCacheService = flightCacheService;
            this._hubDataService = hubDataService;
        }

        public async Task<bool> SaveScore(SaveScoreDto dto, CancellationToken cancellationToken)
        {
            var saved = false;
            if (await this._wertungRepository.Exists(dto.Pilot, dto.Round, dto.Figur, dto.Judge, cancellationToken))
            {
                return false;
            }

            await this._wertungRepository.CreateTransaction(cancellationToken);
            try
            {
                saved = await this._wertungRepository.SaveAsync(new Wertung()
                {
                    Durchgang = dto.Round,
                    Figur = dto.Figur,
                    Judge = dto.Judge,
                    Teilnehmer = dto.Pilot,
                    Wert = dto.Value
                }, cancellationToken);
                await this._wertungRepository.Commit(cancellationToken);
                await this._flightCacheService.Update(cancellationToken);
                await this._hubDataService.SendJudge(dto.Judge, cancellationToken);

            }
            catch (Exception ex)
            {
                await this._wertungRepository.Rollback(cancellationToken);
                this._logger?.LogError(ex, ex.Message);
                saved = false;
            }
            return saved;
        }
    }
}
