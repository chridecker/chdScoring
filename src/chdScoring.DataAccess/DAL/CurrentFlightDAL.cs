using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
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
    public class CurrentFlightDAL : BaseDAL, ICurrentFlightDAL
    {
        public CurrentFlightDAL(ILogger<CurrentFlightDAL> logger, IWettkampfLeitungRepository wettkampfLeitungRepository, ITeilnehmerRepository teilnehmerRepository, IJudgeRepository judgeRepository, IFigurRepository figurRepository, IProgrammRepository programmRepository, IWertungRepository wertungRepository, IKlasseRepository klasseRepository, ICountryImageRepository countryImageRepository, IImageRepository imageRepository, IDurchgangPanelRepository durchgangPanelRepository, IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository, IJudgePanelRepository judgePanelRepository, IStammDatenRepository stammDatenRepository, IBebwerbRepository bebwerbRepository, IDurchgangRepository durchgangRepository, ITeilnehmerBewerbRepository teilnehmerBewerbRepository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, figurProgrammRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
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
                    var klasse = await this._klasseRepository.GetCurrentKlasse(cancellationToken);
                    var round = currentPilot.Durchgang;
                    var pilot = await this._teilnehmerRepository.FindById(currentPilot.Teilnehmer, cancellationToken);
                    var judges = await this._judgeRepository.GetRoundPanel(round, cancellationToken);
                    var maneouvreLst = (await this._figurRepository.GetProgramToRound(round, cancellationToken)).OrderBy(o => o.Id);
                    var scores = await this._wertungRepository.GetScoresToPilotInRound(pilot.Id, round, cancellationToken);
                    var program = await this._programmRepository.GetProgramToRound(round, cancellationToken);

                    TimeSpan? time = currentPilot.Start_Time == DateTime.Today ? null : TimeSpan.FromMinutes(klasse.Zeit) - (DateTime.Now - currentPilot.Start_Time);

                    dto = new CurrentFlight()
                    {
                        LeftTime = time,
                        Pilot = new PilotDto { Id = pilot.Id, Name = $"{pilot.Vorname} {pilot.Nachname.ToLower()}" },
                        Judges = judges.Select(judge => new JudgeDto { Id = judge.Id, Name = $"{judge.Vorname} {judge.Name.ToUpper()}" }),
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
                                Current = i == judgeScores.Count() + 1,
                            });
                        }
                        dto.ManeouvreLst[judge.Id] = figurs;
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, ex.Message);
            }
            return dto;
        }
    }
}
