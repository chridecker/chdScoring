using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.DAL
{
    public class ScoreDAL : BaseDAL, IScoreDAL
    {
        private readonly IWertungHistoryRepository _wertungHistoryRepository;

        public ScoreDAL(ILogger<ScoreDAL> logger, IWettkampfLeitungRepository wettkampfLeitungRepository, IWertungHistoryRepository wertungHistoryRepository,
            ITeilnehmerRepository teilnehmerRepository, IJudgeRepository judgeRepository, IFigurRepository figurRepository, IProgrammRepository programmRepository, IWertungRepository wertungRepository, IKlasseRepository klasseRepository, ICountryImageRepository countryImageRepository, IImageRepository imageRepository, IDurchgangPanelRepository durchgangPanelRepository, IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository, IJudgePanelRepository judgePanelRepository, IStammDatenRepository stammDatenRepository, IBebwerbRepository bebwerbRepository, IDurchgangRepository durchgangRepository, ITeilnehmerBewerbRepository teilnehmerBewerbRepository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, figurProgrammRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
            this._wertungHistoryRepository = wertungHistoryRepository;
        }

        public async Task<NotificationDto> CreateZeroNotification(SaveScoreDto dto)
        {
            var judge = await this._judgeRepository.FirstOrDefaultAsync(x => x.Id == dto.Judge);
            var pilot = await this._teilnehmerRepository.FirstOrDefaultAsync(x => x.Id == dto.Pilot);
            var message = $"Judge: {dto.Judge} {judge.Vorname.Substring(0, 1)} {judge.Name.ToUpper()}{Environment.NewLine}" +
                $"Pilot: {dto.Pilot} {pilot.FullName}{Environment.NewLine}" +
                $"Figur: {dto.Figur} -> {dto.Value}";

            return new NotificationDto($"Wertung '{dto.Value}'", message);
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
                    await this._wertungHistoryRepository.SaveAsync(new Wertung_History
                    {
                        Durchgang = dto.Round,
                        Figur = dto.Figur,
                        Judge = dto.Judge,
                        Teilnehmer = dto.Pilot,
                        Wert_alt = (float)score.Wert,
                        Wert_neu = (float)dto.Value,
                        Time = DateTime.Now,
                        User = dto.User
                    }, cancellationToken);
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
