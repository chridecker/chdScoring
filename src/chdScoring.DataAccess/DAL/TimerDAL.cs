using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.DAL
{
    public class TimerDAL : BaseDAL, ITimerDAL
    {
        public TimerDAL(ILogger<TimerDAL> logger,
            IWettkampfLeitungRepository wettkampfLeitungRepository,
            ITeilnehmerRepository teilnehmerRepository,
            IJudgeRepository judgeRepository,
            IFigurRepository figurRepository,
            IProgrammRepository programmRepository,
            IWertungRepository wertungRepository,
            IKlasseRepository klasseRepository,
            ICountryImageRepository countryImageRepository,
            IImageRepository imageRepository, IDurchgangPanelRepository durchgangPanelRepository,
            IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository,
            IJudgePanelRepository judgePanelRepository, IStammDatenRepository stammDatenRepository, IBebwerbRepository bebwerbRepository, IDurchgangRepository durchgangRepository, ITeilnehmerBewerbRepository teilnehmerBewerbRepository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, figurProgrammRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
        }

        public async Task<bool> SaveRound(SaveRoundDto dto, CancellationToken cancellationToken)
        {
            var round = (await this._durchgangRepository.FirstOrDefaultAsync(x => x.Teilnehmer == dto.Pilot && x.Durchgang == dto.Round)) ?? new Round()
            {
                Durchgang = dto.Round,
                Teilnehmer = dto.Pilot,
                Duration = (int)dto.Duration.TotalSeconds,

            };
            round.Wert_abs = dto.Score;
            await this._durchgangRepository.SaveAsync(round, cancellationToken);

            var normBase = (await this.GetNormalizationBase(dto.Round, cancellationToken)) ?? dto.Score;
            await this._durchgangRepository.NoramlizeRound(dto.Round, normBase, cancellationToken);

            var wl = await this._wettkampfLeitungRepository.FirstOrDefaultAsync(x => x.Teilnehmer == dto.Pilot && x.Durchgang == dto.Round && x.Status == (int)EFlightState.OnAir);
            if (wl != null)
            {
                wl.Status = (int)EFlightState.Saved;
                await this._wettkampfLeitungRepository.SaveAsync(wl, cancellationToken);
            }
            return true;
        }

        public async Task<bool> HandleStart(TimerOperationDto dto, CancellationToken cancellationToken)
        {
            var wl = await this._wettkampfLeitungRepository.GetActiveOnAirfield(dto.Airfield, cancellationToken);
            if (wl != null)
            {
                wl.Start_Time = DateTime.Now.TimeOfDay;
                return await this._wettkampfLeitungRepository.SaveAsync(wl, cancellationToken);
            }
            return false;
        }

        public async Task<bool> HandleStop(TimerOperationDto dto, CancellationToken cancellationToken)
        {
            var wl = await this._wettkampfLeitungRepository.GetActiveOnAirfield(dto.Airfield, cancellationToken);
            if (wl != null)
            {
                wl.Start_Time = TimeSpan.Zero;
                return await this._wettkampfLeitungRepository.SaveAsync(wl, cancellationToken);
            }
            return false;
        }

        private async Task<decimal?> GetNormalizationBase(int round, CancellationToken cancellationToken)
        {
            var lst = await this._durchgangRepository.Where(x => x.Durchgang == round).ToListAsync();
            return lst.Any() ? lst.Max(m => m.Wert_abs) : null;
        }

        public async Task<int> GetFinishedRound(CancellationToken cancellationToken)
        {
            var wl = await this._wettkampfLeitungRepository.Where(x => x.Status == (int)EFlightState.Saved).OrderByDescending(o => o.Durchgang)?.FirstOrDefaultAsync();
            if (wl is null)
            {
                throw new Exception($"Keine offene Runde gefunden");
            }
            return wl.Durchgang;
        }
    }
}
