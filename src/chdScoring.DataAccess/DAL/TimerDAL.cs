using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            IImageRepository imageRepository, IDurchgangPanelRepository durchgangPanelRepository, IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository, IJudgePanelRepository judgePanelRepository, IStammDatenRepository stammDatenRepository, IBebwerbRepository bebwerbRepository, IDurchgangRepository durchgangRepository, ITeilnehmerBewerbRepository teilnehmerBewerbRepository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, figurProgrammRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
        }

        public async Task<bool> SaveRound(SaveRoundDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var round = (await this._durchgangRepository.FirstOrDefaultAsync(x => x.Teilnehmer == dto.Pilot && x.Id == dto.Round)) ?? new Durchgang()
                {
                    Id = dto.Round,
                    Teilnehmer = dto.Pilot,
                    Duration = (int)dto.Duration.TotalSeconds,

                };
                var normBase = (await this.GetNormalizationBase(dto.Round, cancellationToken)) ?? dto.Score;
                round.Wert_prom = (double)Math.Round(((dto.Score / normBase) * 1000), 2);
                round.Wert_abs = dto.Score;
                await this._durchgangRepository.SaveAsync(round, cancellationToken);
                return true;
            }
            catch
            {

            }
            return false;
        }

        public async Task<bool> HandleStart(TimerOperationDto dto, CancellationToken cancellationToken)
        {
            var wl = await this._wettkampfLeitungRepository.GetActiveOnAirfield(dto.Airfield, cancellationToken);
            if (wl != null)
            {
                wl.Start_Time = DateTime.Now.TimeOfDay;
                return await this._wettkampfLeitungRepository.UpdateAsync(wl, cancellationToken);
            }
            return false;
        }

        public async Task<bool> HandleStop(TimerOperationDto dto, CancellationToken cancellationToken)
        {
            var wl = await this._wettkampfLeitungRepository.GetActiveOnAirfield(dto.Airfield, cancellationToken);
            if (wl != null)
            {
                wl.Start_Time = DateTime.Today.TimeOfDay;
                return await this._wettkampfLeitungRepository.UpdateAsync(wl, cancellationToken);
            }
            return false;
        }

        private async Task<decimal?> GetNormalizationBase(int round, CancellationToken cancellationToken)
        {
            var lst = await this._durchgangRepository.Where(x => x.Id == round).ToListAsync();
            return lst.Any() ? lst.Max(m => m.Wert_abs) : null;
        }
    }
}
