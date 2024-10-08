using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.DAL
{
    public class ScoreDAL : BaseDAL, IScoreDAL
    {
        public ScoreDAL(ILogger<ScoreDAL> logger, IWettkampfLeitungRepository wettkampfLeitungRepository,
            ITeilnehmerRepository teilnehmerRepository, IJudgeRepository judgeRepository, IFigurRepository figurRepository, IProgrammRepository programmRepository, IWertungRepository wertungRepository, IKlasseRepository klasseRepository, ICountryImageRepository countryImageRepository, IImageRepository imageRepository, IDurchgangPanelRepository durchgangPanelRepository, IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository, IJudgePanelRepository judgePanelRepository, IStammDatenRepository stammDatenRepository, IBebwerbRepository bebwerbRepository, IDurchgangRepository durchgangRepository, ITeilnehmerBewerbRepository teilnehmerBewerbRepository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, figurProgrammRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
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
            }
            catch (Exception ex)
            {
                await this._wertungRepository.Rollback(cancellationToken);
                this._logger?.LogError(ex, ex.Message);
                saved = false;
            }
            return saved;
        }

        public async Task<bool> UpdateScore(SaveScoreDto dto, CancellationToken cancellationToken)
        {
            if (!(await this._wertungRepository.Exists(dto.Pilot, dto.Round, dto.Figur, dto.Judge, cancellationToken)))
            {
                return false;
            }
            try
            {
                var score = await this._wertungRepository.Find(dto.Pilot, dto.Round, dto.Figur, dto.Judge, cancellationToken);
                if (score != null)
                {
                    score.Wert = dto.Value;
                    await this._wertungRepository.SaveAsync(score, cancellationToken);
                    return true;
                }
            }
            catch { }
            return false;

        }
    }
}
