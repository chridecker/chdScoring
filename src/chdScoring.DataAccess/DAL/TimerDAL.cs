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
    }
}
