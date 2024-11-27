using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.DataAccess.DAL
{
    public class CurrentFlightDAL : BaseDAL, ICurrentFlightDAL
    {
        public CurrentFlightDAL(ILogger<CurrentFlightDAL> logger,
            IWettkampfLeitungRepository wettkampfLeitungRepository, ITeilnehmerRepository teilnehmerRepository, IJudgeRepository judgeRepository,
            IFigurRepository figurRepository, IProgrammRepository programmRepository, IWertungRepository wertungRepository, IKlasseRepository klasseRepository,
            ICountryImageRepository countryImageRepository, IImageRepository imageRepository, IDurchgangPanelRepository durchgangPanelRepository,
            IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository, IJudgePanelRepository judgePanelRepository,
            IStammDatenRepository stammDatenRepository, IBebwerbRepository bebwerbRepository, IDurchgangRepository durchgangRepository, ITeilnehmerBewerbRepository teilnehmerBewerbRepository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, figurProgrammRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
        }

        public async Task<CurrentFlight> GetCurrentFlightData(CancellationToken cancellationToken)
        {
            CurrentFlight dto = null;
            try
            {
                var lst = await this._wettkampfLeitungRepository.CurrentRoundSet(cancellationToken);
                var currentPilot = lst.OrderBy(o => o.Start).FirstOrDefault(x => x.Status == (int)EFlightState.OnAir);

                if (currentPilot != null)
                {
                    var stammdaten = await this._stammDatenRepository.FindAll(cancellationToken);
                    var klasse = await this._klasseRepository.GetCurrentKlasse(cancellationToken);

                    var currentTime = DateTime.Now.TimeOfDay;
                    TimeSpan? time = currentPilot.Start_Time == TimeSpan.Zero || currentTime < currentPilot.Start_Time ? null : TimeSpan.FromMinutes(klasse.Zeit) - (currentTime - currentPilot.Start_Time);

                    dto = new CurrentFlight()
                    {
                        EditScoreEnabled = stammdaten.FirstOrDefault()?.Edit ?? false,
                        StartTime = currentPilot.Start_Time,
                        LeftTime = time.HasValue && time.Value < TimeSpan.Zero ? TimeSpan.Zero : time,
                    };
                    dto = await this.GetRoundData(dto, currentPilot, cancellationToken);
                    dto.Round.Time = TimeSpan.FromMinutes(klasse.Zeit);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, ex.Message);
            }
            return dto;
        }

        public async Task<RoundDataDto> GetRoundData(int pilot, int round, CancellationToken cancellationToken)
        {
            var dto = new RoundDataDto();
            var wl = await this._wettkampfLeitungRepository
                .Where(x => x.Status >= (int)EFlightState.OnAir)
                .Include(i => i.Pilot)
                .FirstOrDefaultAsync(x => x.Teilnehmer == pilot && x.Durchgang == round);

            return await this.GetRoundData(dto, wl, cancellationToken);
        }

        private async Task<T> GetRoundData<T>(T dto, Wettkampf_Leitung wl, CancellationToken cancellationToken)
            where T : RoundDataDto
        {
            var round = wl.Durchgang;
            var judges = await this._judgeRepository.GetRoundPanel(round, cancellationToken);
            var program = await this._programmRepository.GetProgramToRound(round, cancellationToken);
            var maneouvreLst = (await this._figurRepository.GetProgramToRound(round, cancellationToken)).OrderBy(o => o.Id);
            var scores = await this._wertungRepository.GetScoresToPilotInRound(wl.Teilnehmer, round, cancellationToken);

            dto.Pilot = new PilotDto { Id = wl.Pilot.Id, Name = wl.Pilot.FullName };
            dto.Judges = judges.Select(judge => new JudgeDto { Id = judge.Id, Name = $"{judge.Vorname} {judge.Name.ToUpper()}", EditScore = judge.EditScore });
            dto.Round = new RoundDto
            {
                Id = round,
                Program = program.Title,
            };

            foreach (var judge in judges.OrderBy(o => o.Id))
            {
                var dict = new Dictionary<int, IEnumerable<ManeouvreDto>>();
                var judgeScores = scores.Where(x => x.Judge == judge.Id);
                var figurs = new List<ManeouvreDto>();
                for (int i = 1; i <= maneouvreLst.Count(); i++)
                {
                    var element = maneouvreLst.ElementAt(i - 1);
                    figurs.Add(new ManeouvreDto
                    {
                        Id = i,
                        Name = element.Name,
                        Value = element.Wert,
                        Score = judgeScores.FirstOrDefault(x => x.Figur == i)?.Wert,
                        Saved = judgeScores.Any(x => x.Figur == i),
                    });
                }
                dto.ManeouvreLst[judge.Id] = figurs;
            }
            return dto;
        }
    }
}
