using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Extensions;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.DAL
{
    public class ScoreDAL : BaseDAL, IScoreDAL
    {
        private readonly IWertungHistoryRepository _wertungHistoryRepository;
        private readonly ITeilnehmerDurchgangJudgeRespository _teilnehmerDurchgangJudgeRespository;

        public ScoreDAL(ILogger<ScoreDAL> logger, IWettkampfLeitungRepository wettkampfLeitungRepository, IWertungHistoryRepository wertungHistoryRepository, ITeilnehmerDurchgangJudgeRespository teilnehmerDurchgangJudgeRespository,
            ITeilnehmerRepository teilnehmerRepository, IJudgeRepository judgeRepository, IFigurRepository figurRepository, IProgrammRepository programmRepository, IWertungRepository wertungRepository, IKlasseRepository klasseRepository, ICountryImageRepository countryImageRepository, IImageRepository imageRepository, IDurchgangPanelRepository durchgangPanelRepository, IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository, IJudgePanelRepository judgePanelRepository, IStammDatenRepository stammDatenRepository, IBebwerbRepository bebwerbRepository, IDurchgangRepository durchgangRepository, ITeilnehmerBewerbRepository teilnehmerBewerbRepository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, figurProgrammRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
            this._wertungHistoryRepository = wertungHistoryRepository;
            this._teilnehmerDurchgangJudgeRespository = teilnehmerDurchgangJudgeRespository;
        }

        public async Task<NotificationDto> CreateZeroNotification(SaveScoreDto dto)
        {
            var judge = await this._judgeRepository.FirstOrDefaultAsync(x => x.Id == dto.Judge);
            var pilot = await this._teilnehmerRepository.FirstOrDefaultAsync(x => x.Id == dto.Pilot);
            var message = $"Judge: {dto.Judge} {judge.Vorname.Substring(0, 1)} {judge.Name.ToUpper()}{Environment.NewLine}" +
                $"Pilot: {dto.Pilot} {pilot.Vorname} {pilot.Nachname.ToUpper()}{Environment.NewLine}" +
                $"Figur: {dto.Figur} -> {dto.Value}";

            return new NotificationDto($"Wertung '{dto.Value}'", message);
        }

        public async Task<bool> HasNotObserved(SaveScoreDto dto, CancellationToken cancellationToken)
        {
            var scores = this._wertungRepository.Where(x => x.Durchgang == dto.Round && x.Teilnehmer == dto.Pilot && x.Figur == dto.Figur);
            return scores.Any(a => a.Wert < 0);
        }
        public async Task<bool> TryHandleNotObserved(SaveScoreDto dto, CancellationToken cancellationToken)
        {
            var jp = await this._judgePanelRepository.FirstOrDefaultAsync(x => x.Judge == dto.Judge);
            var judges = await this._judgePanelRepository.Where(x => x.Panel == jp.Panel).ToListAsync();
            var scores = await this._wertungRepository.Where(x => x.Durchgang == dto.Round && x.Teilnehmer == dto.Pilot && x.Figur == dto.Figur).ToListAsync();
            if (scores.Count < judges.Select(s => s.Judge).Distinct().Count())
            {
                return false;
            }
            foreach (var noScore in scores.Where(x => x.Wert < 0))
            {
                var avg = scores.Where(x => x.Wert >= 0).Average(x => x.Wert).RoundToNearestHalf();
                noScore.Wert = avg * (-1);
                await this._wertungRepository.SaveAsync(noScore, cancellationToken);
            }
            return true;
        }

        public async Task<bool> SaveScore(SaveScoreDto dto, CancellationToken cancellationToken)
        {
            var saved = false;
            if (await this._wertungRepository.Exists(dto.Pilot, dto.Round, dto.Figur, dto.Judge, cancellationToken))
            {
                return false;
            }
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
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, ex.Message);
                saved = false;
            }
            return saved;
        }
        public async Task<bool> ImportFlight(ImportRoundScoreDto dto, CancellationToken cancellationToken)
        {
            var saved = false;
            var round = await this._durchgangRepository.FirstOrDefaultAsync(x => x.Teilnehmer == dto.Pilot && x.Durchgang == dto.Round);
            if (round is null)
            {
                return saved;
            }
            try
            {
                foreach (var score in dto.Scores.OrderBy(o => o.Figure))
                {
                    if (await this._wertungRepository.Exists(dto.Pilot, dto.Round, score.Figure, dto.Judge, cancellationToken))
                    {
                        var existing = await this._wertungRepository.Find(dto.Pilot, dto.Round, score.Figure, dto.Judge, cancellationToken);
                        if (existing.Wert == score.Value)
                        {
                            continue;
                        }
                        await this._wertungHistoryRepository.SaveAsync(new Wertung_History
                        {
                            Durchgang = dto.Round,
                            Figur = score.Figure,
                            Judge = dto.Judge,
                            Teilnehmer = dto.Pilot,
                            Wert_alt = (float)existing.Wert,
                            Wert_neu = (float)score.Value,
                            Time = DateTime.Now,
                            User = 0
                        }, cancellationToken);
                        existing.Wert = score.Value;
                        await this._wertungRepository.SaveAsync(existing, cancellationToken);
                        saved = true;
                    }
                    else
                    {
                        saved = await this._wertungRepository.SaveAsync(new Wertung()
                        {
                            Durchgang = dto.Round,
                            Figur = score.Figure,
                            Judge = dto.Judge,
                            Teilnehmer = dto.Pilot,
                            Wert = score.Value
                        }, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
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

        public async Task<bool> ConfirmScores(ConfirmScoresDto saveScoreDto, CancellationToken cancellationToken)
        {
            if (await this._teilnehmerDurchgangJudgeRespository.Exists(saveScoreDto.Pilot, saveScoreDto.Round, saveScoreDto.Judge, cancellationToken))
            {
                return false;
            }
            return await this._teilnehmerDurchgangJudgeRespository.SaveAsync(new Teilnehmer_Durchgang_Judge()
            {
                Judge = saveScoreDto.Judge,
                Teilnehmer = saveScoreDto.Pilot,
                Durchgang = saveScoreDto.Round,
                Time = saveScoreDto.Time
            }, cancellationToken);

        }
        public async Task<bool> UnConfirmScores(ConfirmScoresDto saveScoreDto, CancellationToken cancellationToken)
        {
            if (await this._teilnehmerDurchgangJudgeRespository.Exists(saveScoreDto.Pilot, saveScoreDto.Round, saveScoreDto.Judge, cancellationToken))
            {
                var entry = await this._teilnehmerDurchgangJudgeRespository.FirstOrDefaultAsync(x => x.Judge == saveScoreDto.Judge && x.Teilnehmer == saveScoreDto.Pilot && x.Durchgang == saveScoreDto.Round);
                return await this._teilnehmerDurchgangJudgeRespository.Delete(entry);
            }
            return false;
        }
    }
}
